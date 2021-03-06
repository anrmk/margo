﻿
using AutoMapper;

using Core.Data.Dto;
using Core.Data.Dto.Nsi;
using Core.Extension;

using Microsoft.Extensions.DependencyInjection;
using System.Linq;
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

            CreateMap<CompanyListViewModel, CompanyDto>().ReverseMap();

            CreateMap<CompanySectionViewModel, CompanySectionDto>().ReverseMap();
            CreateMap<CompanySectionFieldViewModel, CompanySectionFieldDto>().ReverseMap();
            #endregion

            #region UCCOUNTS
            CreateMap<UccountViewModel, UccountDto>().ReverseMap();
            CreateMap<UccountListViewModel, UccountDto>()
                .ReverseMap()
                .ForMember(d => d.ModifiedDate, o => o.MapFrom(s => s.Updated))
                .ForMember(
                    d => d.Name,
                    o => o.MapFrom(s => s.VendorId != 0
                        ? s.VendorName
                        : s.CompanyName))
                .ForMember(d => d.ServiceCount, o => o.MapFrom(s => s.Services.Count()))
                .ForMember(
                    d => d.Kind,
                    o => o.MapFrom(s => s.Kind == Core.Data.Enums.UccountTypes.BUSINESS
                        ? "Business"
                        : "Personal"));
            CreateMap<UccountSectionViewModel, UccountSectionDto>().ReverseMap();
            CreateMap<UccountSectionFieldViewModel, UccountSectionFieldDto>().ReverseMap();
            CreateMap<UccountServiceViewModel, UccountServiceDto>().ReverseMap();
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
            CreateMap<SectionFieldsFilterViewModel, SectionFieldsFilterDto>().ReverseMap();


            #endregion

            CreateMap<CategoryViewModel, CategoryDto>().ReverseMap();
            CreateMap<CategoryListViewModel, CategoryDto>().ReverseMap();
            CreateMap<CategoryFieldViewModel, CategoryFieldDto>().ReverseMap();

            CreateMap<VendorViewModel, VendorDto>().ReverseMap();
            CreateMap<VendorListViewModel, VendorDto>().ReverseMap();
            CreateMap<VendorFieldViewModel, VendorFieldDto>().ReverseMap();


            CreateMap<NsiViewModel, NsiDto>().ReverseMap();

            #region SERVICES
            CreateMap<ServiceViewModel, ServiceDto>().ReverseMap();
            #endregion

            #region PERSONS
            CreateMap<PersonViewModel, PersonDto>().ReverseMap();
            CreateMap<PersonListViewModel, PersonDto>().ReverseMap();
            #endregion
        }
    }
}
