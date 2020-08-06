using System.ComponentModel.DataAnnotations;

using Core.Data.Enums;
using Core.Extension;
using Web.Utils;

namespace Web.ViewModels {
    public class UccountVendorFieldViewModel {
        public long Id { get; set; }
        public string Name { get; set; }
        public string Value { get; set; }
        public FieldEnum Type { get; set; }
        public string TypeName => Type.GetAttribute<DisplayAttribute>().Name.ToLower();
        public string HTMLTypeName => Type.GetHTMLType();
        public bool IsRequired { get; set; }

        public UccountVendorFieldViewModel() {
            Value = "";
        }
    }
}
