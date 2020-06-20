namespace Core.Data.Dto {
    public class CompanySectionFieldDto {
        public long Id { get; set; }

        public long SectionId { get; set; }

        public int Type { get; set; }

        public string Name { get; set; }

        public string Value { get; set; }

        public string Secret { get; set; }

        public string Note { get; set; }

        public string Link { get; set; }
    }
}
