using System.Linq;

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
            #region COMPANY
            CreateMap<CompanyViewModel, CompanyDto>()
                .ForMember(d => d.Address, o => o.MapFrom(s => new CompanyAddressDto() { Id = s.AddressId, Address = s.Address, Address2 = s.Address2, City = s.City, State = s.State, ZipCode = s.ZipCode, Country = s.Country }))
                .ReverseMap()
                .ForMember(d => d.AddressId, o => o.MapFrom(s => (s.Address != null) ? s.Address.Id : 0))
                .ForMember(d => d.Address, o => o.MapFrom(s => (s.Address != null) ? s.Address.Address : ""))
                .ForMember(d => d.Address2, o => o.MapFrom(s => (s.Address != null) ? s.Address.Address2 : ""))
                .ForMember(d => d.City, o => o.MapFrom(s => (s.Address != null) ? s.Address.City : ""))
                .ForMember(d => d.State, o => o.MapFrom(s => (s.Address != null) ? s.Address.State : ""))
                .ForMember(d => d.Country, o => o.MapFrom(s => (s.Address != null) ? s.Address.Country : ""))
                .ForMember(d => d.ZipCode, o => o.MapFrom(s => (s.Address != null) ? s.Address.ZipCode : ""));

            CreateMap<CompanyListViewModel, CompanyDto>()
                .ReverseMap()
                .ForMember(d => d.Address, o => o.MapFrom(s => (s.Address != null) ? s.Address.ToString() : ""));
            #endregion

            #region SUPPLIER
            CreateMap<SupplierListViewModel, SupplierDto>()
                .ForMember(d => d.Invoices, o => o.Ignore())
                .ReverseMap()
                .ForMember(d => d.Company, o => o.MapFrom(s => (s.Company != null) ? s.Company.Name : ""))
                .ForMember(d => d.Address, o => o.MapFrom(s => (s.Address != null) ? s.Address.ToString() : ""));

            CreateMap<SupplierViewModel, SupplierDto>()
               .ForMember(d => d.Address, o => o.MapFrom(s => new SupplierAddressDto() { Id = s.AddressId, Address = s.Address, Address2 = s.Address2, City = s.City, State = s.State, ZipCode = s.ZipCode, Country = s.Country }))
               .ReverseMap()
               .ForMember(d => d.AddressId, o => o.MapFrom(s => (s.Address != null) ? s.Address.Id : (long?)null))
               .ForMember(d => d.Address, o => o.MapFrom(s => (s.Address != null) ? s.Address.Address : ""))
               .ForMember(d => d.Address2, o => o.MapFrom(s => (s.Address != null) ? s.Address.Address2 : ""))
               .ForMember(d => d.City, o => o.MapFrom(s => (s.Address != null) ? s.Address.City : ""))
               .ForMember(d => d.State, o => o.MapFrom(s => (s.Address != null) ? s.Address.State : ""))
               .ForMember(d => d.ZipCode, o => o.MapFrom(s => (s.Address != null) ? s.Address.ZipCode : ""))
               .ForMember(d => d.Country, o => o.MapFrom(s => (s.Address != null) ? s.Address.Country : ""));

            #endregion

            #region INVOICE
            CreateMap<InvoiceListViewModel, InvoiceDto>()
               .ReverseMap()
               .ForMember(d => d.CompanyName, o => o.MapFrom(s => s.Company.Name))
               .ForMember(d => d.CustomerName, o => o.MapFrom(s => s.Supplier.Name))
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
