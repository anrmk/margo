using Core.Data.Enums;

namespace Core.Data.Dto {
    public class CategoryFieldDto {
        public long Id { get; set; }
        public FieldEnum Type { get; set; }
        public string Name { get; set; }
        public bool IsRequired { get; set; }
        public long CategoryId { get; set; }
        public int Sort { get; set; }
        public bool IsHidden { get; set; }
    }
}
