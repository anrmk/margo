using System;

namespace Core.Data.Dto {
    public class PersonDto {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string SurName { get; set; }
        public string MiddleName { get; set; }
        public string Description { get; set; }


        public DateTime CreatedDate { get; set; }
        public string CreatedBy { get; set; }
        public DateTime UpdatedDate { get; set; }
        public string UpdatedBy { get; set; }

        public string FullName =>
            $"{Name} {MiddleName} {SurName}";
    }
}
