using System.Globalization;

namespace Core.Extension {
    public static class NumberExtensionMethods {
        public static bool IsBetween(this int value, int Min, int Max) {
            // return (value >= Min && value <= Max);
            if(value >= Min && value <= Max) return true;
            else return false;
        }

        public static string ToCurrency(this decimal value) {
            return $"{value:C}".Trim(new char[] { '(', ')' });
        }
    }
}
