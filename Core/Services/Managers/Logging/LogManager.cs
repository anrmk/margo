using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

using Core.Data.Dto;
using Core.Services.Base;

using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace Core.Services.Managers {
    public interface ILogManager: IManager {
        Task<Tuple<List<LogDto>, int>> InfoPager(
            DateTime startDate,
            DateTime endDate,
            Func<LogDto, bool> where,
            Func<LogDto, object> order,
            bool descSort,
            int start,
            int length);
        Task<List<LogDto>> FindAllInfo(DateTime startDate, DateTime endDate);
        Task<LogDto> FindInfo(DateTime startDate, DateTime endDate, Guid id);
        void LogInfo(string message);
        void LogError(string message);
    }

    public class LogManager: ILogManager{
        private readonly ILogger<ILogManager> _logger;

        public LogManager(ILogger<ILogManager> logger) {
            _logger = logger;
        }

        public async Task<Tuple<List<LogDto>, int>> InfoPager(
                DateTime startDate,
                DateTime endDate,
                Func<LogDto, bool> where,
                Func<LogDto, object> order,
                bool descSort,
                int start,
                int length) {
            var logs = await ReadAllFilesAsync(startDate, endDate);

            var result = logs.Where(where);
            result = descSort
                ? result.OrderByDescending(order)
                : result.OrderBy(order);
            result.Take(length).Skip(start);

            return Tuple.Create(result.ToList(), result.Count());
        }

        public async Task<List<LogDto>> FindAllInfo(DateTime startDate, DateTime endDate) {
            return await ReadAllFilesAsync(startDate, endDate);
        }

        public async Task<LogDto> FindInfo(DateTime startDate, DateTime endDate, Guid id) {
            for(var curDay = startDate.Date; curDay <= endDate.Date; curDay = curDay.AddDays(1)) {
                foreach(var logRecord in IterateLogRecords(await ReadFileAsync(curDay), curDay)) {
                    if(logRecord.Id == id)
                        return logRecord;
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

        private async Task<List<LogDto>> ReadAllFilesAsync(DateTime startDate, DateTime endDate) {
            var result = new List<LogDto>();
            for(var curDay = startDate.Date; curDay <= endDate.Date; curDay = curDay.AddDays(1)) {
                foreach(var logRecord in IterateLogRecords(await ReadFileAsync(curDay), curDay)) {
                    result.Add(logRecord);
                }
            }
            return result;
        }

        private IEnumerable<LogDto> IterateLogRecords(string[] logRecords, DateTime curDate) {
            foreach(var logRecord in logRecords) {
                var logRecordParts = logRecord.Split('|');
                var data = JsonConvert.DeserializeObject<LogDto>(logRecordParts.LastOrDefault());
                data.Logged = DateTime.TryParse(logRecordParts.FirstOrDefault(), out var date)
                    ? date : curDate.Date;

                yield return data;
            }
        }

        private async Task<string[]> ReadFileAsync(DateTime date) {
            var fileName = $"{Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location)}\\Logs\\Activities\\{date:yyyy-MM-dd}.log";
            return !File.Exists(fileName)
                 ? new string[0]
                 : await File.ReadAllLinesAsync(fileName, Encoding.UTF8);
        }
    }
}
