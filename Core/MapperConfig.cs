using System;
using System.Linq;

using AutoMapper;

using Core.Data.Dto;
using Core.Data.Entities;

using Microsoft.AspNetCore.Identity;

namespace Core {
    public class MapperConfig: Profile {
        public MapperConfig() {
            CreateMap<AspNetUserEntity, AspNetUserDto>().ReverseMap()
                .ForMember(d => d.Id, o => o.MapFrom(s => string.IsNullOrEmpty(s.Id) ? Guid.NewGuid().ToString() : s.Id));
            CreateMap<AspNetUserProfileEntity, AspNetUserProfileDto>().ReverseMap();
            CreateMap<AspNetRoleDto, IdentityRole>().ReverseMap();
            CreateMap<AspNetUserProfileDto, AspNetUserProfileEntity>().ReverseMap();

            CreateMap<LogEntity, LogDto>().ReverseMap();

            #region COMPANY
            CreateMap<CompanyDto, CompanyEntity>()
                .ReverseMap()
                .ForMember(d => d.CEOName, o => o.MapFrom(s => s.CEO.Name));

            //CreateMap<CompanySectionDto, CompanySectionEntity>()
            //    .ForMember(d => d.Company, o => o.Ignore())
            //    .ForMember(d => d.Section, o => o.Ignore())
            //    .ReverseMap()
            //    .ForMember(d => d.CompanyName, o => o.MapFrom(s => s.Company.Name))
            //    .ForMember(d => d.SectionName, o => o.MapFrom(s => s.Section.Name))
            //    .ForMember(d => d.SectionCode, o => o.MapFrom(s => s.Section.Code))
            //    .ForMember(d => d.SectionDescription, o => o.MapFrom(s => s.Section.Description));

            // CreateMap<CompanySectionFieldDto, CompanySectionFieldEntity>().ReverseMap();
            CreateMap<CompanyDataDto, CompanyDataEntity>()
                .ReverseMap()
                .ForMember(d => d.Name, o => o.MapFrom(s => s.Field.Name))
                .ForMember(d => d.Value, o => o.MapFrom(s => s.Field.Value));
                
            CreateMap<CompanySectionDto, CompanySectionEntity>().ReverseMap();
            CreateMap<CompanySectionFieldDto, CompanySectionFieldEntity>().ReverseMap();

            #endregion

            #region INVOICE
            CreateMap<InvoiceDto, InvoiceEntity>()
                .ReverseMap()
                .ForMember(d => d.CustomerName, o => o.MapFrom(s => s.Account.Company != null
                    ? s.Account.Company.Name
                    : $"{s.Account.Person.SurName} {s.Account.Person.MiddleName} {s.Account.Person.Name}"))
                .ForMember(d => d.PaymentAmount, o => o.MapFrom(s => s.Payments.Sum(p => p.Amount)))
                .ForMember(d => d.PaymentDate, o => o.MapFrom(s => s.Payments.Any() ? s.Payments.Max(p => p.Date) : (DateTime?)null));
            CreateMap<InvoiceServiceDto, InvoiceServiceEntity>().ReverseMap();
            #endregion

            CreateMap<PaymentDto, PaymentEntity>()
                .ForMember(d => d.Invoice, o => o.Ignore())
                .ReverseMap()
                .ForMember(d => d.InvoiceNo, o => o.MapFrom(s => s.Invoice != null ? s.Invoice.No : string.Empty));

            #region VENDOR
            CreateMap<VendorDto, VendorEntity>().ReverseMap();
            CreateMap<VendorFieldDto, VendorFieldEntity>().ReverseMap();
            #endregion

            //#region SECTIONS
            //CreateMap<SectionDto, SectionEntity>().ReverseMap();
            //CreateMap<SectionFieldDto, SectionFieldEntity>().ReverseMap();
            //#endregion

            #region CATEGORY
            CreateMap<CategoryDto, CategoryEntity>()
                .ForMember(d => d.Parent, o => o.Ignore())
                .ReverseMap()
                .ForMember(d => d.ParentName, o => o.MapFrom(s => s.Parent != null ? s.Parent.Name : ""))
                ;
            CreateMap<CategoryFieldDto, CategoryFieldEntity>().ReverseMap();
            #endregion

            #region UCCOUNTS
            CreateMap<UccountDto, UccountEntity>()
                .ReverseMap()
                .ForMember(d => d.Updated, o => o.MapFrom(s => s.UpdatedDate))
                .ForMember(d => d.Name, o => o.MapFrom(s => s.Person != null ? s.Person.Name : s.Company.Name))
                .ForMember(d => d.VendorName, o => o.MapFrom(x => x.Vendor.Name));
            //.ForMember(d => d.SectionName, o => o.MapFrom(s => s.Section.Name ?? ""));
            //CreateMap<UccountSectionDto, UccountSectionEntity>().ReverseMap();
            //CreateMap<UccountSectionFieldDto, UccountSectionFieldEntity>().ReverseMap();
            CreateMap<UccountServiceDto, UccountServiceEntity>().ReverseMap();
            CreateMap<UccountServiceFieldDto, UccountServiceFieldEntity>().ReverseMap();
            CreateMap<UccountVendorFieldDto, UccountVendorFieldEntity>().ReverseMap();
            #endregion

            //#region SERVICES
            //CreateMap<ServiceDto, UccountServiceEntity>().ReverseMap();
            //#endregion

            #region PERSONS
            CreateMap<PersonDto, PersonEntity>().ReverseMap();
            #endregion
        }
    }
}
