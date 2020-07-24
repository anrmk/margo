using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Core.Data.Entities
{
    [Table(name: "Services")]
    public class ServiceEntity : EntityBase<long>
    {
        public virtual UccountEntity Account { get; set; }

        public virtual CategoryEntity Category { get; set; }
    }
}
