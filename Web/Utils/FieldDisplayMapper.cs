using System;
using System.ComponentModel.DataAnnotations;

using Core.Extension;

namespace Web.Utils {
    public static class FieldDisplayMapper {
        public static string GetHTMLType(this Enum enumValue) {
            var enumDisplay = enumValue.GetAttribute<DisplayAttribute>().Name;

            switch(enumDisplay) {
                case "String":
                    return "text";
                case "Number":
                    return "number";
                case "Date Time":
                    return "date";
                case "True of False":
                    return "checkbox";
                case "Password":
                    return "password";
                case "E-mail":
                    return "email";
                case "Link":
                    return "url";
            }

            return enumDisplay;
        }
    }
}