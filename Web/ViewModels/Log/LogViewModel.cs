using System;

namespace Web.ViewModels {
    public class LogViewModel {
        public Guid Id { get; set; }
        public string Application { get; set; }
        public DateTime Logged { get; set; }
        public string Level { get; set; }
        public string Message { get; set; }
        public string Logger { get; set; }
        public string Callsite { get; set; }
        public string Exception { get; set; }
        public string UserName { get; set; }
        public string Action { get; set; }
        public string Url { get; set; }
        public string Method { get; set; }
    }
}
