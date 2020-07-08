using Core.Data.Enums;

namespace Core.Data.Dto {
    public class SectionFieldDto {
        public long Id { get; set; }
        public FieldEnum Type { get; set; }
        public string Name { get; set; }
        public bool IsRequired { get; set; }
        public long SectionId { get; set; }
    }
}
