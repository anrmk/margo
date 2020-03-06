using System;

namespace Core.Extension {
    public static class DateTimeExtension {
        public static DateTime LastDayOfMonth(this DateTime date) {
            return date.AddDays(1 - (date.Day)).AddMonths(1).AddDays(-1);
        }

        public static DateTime FirstDayOfMonth(this DateTime date) {
            return new DateTime(date.Year, date.Month, 1);
        }
    }
}
