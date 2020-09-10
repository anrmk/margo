using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

using Core.Data.Dto;
using Core.Extension;
using Core.Services.Base;

using Microsoft.Extensions.Logging;

namespace Core.Services.Managers {
    public interface ILogManager: IManager {
        Task<Tuple<List<LogDto>, int>> Pager(
            DateTime startDate,
            DateTime endDate,
            Func<LogDto, bool> where,
            Func<LogDto, object> order,
            bool descSort,
            int start,
            int length,
            bool isException);
        Task<List<LogDto>> FindAll(DateTime startDate, DateTime endDate, bool isException);
        Task<LogDto> Find(DateTime startDate, DateTime endDate, Guid id);
        void LogInfo(string message);
        void LogError(string message);
    }

    public class LogManager: ILogManager {
        private readonly ILogger<ILogManager> _logger;

        public LogManager(ILogger<ILogManager> logger) {
            _logger = logger;
        }

        public async Task<Tuple<List<LogDto>, int>> Pager(
                DateTime startDate,
                DateTime endDate,
                Func<LogDto, bool> where,
                Func<LogDto, object> order,
                bool descSort,
                int start,
                int length,
                bool isException) {
            var logs = await ReadAllFilesAsync(startDate, endDate, isException);
            var result = logs.Where(where);

            var count = result.Count();

            result = descSort
                ? result.OrderByDescending(order)
                : result.OrderBy(order);
            result = result.Skip(start).Take(length);

            return Tuple.Create(result.ToList(), count);
        }

        public async Task<List<LogDto>> FindAll(DateTime startDate, DateTime endDate, bool isException) {
            return await ReadAllFilesAsync(startDate, endDate, isException);
        }

        public async Task<LogDto> Find(DateTime startDate, DateTime endDate, Guid id) {
            for(var curDay = startDate.Date; curDay <= endDate.Date; curDay = curDay.AddDays(1)) {
                var errorLogs = await ReadLogAsync(curDay, true);
                foreach(var logRecord in IterateLogRecords(errorLogs, curDay, true)) {
                    if(logRecord.Id == id) {
                        logRecord.Level = "Error";
                        return logRecord;
                    }
                }
                var infoLogs = await ReadLogAsync(curDay, false);
                foreach(var logRecord in IterateLogRecords(infoLogs, curDay, false)) {
                    if(logRecord.Id == id) {
                        logRecord.Level = "Information";
                        return logRecord;
                    }
                }
            }
            throw new ArgumentException("Log record not found!");
        }

        public void LogInfo(string message) {
            _logger.LogInformation(message);
        }

        public void LogError(string message) {
            _logger.LogError(message);
        }

        private async Task<List<LogDto>> ReadAllFilesAsync(DateTime startDate, DateTime endDate, bool isException) {
            var result = new List<LogDto>();
            for(var curDay = startDate.Date; curDay <= endDate.Date; curDay = curDay.AddDays(1)) {
                foreach(var logRecord in IterateLogRecords(await ReadLogAsync(curDay, isException), curDay, isException)) {
                    result.Add(logRecord);
                }
            }
            return result;
        }

        private IEnumerable<LogDto> IterateLogRecords(string[] logRecords, DateTime curDate, bool isException) {
            foreach(var logRecord in logRecords) {
                var logRecordParts = logRecord.Split('|');

                if(!logRecordParts.LastOrDefault().TryParseJson(out LogDto data)) {
                    data = new LogDto { Message = logRecordParts.LastOrDefault() };
                }

                data.Logged = DateTime.TryParse(logRecordParts.FirstOrDefault(), out var date)
                    ? date : curDate.Date;

                data.Level = isException
                    ? "Error"
                    : "Information";

                yield return data;
            }
        }

        private async Task<string[]> ReadLogAsync(DateTime date, bool isException) {
            var localPath = isException ? "Errors" : "Activities";
            var fileName = $"{Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location)}\\Logs\\{localPath}\\{date:yyyy-MM-dd}.log";
            return !File.Exists(fileName)
                 ? new string[0]
                 : await File.ReadAllLinesAsync(fileName, Encoding.UTF8);
        }
    }
}
