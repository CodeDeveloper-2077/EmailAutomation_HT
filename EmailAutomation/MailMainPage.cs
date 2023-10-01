using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using SeleniumExtras.PageObjects;

namespace EmailAutomation
{
    public class MailMainPage
    {
        private readonly IWebDriver _driver;
        private readonly WebDriverWait _wait;
        private readonly string url = "https://www.gmail.com";

        public MailMainPage(IWebDriver driver)
        {
            _driver = driver;
            _wait = new WebDriverWait(_driver, TimeSpan.FromSeconds(30));
            PageFactory.InitElements(_driver, this);
        }

        [FindsBy(How = How.Id, Using = "identifierId")]
        public IWebElement EmailInput { get; set; }

        [FindsBy(How = How.XPath, Using = "//*[@id=\"identifierNext\"]/div/button/span")]
        public IWebElement NextButton { get; set; }

        public void Navigate()
        {
            _driver.Navigate().GoToUrl(url);
        }

        public void Login(string email, string password)
        {
            EmailInput.SendKeys(email);
            NextButton.Click();

            var passwordInput = _wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementExists(By.XPath("//*[@id=\"password\"]/div[1]/div/div[1]/input")));
            var passwordNextButton = _wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementToBeClickable(By.XPath("//*[@id=\"passwordNext\"]/div/button/span")));

            passwordInput.SendKeys(password);
            passwordNextButton.Click();
        }
    }
}