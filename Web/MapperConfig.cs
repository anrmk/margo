
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

            CreateMap<LogViewModel, LogDto>().ReverseMap();
            CreateMap<LogFilterViewModel, LogFilterDto>().ReverseMap();

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

            CreateMap<CompanySectionViewModel, CompanySectionDto>().ReverseMap();
            CreateMap<CompanySectionFieldViewModel, CompanySectionFieldDto>().ReverseMap();
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
            CreateMap<VendorSectionViewModel, VendorSectionDto>()
                .ForMember(d => d.Fields, o => o.Ignore())
                .ReverseMap();
            CreateMap<VendorSectionFieldViewModel, VendorSectionFieldDto>().ReverseMap();

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

            #region INVOICE
            CreateMap<InvoiceListViewModel, InvoiceDto>()
               .ReverseMap()
               .ForMember(d => d.Amount, o => o.MapFrom(s => s.Amount.ToCurrency()))
               .ForMember(d => d.BalanceAmount, o => o.MapFrom(s => (s.Amount - (s.PaymentAmount ?? 0)).ToCurrency()));
            CreateMap<InvoiceViewModel, InvoiceDto>().ReverseMap();
            CreateMap<InvoiceFilterViewModel, InvoiceFilterDto>().ReverseMap();
            #endregion

            CreateMap<PaymentViewModel, PaymentDto>().ReverseMap();
            CreateMap<PaymentFilterViewModel, PaymentFilterDto>().ReverseMap();

            #region SECTIONS
            CreateMap<SectionViewModel, SectionDto>().ReverseMap();
            CreateMap<SectionListViewModel, SectionDto>().ReverseMap();
            CreateMap<SectionFieldViewModel, SectionFieldDto>().ReverseMap();

            #endregion

            CreateMap<NsiViewModel, NsiDto>().ReverseMap();
        }
    }
}
