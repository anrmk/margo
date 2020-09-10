
using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;

using AutoMapper;

using Core.Data.Dto;
using Core.Data.Enums;
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

            CreateMap<AspNetUserDenyAccessViewModel, AspNetUserDenyAccessCompanyDto>()
                .ReverseMap()
                .ForMember(d => d.EntityId, o => o.MapFrom(s => s.CompanyId))
                .ForMember(d => d.EntityName, o => o.MapFrom(s => s.Company.Name));
            CreateMap<AspNetUserDenyAccessViewModel, AspNetUserDenyAccessCompanyDto>()
                .ForMember(d => d.CompanyId, o => o.MapFrom(s => s.EntityId));

            CreateMap<AspNetUserDenyAccessViewModel, AspNetUserDenyAccessCategoryDto>()
                .ReverseMap()
                .ForMember(d => d.EntityId, o => o.MapFrom(s => s.CategoryId))
                .ForMember(d => d.EntityName, o => o.MapFrom(s => s.Category.Name));
            CreateMap<AspNetUserDenyAccessViewModel, AspNetUserDenyAccessCategoryDto>()
                .ForMember(d => d.CategoryId, o => o.MapFrom(s => s.EntityId));

            CreateMap<AspNetUserCompanyFavouriteViewModel, AspNetUserCompanyFavouriteDto>()
                .ReverseMap()
                .ForMember(d => d.CompanyId, o => o.MapFrom(s => s.Company == null ? (Guid?)null : s.Company.Id))
                .ForMember(d => d.CompanyName, o => o.MapFrom(s => s.Company == null ? null : s.Company.Name))
                .ForMember(d => d.CompanyDescription, o => o.MapFrom(s => s.Company == null ? null : s.Company.Description))
                .ForMember(d => d.CompanyFounded, o => o.MapFrom(s => s.Company == null ? null : s.Company.Founded))
                .ForMember(d => d.CompanyEIN, o => o.MapFrom(s => s.Company == null ? null : s.Company.EIN))
                .ForMember(d => d.CompanyDB, o => o.MapFrom(s => s.Company == null ? null : s.Company.DB));

            CreateMap<LogViewModel, LogDto>().ReverseMap();
            CreateMap<LogFilterViewModel, LogFilterDto>()
                .ForMember(d => d.StartDate, o => o.MapFrom(s => s.StartDate))
                .ForMember(d => d.EndDate, o => o.MapFrom(s => s.EndDate))
                .ForMember(d => d.Method, o => o.MapFrom(s => !s.Method.HasValue ? null : ((HttpMethodEnum)s.Method).ToString()))
                .ReverseMap();

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
            CreateMap<InvoiceFilterViewModel, InvoiceFilterDto>().ReverseMap();
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

            CreateMap<CategoryViewModel, CategoryDto>()
                .BeforeMap((s, _) => {
                    s.Fields = s.Fields?.Select((field, index) => {
                        field.Sort = index;
                        return field;
                    }).ToList();
                })
                .ReverseMap();
            CreateMap<CategoryListViewModel, CategoryDto>().ReverseMap();
            CreateMap<CategoryFieldViewModel, CategoryFieldDto>().ReverseMap();
            CreateMap<CategoryFilterViewModel, CategoryFilterDto>().ReverseMap();

            CreateMap<VendorViewModel, VendorDto>()
                .ForMember(d => d.Categories, o => o.MapFrom(s => s.Categories.Select(x => new VendorCategoryDto() { CategoryId = x })))
                .BeforeMap((s, _) => {
                    s.Fields = s.Fields?.Select((field, index) => {
                        field.Sort = index;
                        return field;
                    }).ToList();
                })
                .ReverseMap()
                .ForMember(d => d.Categories, o => o.MapFrom(s => s.Categories.Select(x => x.CategoryId)));
            CreateMap<VendorListViewModel, VendorDto>().ReverseMap();
            CreateMap<VendorFieldViewModel, VendorFieldDto>().ReverseMap();
            CreateMap<VendorCategoryViewModel, VendorCategoryDto>()
                .ReverseMap()
                .ForMember(d => d.Id, o => o.MapFrom(s => s.Id))
                .ForMember(d => d.CategoryId, o => o.MapFrom(s => s.CategoryId))
                .ForMember(d => d.Name, o => o.MapFrom(s => s.CategoryName));

            //#region SERVICES
            //CreateMap<ServiceViewModel, ServiceDto>().ReverseMap();
            //#endregion

            #region PERSONS
            CreateMap<PersonViewModel, PersonDto>().ReverseMap();
            CreateMap<PersonListViewModel, PersonDto>().ReverseMap();
            #endregion

            #region TODO
            CreateMap<TodoViewModel, TodoDto>().ReverseMap();
            CreateMap<TodoListViewModel, TodoDto>().ReverseMap();
            CreateMap<TodoFilterViewModel, TodoFilterDto>().ReverseMap();
            #endregion
        }
    }
}
