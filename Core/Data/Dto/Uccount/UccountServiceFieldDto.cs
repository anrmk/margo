using Core.Data.Enums;

namespace Core.Data.Dto {
    public class UccountServiceFieldDto {
        public long Id { get; set; }
        public FieldEnum Type { get; set; }
        public string Name { get; set; }
        public string Value { get; set; }
        public bool IsRequired { get; set; }
    }
}
