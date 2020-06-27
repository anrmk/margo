using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Core.Data.Entities {
    [Table(name: "UccountServiceSections")]
    public class UccountServiceSectionEntity: EntityBase<long> {
        [Required]
        [MaxLength(24)]
        public string Name { get; set; }

        [ForeignKey("Account")]
        [Column("Account_Id")]
        public long? AccountId { get; set; }
        public virtual UccountEntity Uccount { get; set; }

        [Column("Section_Id")]
        public long? SectionId { get; set; }
        public virtual UccountServiceSectionEntity Section { get; set; }
    }
}
