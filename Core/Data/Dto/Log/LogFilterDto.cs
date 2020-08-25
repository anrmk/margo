using System;
using System.Net;

namespace Core.Data.Dto {
    public class LogFilterDto: PagerFilterDto {
        public string UserName { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public IPAddress UserIP { get; set; }
        public string Method { get; set; }
        public string Controller { get; set; }
        public string Action { get; set; }
    }
}
