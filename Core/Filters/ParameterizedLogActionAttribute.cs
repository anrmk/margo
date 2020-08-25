using Microsoft.AspNetCore.Mvc;

namespace Core.Filters {
#pragma warning disable 618
    public class ParameterizedLogActionAttribute: TypeFilterAttribute {
        public ParameterizedLogActionAttribute(string par) : base(typeof(LogFilterAttribute)) {
            Arguments = new object[] { par };
        }
    }
#pragma warning restore 618
}
