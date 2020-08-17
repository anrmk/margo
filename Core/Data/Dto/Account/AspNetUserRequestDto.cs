using System;

using AutoMapper.Configuration.Annotations;

namespace Core.Data.Dto {
    public class AspNetUserRequestDto {
        public Guid Id { get; set; }

        public Guid ModelId { get; set; }
        public string Model { get; set; }
        public Type ModelType { get; set; }

        public string Description { get; set; }

        public string ModelTypeName {
            get {
                if(ModelType == typeof(CompanyDto))
                    return "Company";
                if(ModelType == typeof(UccountDto))
                    return "Account";
                else
                    return "Undefined";
            }
        } 

        public DateTime CreatedDate { get; set; }
        public string CreatedBy { get; set; }
        public DateTime UpdatedDate { get; set; }
        public string UpdatedBy { get; set; }
    }
}
