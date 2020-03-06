using AutoMapper;

using Core.Data.Dto;
using Core.Data.Dto.Nsi;
using Core.Data.Entities;
using Core.Extension;

namespace Core {
    public class MapperConfig: Profile {
        public MapperConfig() {
            CreateMap<ApplicationUserEntity, ApplicationUserDto>().ReverseMap();
            CreateMap<ApplicationUserProfileEntity, UserProfileDto>().ReverseMap();

            #region COMPANY
            CreateMap<CompanyDto, CompanyEntity>().ReverseMap();
            CreateMap<CompanyAddressDto, CompanyAddressEntity>().ReverseMap();
            #endregion

            #region INVOICE
            CreateMap<InvoiceDto, InvoiceEntity>()
                .ForMember(d => d.Company, o => o.Ignore())
                .ForMember(d => d.Supplier, o => o.Ignore())
                .ReverseMap();

            #endregion

            #region SUPPLIER
            CreateMap<SupplierDto, SupplierEntity>()
                .ForMember(d => d.Company, o => o.Ignore())
                .ForMember(d => d.Invoices, o => o.Ignore())
                .ReverseMap();
            ;
            CreateMap<SupplierAddressDto, SupplierAddressEntity>().ReverseMap();
            #endregion

            #region NSI
            #endregion
        }
    }
}
