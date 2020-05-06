
using AutoMapper;

using Core.Data.Dto;
using Core.Data.Dto.Nsi;
using Core.Extension;

using Microsoft.Extensions.DependencyInjection;

using Web.ViewModels;

namespace Web {
    public class MapperConfig: Profile {
        public static void Register(IServiceCollection services) {
            var mapperConfig = new MapperConfiguration(mc => {
                mc.AddProfile(new Core.MapperConfig());
                mc.AddProfile(new MapperConfig());
            });
            IMapper mapper = mapperConfig.CreateMapper();
            services.AddSingleton(mapper);
        }

        public MapperConfig() {
            CreateMap<PagerFilterViewModel, PagerFilter>();

            #region COMPANY
            CreateMap<CompanyViewModel, CompanyDto>().ReverseMap();
            CreateMap<CompanyViewModel, CompanyGeneralViewModel>()
                .ReverseMap()
                .ForMember(d => d.General, o => o.MapFrom(s => new CompanyGeneralViewModel() {
                    Id = s.Id,
                    Name = s.Name,
                    No = s.No,
                    PhoneNumber = s.PhoneNumber,
                    Website = s.Website,
                    Email = s.Email,
                    CEO = s.CEO,
                    DB = s.DB,
                    EIN = s.EIN,
                    Founded = s.Founded
                }));

            CreateMap<CompanyGeneralViewModel, CompanyGeneralDto>().ReverseMap();
            CreateMap<CompanyAddressViewModel, CompanyAddressDto>().ReverseMap();

            CreateMap<CompanyListViewModel, CompanyDto>()
                .ReverseMap()
                .ForMember(d => d.No, o => o.MapFrom(s => s.General.No))
                .ForMember(d => d.Name, o => o.MapFrom(s => s.General.Name))
                .ForMember(d => d.PhoneNumber, o => o.MapFrom(s => s.General.PhoneNumber))
                .ForMember(d => d.Address, o => o.MapFrom(s => (s.Address != null) ? s.Address.ToString() : ""));

            #endregion

            #region VENDOR
            CreateMap<VendorViewModel, VendorDto>().ReverseMap();
            CreateMap<VendorViewModel, VendorGeneralDto>().ReverseMap()
                .ForMember(d => d.General, o => o.MapFrom(s => new VendorGeneralViewModel() {
                    Id = s.Id,
                    Name = s.Name,
                    No = s.No,
                    PhoneNumber = s.PhoneNumber,
                    Website = s.Website,
                    Email = s.Email,
                    Description = s.Description
                }));

            CreateMap<VendorGeneralViewModel, VendorGeneralDto>().ReverseMap();
            CreateMap<VendorAddressViewModel, VendorAddressDto>().ReverseMap();

            CreateMap<VendorListViewModel, VendorDto>()
                .ReverseMap()
                .ForMember(d => d.No, o => o.MapFrom(s => s.General.No))
                .ForMember(d => d.Name, o => o.MapFrom(s => s.General.Name))
                .ForMember(d => d.Description, o => o.MapFrom(s => s.General.Description))
                .ForMember(d => d.PhoneNumber, o => o.MapFrom(s => s.General.PhoneNumber))
                .ForMember(d => d.Email, o => o.MapFrom(s => s.General.Email))
                .ForMember(d => d.Website, o => o.MapFrom(s => s.General.Website))
                .ForMember(d => d.Address, o => o.MapFrom(s => (s.Address != null) ? s.Address.ToString() : ""));
            #endregion

            #region VACCOUNT
            CreateMap<VaccountViewModel, VaccountDto>().ReverseMap();
            CreateMap<VaccountListViewModel, VaccountDto>().ReverseMap()
                .ForMember(d => d.VendorName, o => o.MapFrom(s => s.Vendor.General.Name))
                .ForMember(d => d.CompanyName, o => o.MapFrom(s => s.Company.General.Name));
            CreateMap<VaccountFilterViewModel, VaccountFilterDto>();

            #endregion

            #region INVOICE
            CreateMap<InvoiceListViewModel, InvoiceDto>()
               .ReverseMap()
               .ForMember(d => d.CompanyName, o => o.MapFrom(s => s.Company.General.Name))
               .ForMember(d => d.Amount, o => o.MapFrom(s => (s.Subtotal * (1 + s.TaxRate / 100)).ToString("0.##")));

            CreateMap<InvoiceViewModel, InvoiceDto>()
                .ForMember(d => d.Supplier, o => o.Ignore())
                .ForMember(d => d.Company, o => o.Ignore())
                .ReverseMap();

            CreateMap<InvoiceFilterViewModel, InvoiceFilterDto>().ReverseMap();

            #endregion

            CreateMap<NsiViewModel, NsiDto>().ReverseMap();
        }
    }
}
