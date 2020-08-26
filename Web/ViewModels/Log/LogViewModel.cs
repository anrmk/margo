using System;

namespace Web.ViewModels {
    public class LogViewModel {
        public DateTime Logged { get; set; }
        public Guid Id { get; set; }
        public string UserAddress { get; set; }
        public string Level { get; set; }
        public string UserName { get; set; }
        public string Message { get; set; }
        public string Method { get; set; }
        public string Controller { get; set; }
        public string Action { get; set; }
        public string Url { get; set; }
        public string Arguments { get; set; }
    }
}
