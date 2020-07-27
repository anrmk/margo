using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Core.Data.Entities
{
    [Table(name: "Persons")]
    public class UccountPersonEntity : EntityBase<long>
    {
        public virtual UccountEntity Account { get; set; }

        [Required]
        [MaxLength(100)]
        public string Name { get; set; }
    }
}
