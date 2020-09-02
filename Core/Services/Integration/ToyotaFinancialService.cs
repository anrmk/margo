using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

using Core.Context;
using Core.Data.Dto;
using Core.Extension;

using NLog;

using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Firefox;
using OpenQA.Selenium.Support.UI;

namespace Core.Services.Integration {
    public interface IToyotaFinancialService: ISeleniumService<InvoiceDto> {
        void Login();
        List<InvoiceDto> Invoice();
    }

    public class ToyotaFinancialService: SeleniumService<InvoiceDto>, IToyotaFinancialService {
        private UccountDto Uccount { get; set; }

        private string LoginLink {
            get {
                var linkField = Uccount.Fields.Where(x => x.Type == Data.Enums.FieldEnum.LINK).FirstOrDefault();
                return linkField.Value + @"/us/en/consumer-web/home/login?deeplink=account";
            }
        }

        private string UserName {
            get {
                var usernameField = Uccount.Fields.Where(x => x.Type == Data.Enums.FieldEnum.TEXT && x.Name.ToUpper() == "USERNAME").FirstOrDefault();
                return usernameField.Value;
            }
        }

        private string Password {
            get {
                var passField = Uccount.Fields.Where(x => x.Type == Data.Enums.FieldEnum.PASSWORD && x.Name.ToUpper() == "PASSWORD").FirstOrDefault();
                return passField.Value.Decrypt();
            }
        }

        public ToyotaFinancialService(IApplicationContext context) : base(context) {
        }

        public override Task<List<InvoiceDto>> Execute(params object[] keys) {
            Uccount = (UccountDto)keys[0];

            if(LoginLink == null) {
                return null;
            }
            var invoices = new List<InvoiceDto>();
            try {
                Login();
                //if(Driver.Url.Contains("/consumer-web/login/mfa")) {
                //    Console.WriteLine("Request verification code!");
                //    return null;
                //}

                invoices = Invoice();
                Driver.Quit();
            } catch(Exception er) {
                Console.WriteLine(er.Message ?? er.StackTrace);
                Driver.Quit();
            }
            return Task.FromResult(invoices);
        }

        public void Login() {
            Driver.Manage().Timeouts().PageLoad = TimeSpan.FromSeconds(10);
            Driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(10);

            Driver.Navigate().GoToUrl(LoginLink);

            Driver.FindElement(By.Id("login_username")).SendKeys(UserName);
            Driver.FindElement(By.Id("login_password")).SendKeys(Password);
            Driver.FindElement(By.Id("login_form_button_login")).Submit();
        }

        public List<InvoiceDto> Invoice() {
            var accountField = Driver.FindElement(By.CssSelector("div.account-info__label")).Text;
            var amountField = Driver.FindElement(By.CssSelector("div.account_amount_due")).Text;
            var dueDateField = Driver.FindElement(By.CssSelector("div.amount_due_date")).Text;

            DateTime dateTime;
            decimal amount;

            var accountString = Regex.Match(accountField, @"(?<=:)(.*?)(?=\s*\•)").Value;
            var dateTimeString = Regex.Match(dueDateField, @"(0?[1-9]|1[012])[- /.](0?[1-9]|[12][0-9]|3[01])[- /.](19|20)\d\d$").Value;
            var amountString = Regex.Match(amountField, @"[\d]+[\.][\d]{2}$").Value;

            var invoice = new InvoiceDto() {
                No = Guid.NewGuid().ToString("N").Substring(0, 16),
                AccountId = Uccount.Id,
                Services = new List<InvoiceServiceDto>()
            };

            var service = Uccount.Services.Where(x => x.Fields.Any(y => y.Value.Equals(accountString.Trim()     ))).FirstOrDefault();
            if(service != null) {
                var invoiceService = new InvoiceServiceDto() {
                    Name = service.CategoryName + "" + accountString,
                    Count = 1
                };

                if(decimal.TryParse(amountString, out amount)) {
                    invoiceService.Amount = amount;
                    invoice.Amount = amount;
                }

                invoice.Services.Add(invoiceService);
            }

            if(DateTime.TryParse(dateTimeString, out dateTime)) {
                invoice.DueDate = dateTime;
                invoice.Date = dateTime.FirstDayOfMonth();
            }

            return new List<InvoiceDto>() { invoice };
        }

        private void Payments() {
            Driver.Navigate().GoToUrl("https://www.toyotafinancial.com/us/en/consumer-web/secure/manage-account/payment-details");
            var tableElement = Driver.FindElement(By.XPath("/html/body/app-root/app-secure-container/main/mat-sidenav-container/mat-sidenav-content/div/app-manage-account-container/div/div/div/article/div/div[2]/app-payment-details/section/div[1]/app-transaction-history/mat-accordion/mat-expansion-panel/div/div/div/div[2]/div[1]/table"));
            var tableTrElement = tableElement.FindElements(By.TagName("tbody/tr"));
            foreach(var tr in tableTrElement) {
                var elem = tr.GetAttribute("");
            }
        }


    }
}
