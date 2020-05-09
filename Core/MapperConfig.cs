using AutoMapper;

using Core.Data.Dto;
using Core.Data.Entities;
using Core.Data.Entities.Vaccount;

namespace Core {
    public class MapperConfig: Profile {
        public MapperConfig() {
            CreateMap<ApplicationUserEntity, ApplicationUserDto>().ReverseMap();
            CreateMap<ApplicationUserProfileEntity, UserProfileDto>().ReverseMap();

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
                .ForMember(d => d.SectionName, o => o.MapFrom(s => s.Section.Name));
            #endregion

            #region INVOICE
            CreateMap<InvoiceDto, InvoiceEntity>()
                .ForMember(d => d.Account, o => o.Ignore())
                .ReverseMap();

            #endregion

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
            #endregion

            #region VACCOUNT
            CreateMap<VaccountDto, VaccountEntity>().ReverseMap();
            CreateMap<VaccountSecurityDto, VaccountSecurityEntity>().ReverseMap();
            CreateMap<VaccountSecurityQuestionDto, VaccountSecurityQuestionEntity>().ReverseMap();
            #endregion

            #region SECTIONS
            CreateMap<SectionDto, SectionEntity>().ReverseMap();
            #endregion
        }
    }
}
