
using AutoMapper;

using Core.Data.Dto;
using Core.Data.Entities;

namespace Core {
    public class MapperConfig: Profile {
        public MapperConfig() {
            CreateMap<AppNetUserEntity, ApplicationUserDto>().ReverseMap();
            CreateMap<AppNetUserProfileEntity, UserProfileDto>().ReverseMap();

            CreateMap<LogEntity, LogDto>().ReverseMap();

            #region COMPANY
            CreateMap<CompanyDto, CompanyEntity>()
                .ReverseMap()
              ;
            CreateMap<CompanyGeneralDto, CompanyEntity>().ReverseMap();
            CreateMap<CompanyAddressDto, CompanyAddressEntity>().ReverseMap();
            CreateMap<CompanySectionDto, CompanySectionEntity>()
                .ForMember(d => d.Company, o => o.Ignore())
                .ForMember(d => d.Section, o => o.Ignore())
                .ReverseMap()
                .ForMember(d => d.CompanyName, o => o.MapFrom(s => s.Company.Name))
                .ForMember(d => d.SectionName, o => o.MapFrom(s => s.Section.Name))
                .ForMember(d => d.SectionCode, o => o.MapFrom(s => s.Section.Code))
                .ForMember(d => d.SectionDescription, o => o.MapFrom(s => s.Section.Description));

            CreateMap<CompanySectionFieldDto, CompanySectionFieldEntity>().ReverseMap();

            #endregion

            #region INVOICE
            CreateMap<InvoiceDto, InvoiceEntity>()
                .ForMember(d => d.Company, o => o.Ignore())
                .ForMember(d => d.Vendor, o => o.Ignore())
                .ReverseMap()
                .ForMember(d => d.CompanyName, o => o.MapFrom(s => s.Company.Name))
                .ForMember(d => d.VendorName, o => o.MapFrom(s => s.Vendor.Name))
                ;

            #endregion

            CreateMap<PaymentDto, PaymentEntity>()
                .ForMember(d => d.Invoice, o => o.Ignore())
                .ReverseMap()
                .ForMember(d => d.InvoiceNo, o => o.MapFrom(s => s.Invoice != null ? s.Invoice.No : ""));

            #region VENDOR
            CreateMap<VendorDto, VendorEntity>().ReverseMap();
            CreateMap<VendorFieldDto, VendorFieldEntity>().ReverseMap();
            #endregion

            #region SECTIONS
            CreateMap<SectionDto, SectionEntity>().ReverseMap();
            CreateMap<SectionFieldDto, SectionFieldEntity>().ReverseMap();
            #endregion

            #region CATEGORY
            CreateMap<CategoryDto, CategoryEntity>()
                .ForMember(d => d.Parent, o => o.Ignore())
                .ReverseMap()
                .ForMember(d => d.ParentName, o => o.MapFrom(s => s.Parent != null ? s.Parent.Name : ""))
                ;
            CreateMap<CategoryFieldDto, CategoryFieldEntity>().ReverseMap();
            #endregion

            #region UCCOUNTS
            CreateMap<UccountDto, UccountEntity>()
                .ReverseMap();
            //.ForMember(d => d.SectionName, o => o.MapFrom(s => s.Section.Name ?? ""));
            CreateMap<UccountSectionDto, UccountSectionEntity>().ReverseMap();
            CreateMap<UccountSectionFieldDto, UccountSectionFieldEntity>().ReverseMap();
            CreateMap<UccountServiceDto, UccountServiceEntity>().ReverseMap();
            #endregion

            #region SERVICES
            CreateMap<ServiceDto, UccountServiceEntity>().ReverseMap();
            #endregion
        }
    }
}
