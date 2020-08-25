using Microsoft.AspNetCore.Mvc;

namespace Core.Filters {
#pragma warning disable 618
    public class LogActionAttribute: TypeFilterAttribute {
        public LogActionAttribute() : base(typeof(LogFilterAttribute)) {
            Arguments = new object[] { string.Empty };
        }
    }
#pragma warning restore 618
}
