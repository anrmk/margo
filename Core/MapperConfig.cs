
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
                .ForMember(d => d.General, o => o.MapFrom(s => new CompanyGeneralDto() {
                    Id = s.Id,
                    Name = s.Name,
                    No = s.No,
                    PhoneNumber = s.PhoneNumber,
                    Website = s.Website,
                    Email = s.Email,
                    Founded = s.Founded,
                    EIN = s.EIN,
                    DB = s.DB,
                    CEO = s.CEO
                }));
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
            CreateMap<VendorDto, VendorEntity>()
                .ReverseMap()
                .ForMember(d => d.General, o => o.MapFrom(s => new VendorGeneralDto() {
                    Id = s.Id,
                    No = s.No,
                    Name = s.Name,
                    Description = s.Description,
                    Email = s.Email,
                    PhoneNumber = s.PhoneNumber,
                    Website = s.Website
                }));
            ;
            CreateMap<VendorGeneralDto, VendorEntity>().ReverseMap();
            CreateMap<VendorAddressDto, VendorAddressEntity>().ReverseMap();
            CreateMap<VendorSectionDto, VendorSectionEntity>()
                .ForMember(d => d.Vendor, o => o.Ignore())
                .ForMember(d => d.Section, o => o.Ignore())
                .ReverseMap()
                .ForMember(d => d.VendorName, o => o.MapFrom(s => s.Vendor.Name))
                .ForMember(d => d.SectionName, o => o.MapFrom(s => s.Section.Name))
                .ForMember(d => d.SectionCode, o => o.MapFrom(s => s.Section.Code))
                ;
            CreateMap<VendorSectionFieldDto, VendorSectionFieldEntity>().ReverseMap();
            #endregion

            #region SECTIONS
            CreateMap<SectionDto, SectionEntity>().ReverseMap();
            CreateMap<SectionFieldDto, SectionFieldEntity>().ReverseMap();
            #endregion

        }
    }
}
