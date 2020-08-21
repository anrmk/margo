using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Core.Data.Entities {
    [Table(name: "AspNetUserRequests")]
    public class AspNetUserRequestEntity: AuditableEntity<Guid> {
        public Guid? ModelId { get; set; }

        public string Model { get; set; }

        public string ModelType { get; set; }
    }
}
