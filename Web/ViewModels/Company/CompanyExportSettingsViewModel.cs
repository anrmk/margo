using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Web.ViewModels {
    public class CompanyExportSettingsViewModel {
        public long Id { get; set; }

        [Display(Name = "Company")]
        public long CompanyId { get; set; }

        [Required]
        [Display(Name = "Name")]
        public string Name { get; set; }

        [Display(Name = "File title")]
        public string Title { get; set; }

        [Display(Name = "Include all customers")]
        public bool IncludeAllCustomers { get; set; }

        [Display(Name = "Sort field")]
        public CompanyExportSettingsSortBy Sort { get; set; } = CompanyExportSettingsSortBy.BUSINESS_NAME;

        [Display(Name = "Fields")]
        public List<CompanyExportSettingsFieldViewModel> Fields { get; set; }
    }

    public class CompanyExportSettingsFieldViewModel {
        public long Id { get; set; }

        public long ExportSettingsId { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public string Value { get; set; }

        public int Sort { get; set; }

        [Display(Name = "Is active")]
        public bool IsActive { get; set; }

        [Display(Name = "Is editable")]
        public bool IsEditable { get; set; }
    }

    public enum CompanyExportSettingsSortBy {
        ACCOUNT_NUMBER = 0,
        BUSINESS_NAME = 1
    }
}
