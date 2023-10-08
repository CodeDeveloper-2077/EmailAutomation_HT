using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using SeleniumExtras.PageObjects;
using SeleniumExtras.WaitHelpers;

namespace EmailAutomation
{
    public class MailMainPage
    {
        private readonly string _url = "https://www.gmail.com";
        private readonly IWebDriver _driver;
        private readonly WebDriverWait _wait;

        public MailMainPage(IWebDriver driver)
        {
            _driver = driver;
            _wait = new WebDriverWait(_driver, TimeSpan.FromMinutes(3));
            PageFactory.InitElements(_driver, this);
        }

        [FindsBy(How = How.Id, Using = "identifierId")]
        public IWebElement EmailInput { get; set; }

        [FindsBy(How = How.XPath, Using = "//*[@id='identifierNext']/div/button/span")]
        public IWebElement NextButton { get; set; }

        public void Navigate()
        {
            _driver.Navigate().GoToUrl(_url);
        }

        public void Login(string email, string password)
        {
            EmailInput.SendKeys(email);
            NextButton.Click();

            var passwordInput = _wait.Until(ExpectedConditions.ElementExists(By.XPath("//*[@id='password']/div[1]/div/div[1]/input")));
            var passwordNextButton = _wait.Until(ExpectedConditions.ElementToBeClickable(By.XPath("//*[@id='passwordNext']/div/button/span")));

            passwordInput.SendKeys(password);
            passwordNextButton.Click();
        }

        public void ChangeNickname(string email, string password)
        {
            this.Login(email, password);

            var accountButton = _wait.Until(ExpectedConditions.ElementToBeClickable(By.XPath("//*[@id='gb']/div[2]/div[3]/div[1]/div[2]/div/a")));
            accountButton.Click();

            var manageButton = _wait.Until(ExpectedConditions.ElementToBeClickable(By.XPath(".//*[text()='Manage your Google Account']")));
            manageButton.Click();
        }

        public string GetEmptyPasswordErrorMessage()
        {
            var expectedElement = _wait.Until(ExpectedConditions.ElementExists(By.XPath("//*[@id='yDmH0d']/c-wiz/div/div[2]/div/div[1]/div/form/span/section[2]/div/div/div[1]/div[2]/div[2]/span")));

            return expectedElement.Text;
        }

        public string GetEmptyEmailErrorMessage()
        {
            var expectedElement = _wait.Until(ExpectedConditions.ElementExists(By.XPath("//*[@id='yDmH0d']/c-wiz/div/div[2]/div/div[1]/div/form/span/section/div/div/div[1]/div/div[2]")));

            return expectedElement.Text;
        }

        public string GetSuccessfulSentMessageNotification()
        {
            var expectedElement = _wait.Until(ExpectedConditions.ElementExists(By.XPath(".//span[text()='Message sent']")));

            return expectedElement.Text;
        }

        public string GetSuccessfulLoginMessage()
        {
            var expectedElement = _wait.Until(ExpectedConditions.ElementExists(By.XPath(".//*[text()='Inbox']")));

            return expectedElement.Text;
        }
    }
}