﻿
using AutoMapper;

using Core.Data.Dto;
using Core.Data.Dto.Nsi;
using Core.Extension;

using Microsoft.Extensions.DependencyInjection;
using System.ComponentModel.DataAnnotations;
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

            CreateMap<AspNetUserViewModel, AspNetUserDto>()
                .ForMember(d => d.Roles, o => o.MapFrom(s => s.Roles.Select(x => new AspNetRoleDto() { Id = x})))
                .ReverseMap()
                .ForMember(d => d.Roles, o => o.MapFrom(s => s.Roles.Select(x => x.Id)));
            CreateMap<AppNetUserListViewModel, AspNetUserDto>().ReverseMap()
                .ForMember(d => d.Roles, o => o.MapFrom(s => s.Roles.Select(x => x.Name)));
            CreateMap<AspNetRoleViewModel, AspNetRoleDto>().ReverseMap();
            CreateMap<AspNetUserProfileViewModel, AspNetUserProfileDto>().ReverseMap();

            CreateMap<LogViewModel, LogDto>().ReverseMap();
            CreateMap<LogFilterViewModel, LogFilterDto>().ReverseMap();

            #region COMPANY
            CreateMap<CompanyViewModel, CompanyDto>().ReverseMap();
            CreateMap<CompanyViewModel, CompanyGeneralViewModel>().ReverseMap();

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
                .ForMember(
                    d => d.ServiceCount,
                    o => o.MapFrom(s => s.Services.Count()))
                .ForMember(
                    d => d.Kind,
                    o => o.MapFrom(s =>
                        s.Kind.GetAttribute<DisplayAttribute>().Name));
            CreateMap<UccountSectionViewModel, UccountSectionDto>().ReverseMap();
            CreateMap<UccountSectionFieldViewModel, UccountSectionFieldDto>().ReverseMap();
            CreateMap<UccountServiceFieldViewModel, UccountServiceFieldDto>().ReverseMap();
            CreateMap<UccountVendorFieldViewModel, UccountVendorFieldDto>().ReverseMap();
            CreateMap<UccountServiceViewModel, UccountServiceDto>().ReverseMap();
            #endregion

            #region INVOICE
            CreateMap<InvoiceListViewModel, InvoiceDto>()
               .ReverseMap()
               .ForMember(d => d.AmountDecimal, o => o.MapFrom(s => s.Amount))
               .ForMember(d => d.Amount, o => o.MapFrom(s => s.Amount.ToCurrency()))
               .ForMember(d => d.BalanceAmountDecimal, o => o.MapFrom(s => s.Amount - (s.PaymentAmount ?? default)))
               .ForMember(d => d.BalanceAmount, o => o.MapFrom(s => (s.Amount - (s.PaymentAmount ?? default)).ToCurrency()));
            CreateMap<InvoiceViewModel, InvoiceDto>().ReverseMap();
            CreateMap<InvoiceFilterViewModel, InvoiceFilterDto>().ReverseMap();
            CreateMap<InvoiceServiceViewModel, InvoiceServiceDto>().ReverseMap();
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
