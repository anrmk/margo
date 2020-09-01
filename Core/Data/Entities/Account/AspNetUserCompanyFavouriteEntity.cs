using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Core.Data.Entities {
    [Table(name: "CompanyFavourites")]
    public class AspNetUserCompanyFavouriteEntity: EntityBase<long> {
        [ForeignKey("Company")]
        [Column("Company_Id")]
        public Guid CompanyId { get; set; }
        public virtual CompanyEntity Company { get; set; }

        [ForeignKey("User")]
        [Column("User_Id")]
        public string UserId { get; set; }
        public virtual AspNetUserEntity User { get; set; }

        [Range(0, int.MaxValue)]
        public int Sort { get; set; }
    }
}
