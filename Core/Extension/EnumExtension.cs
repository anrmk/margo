using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;

namespace Core.Extension {
    public static class EnumExtension {
        public static TAttribute GetAttribute<TAttribute>(this Enum enumValue)
                where TAttribute : Attribute {
            return enumValue.GetType()
                .GetMember(enumValue.ToString())
                .First()
                .GetCustomAttribute<TAttribute>();
        }

        public static IEnumerable<(int Id, string Name)> GetAll<EnumType>()
                where EnumType : Enum {
            return Enum.GetValues(typeof(EnumType))
                .Cast<EnumType>()
                .Select(x => (
                    Convert.ToInt32(x),
                    x.GetAttribute<DisplayAttribute>().Name));
        }
    }
}
