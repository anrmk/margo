
using System.ComponentModel.DataAnnotations;
using System.Linq;

using AutoMapper;

using Core.Data.Dto;

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
            CreateMap<PagerFilterViewModel, PagerFilterDto>();

            CreateMap<AspNetUserViewModel, AspNetUserDto>()
                .ForMember(d => d.Roles, o => o.MapFrom(s => s.Roles.Select(x => new AspNetRoleDto() { Id = x })))
                .ReverseMap()
                .ForMember(d => d.Roles, o => o.MapFrom(s => s.Roles.Select(x => x.Id)));
            CreateMap<AspNetUserListViewModel, AspNetUserDto>().ReverseMap()
                .ForMember(d => d.Roles, o => o.MapFrom(s => s.Roles.Select(x => x.Name)));
            CreateMap<AspNetRoleViewModel, AspNetRoleDto>().ReverseMap();
            CreateMap<AspNetUserProfileViewModel, AspNetUserProfileDto>().ReverseMap();
            CreateMap<AspNetUserRequestViewModel, AspNetUserRequestDto>().ReverseMap();
            CreateMap<AspNetUserRequestListViewModel, AspNetUserRequestDto>().ReverseMap();

            CreateMap<AspNetUserGrantsListViewModel, AspNetUserCompanyGrantsListDto>().ReverseMap();
            CreateMap<AspNetUserGrantsViewModel, AspNetUserCompanyGrantsDto>()
                .ReverseMap()
                .ForMember(d => d.EntityId, o => o.MapFrom(s => s.CompanyId))
                .ForMember(d => d.EntityName, o => o.MapFrom(s => s.Company.Name))
                .ForMember(d => d.IsGranted, o => o.MapFrom(s => !s.IsGranted ? null : "granted"));
            CreateMap<AspNetUserGrantsViewModel, AspNetUserCompanyGrantsDto>()
                .ForMember(d => d.CompanyId, o => o.MapFrom(s => s.EntityId))
                .ForMember(d => d.IsGranted, o => o.MapFrom(s => !string.IsNullOrEmpty(s.IsGranted)));

            CreateMap<AspNetUserGrantsListViewModel, AspNetUserCategoryGrantsListDto>().ReverseMap();
            CreateMap<AspNetUserGrantsViewModel, AspNetUserCategoryGrantsDto>()
                .ReverseMap()
                .ForMember(d => d.EntityId, o => o.MapFrom(s => s.CategoryId))
                .ForMember(d => d.EntityName, o => o.MapFrom(s => s.Category.Name))
                .ForMember(d => d.IsGranted, o => o.MapFrom(s => !s.IsGranted ? null : "granted"));
            CreateMap<AspNetUserGrantsViewModel, AspNetUserCategoryGrantsDto>()
                .ForMember(d => d.CategoryId, o => o.MapFrom(s => s.EntityId))
                .ForMember(d => d.IsGranted, o => o.MapFrom(s => !string.IsNullOrEmpty(s.IsGranted)));

            CreateMap<LogViewModel, LogDto>().ReverseMap();
            CreateMap<LogFilterViewModel, LogFilterDto>().ReverseMap();

            #region COMPANY
            CreateMap<CompanyViewModel, CompanyDto>()
                .ReverseMap()
                .ForMember(d => d.Data, o => o.Ignore());
            CreateMap<CompanyListViewModel, CompanyDto>().ReverseMap();

            CreateMap<CompanyDataViewModel, CompanyDataDto>().ReverseMap();
            CreateMap<CompanyDataListViewModel, CompanyDto>()
                .ReverseMap()
                .ForMember(d => d.CompanyId, o => o.MapFrom(s => s.Id));
            
            CreateMap<CompanyDataListViewModel, CompanyDataListDto>().ReverseMap();
            CreateMap<CompanyDataListViewModel, CompanyDataDto>().ReverseMap();

            CreateMap<CompanySectionFieldViewModel, CompanySectionFieldDto>().ReverseMap();
            CreateMap<CompanySectionViewModel, CompanySectionDto>().ReverseMap();
            CreateMap<CompanyFilterViewModel, CompanyFilterDto>().ReverseMap();

            // CreateMap<CompanySectionViewModel, CompanySectionDto>().ReverseMap();
            // CreateMap<CompanySectionFieldViewModel, CompanySectionFieldDto>().ReverseMap();
            #endregion

            #region UCCOUNTS
            CreateMap<UccountViewModel, UccountDto>().ReverseMap();
            CreateMap<UccountListViewModel, UccountDto>()
                .ReverseMap()
                .ForMember(d => d.ServiceCount, o => o.MapFrom(s => s.Services.Count()))
                .ForMember(d => d.Kind, o => o.MapFrom(s => s.Kind.GetAttribute<DisplayAttribute>().Name));
            //CreateMap<UccountSectionViewModel, UccountSectionDto>().ReverseMap();
            //CreateMap<UccountSectionFieldViewModel, UccountSectionFieldDto>().ReverseMap();
            CreateMap<UccountServiceFieldViewModel, UccountServiceFieldDto>().ReverseMap();
            CreateMap<UccountVendorFieldViewModel, UccountVendorFieldDto>().ReverseMap();
            CreateMap<UccountServiceViewModel, UccountServiceDto>().ReverseMap();
            CreateMap<UccountFilterViewModel, UccountFilterDto>().ReverseMap();
            #endregion

            #region INVOICE
            CreateMap<InvoiceListViewModel, InvoiceDto>()
               .ReverseMap()
               .ForMember(d => d.AmountDecimal, o => o.MapFrom(s => s.Amount))
               .ForMember(d => d.Amount, o => o.MapFrom(s => s.Amount.ToCurrency()))
               .ForMember(d => d.BalanceAmountDecimal, o => o.MapFrom(s => s.Amount - (s.PaymentAmount ?? default)))
               .ForMember(d => d.BalanceAmount, o => o.MapFrom(s => (s.Amount - (s.PaymentAmount ?? default)).ToCurrency()));
            CreateMap<InvoiceViewModel, InvoiceDto>().ReverseMap();
            CreateMap<InvoiceFilterViewModel, InvoiceFilterDto>()
                .ForMember(d => d.CompanyId, o => o.MapFrom(s => s.IsPerson ? null : s.CustomerId))
                .ForMember(d => d.PersonId, o => o.MapFrom(s => s.IsPerson ? s.CustomerId : null));
            CreateMap<InvoiceServiceViewModel, InvoiceServiceDto>().ReverseMap();
            #endregion

            CreateMap<PaymentViewModel, PaymentDto>().ReverseMap();
            CreateMap<PaymentListViewModel, PaymentDto>().ReverseMap();
            CreateMap<PaymentFilterViewModel, PaymentFilterDto>().ReverseMap();

            //#region SECTIONS
            //CreateMap<SectionViewModel, SectionDto>().ReverseMap();
            //CreateMap<SectionListViewModel, SectionDto>().ReverseMap();
            //CreateMap<SectionFieldViewModel, SectionFieldDto>().ReverseMap();
            //CreateMap<SectionFieldsFilterViewModel, SectionFieldsFilterDto>().ReverseMap();
            //#endregion

            CreateMap<CategoryViewModel, CategoryDto>().ReverseMap();
            CreateMap<CategoryListViewModel, CategoryDto>().ReverseMap();
            CreateMap<CategoryFieldViewModel, CategoryFieldDto>().ReverseMap();
            CreateMap<CategoryFilterViewModel, CategoryFilterDto>().ReverseMap();

            CreateMap<VendorViewModel, VendorDto>().ReverseMap();
            CreateMap<VendorListViewModel, VendorDto>().ReverseMap();
            CreateMap<VendorFieldViewModel, VendorFieldDto>().ReverseMap();


            //#region SERVICES
            //CreateMap<ServiceViewModel, ServiceDto>().ReverseMap();
            //#endregion

            #region PERSONS
            CreateMap<PersonViewModel, PersonDto>().ReverseMap();
            CreateMap<PersonListViewModel, PersonDto>().ReverseMap();
            #endregion
        }
    }
}
