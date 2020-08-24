using System;

namespace Core.Data.Dto {
    public class CompanyDataDto {
        public Guid Id { get; set; }
        public Guid CompanyId { get; set; }
        public Guid FieldId { get; set; }
        public Guid ServiceId { get; set; }

        public string ServiceName { get; set; }
        public string Name { get; set; }
        public string Value { get; set; }
    }
}
