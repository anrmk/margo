using System;
using System.Collections.Generic;

namespace Core.Data.Dto {
    public class UccountServiceDto {
        public Guid Id { get; set; }
        public Guid UccountId { get; set; }
        public string Name { get; set; }
        public Guid? CategoryId { get; set; }

        public IEnumerable<UccountServiceFieldDto> Fields { get; set; }
    }
}
