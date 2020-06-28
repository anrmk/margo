using Microsoft.AspNetCore.Mvc;

namespace Web.ViewModels {
    /// <summary>
    /// Filter for list paggination
    /// </summary>
    public class PagerFilterViewModel {
        [FromQuery(Name = "search[value]")]
        public string Search { get; set; } = "";

        [FromQuery(Name = "sort")]
        public string Sort { get; set; }

        [FromQuery(Name = "order")]
        public string Order { get; set; }

        [FromQuery(Name = "start")]
        public int Start { get; set; } = 0;

        [FromQuery(Name = "length")]
        public int Length { get; set; } = 10;

        public int Take { get; set; }
    }
}