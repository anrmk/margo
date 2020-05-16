using System;

namespace Core.Data.Dto {
    public class CompanyGeneralDto {
        public long Id { get; set; }

        public string No { get; set; }
        public string Name { get; set; }
        public string PhoneNumber { get; set; }
        public string Website { get; set; }
        public DateTime? Founded { get; set; }
        public string EIN { get; set; }
        public string DB { get; set; }
        public string Email { get; set; }
        public string CEO { get; set; }
    }
}
