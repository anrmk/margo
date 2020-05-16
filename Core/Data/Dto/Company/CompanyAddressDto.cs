using System.Text;

namespace Core.Data.Dto {
    public class CompanyAddressDto {
        public long Id { get; set; }
        public string Address { get; set; }
        public string Address2 { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string Country { get; set; }
        public string ZipCode { get; set; }

        public override string ToString() {
            var add = new[] { new[] { Address, ", " }, new[] { Address2, ", " }, new[] { City, ", " }, new[] { State, " " }, new[] { Country, ", " }, new[] { ZipCode, null } };
            string suffix = null;
            var sb = new StringBuilder();
            foreach(var item in add) {
                if(!string.IsNullOrWhiteSpace(item[0])) {
                    sb.Append(suffix);
                    sb.Append(item[0]);
                    suffix = item[1];
                }
            }

            return sb.ToString();
        }
    }
}
