using System;
using System.ComponentModel.DataAnnotations;

namespace Web.ViewModels {
    public class AspNetUserCompanyFavouriteViewModel {
        public long Id { get; set; }

        public string UserId { get; set; }

        public Guid? CompanyId { get; set; }

        public string CompanyName { get; set; }

        public string CompanyDescription { get; set; }

        [DataType(DataType.Date)]
        public DateTime? CompanyFounded { get; set; }

        public string CompanyEIN { get; set; }

        public string CompanyDB { get; set; }

        [Range(0, int.MaxValue)]
        public int Sort { get; set; }
    }
}
