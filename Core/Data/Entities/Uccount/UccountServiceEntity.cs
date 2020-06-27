using System.Collections.Generic;

namespace Core.Data.Entities {
    public class UccountServiceEntity {
        public long AccountId { get; set; }

        public ICollection<UccountServiceSectionEntity> Sections { get; set; }
    }
}
