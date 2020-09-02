using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

using Core.Context;
using Core.Extension;

using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;

namespace Core.Services.Integration {
    public interface ISeleniumService<T> {
        Task<List<T>> Execute(params object[] keys);
    }

    public abstract class SeleniumService<T>: ISeleniumService<T> where T : class {
        protected IApplicationContext _context;
        private IWebDriver _webDriver;

        public IWebDriver Driver {
            get {
                if(_webDriver == null) {
                    var curDir = Environment.CurrentDirectory;
                    var location = curDir + @"\Drivers\";
                    ChromeDriverService service = ChromeDriverService.CreateDefaultService(location, "chromedriver.exe");
                    var options = new ChromeOptions() {
                    };
                    //options.AddArgument("headless");
                    _webDriver = new ChromeDriver(service, options);
                }
                return _webDriver;
            }
        }

        protected SeleniumService(IApplicationContext context) {
            _context = context;
            //Driver.Manage().Window.Minimize();
        }

        public virtual Task<List<T>> Execute(params object[] keys) {
            throw new NotImplementedException();
        }

        public IWebElement WaitUntilElementExists(By elementLocator, int timeout = 5000) {
            WebDriverWait wait = new WebDriverWait(Driver, new TimeSpan(timeout));
            Console.WriteLine(DateTime.Now + "  Wait Element");
            return wait.Until(condition => {
                try {
                    var element = Driver.FindElement(elementLocator);
                    Console.WriteLine(DateTime.Now + "  Wait Element");

                    return element.Displayed ? element : null;
                } catch(NoSuchElementException er) {
                    Console.WriteLine(er.Message ?? er.StackTrace);
                    return null;
                }
            });
        }

        public void Down() => Driver.Quit();
    }
}
