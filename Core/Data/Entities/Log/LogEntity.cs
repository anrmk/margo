using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

using NLog;

namespace Core.Data.Entities {
    [Table(name: "Logs")]
    public class LogEntity: EntityBase<long> {
        [Required]
        [MaxLength(50)]
        public string Application { get; set; }

        [Required]
        [ScaffoldColumn(false)]
        [DataType(DataType.DateTime)]
        public DateTime Logged { get; set; }

        [Required]
        [MaxLength(50)]
        public string Level { get; set; }

        [Required]
        public string Message { get; set; }

        public string Logger { get; set; }

        public string Callsite { get; set; }

        public string Exception { get; set; }

        [MaxLength(256)]
        public string UserName { get; set; }

        public string Action { get; set; }

        public string Url { get; set; }

        [MaxLength(12)]
        public string Method { get; set; }
    }
}
