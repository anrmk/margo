using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Core.Data.Entities {
    [Table(name: "AspNetUserDenyAccessCompanies")]
    public class AspNetUserDenyAccessCompanyEntity: EntityBase<Guid> {
        [ForeignKey("User")]
        [Column("User_Id")]
        [Required]
        public string UserId { get; set; }
        public virtual AspNetUserEntity User { get; set; }

        [Column("Company_Id")]
        public Guid CompanyId { get; set; }
        public virtual CompanyEntity Company { get; set; }
    }
}
