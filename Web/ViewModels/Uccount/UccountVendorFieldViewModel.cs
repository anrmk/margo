using System.ComponentModel.DataAnnotations;
using Core.Data.Enums;
using Core.Extension;

namespace Web.ViewModels {
    public class UccountVendorFieldViewModel {
        public long Id { get; set; }
        public string Name { get; set; }
        public string Value { get; set; }
        public FieldEnum Type { get; set; }
        public string TypeName => Type.GetAttribute<DisplayAttribute>().Name;
        public bool IsRequired { get; set; }
    }
}
