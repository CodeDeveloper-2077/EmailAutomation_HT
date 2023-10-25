using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using SeleniumExtras.PageObjects;
using SeleniumExtras.WaitHelpers;

namespace EmailAutomation
{
    public class MailMainPage
    {
        private readonly By _passwordInputLocator = By.XPath("//*[@id='password']//input");
        private readonly By _passwordNextButtonLocator = By.XPath("//*[@id='passwordNext']//span");
        private readonly By _nicknameFieldLocator = By.XPath("//*[@id='yDmH0d']//div[@class='gWjfMb']");
        private readonly By _emptyPasswordLabelLocator = By.XPath("//*[@id='yDmH0d']//span[2]");
        private readonly By _successfulSentLabelLocator = By.XPath(".//span[text()='Message sent']");
        private readonly By _successfulLoginLabelLocator = By.XPath(".//*[text()='Inbox']");
        private readonly By _emptyEmailLabelLocator = By.XPath("//*[@id='yDmH0d']//div[contains(@class, 'o6cuMc')]");
        private readonly string _url = "https://www.gmail.com";

        private readonly IWebDriver _driver;
        private readonly WebDriverWait _wait;

        public MailMainPage(IWebDriver driver)
        {
            _driver = driver;
            _wait = new WebDriverWait(_driver, TimeSpan.FromSeconds(10));
            PageFactory.InitElements(_driver, this);
        }

        [FindsBy(How = How.Id, Using = "identifierId")]
        public IWebElement EmailInput { get; set; }

        [FindsBy(How = How.XPath, Using = "//*[@id='identifierNext']//span")]
        public IWebElement NextButton { get; set; }

        [FindsBy(How = How.Id, Using = "i6")]
        public IWebElement NameInput { get; set; }

        [FindsBy(How = How.XPath, Using = ".//span[text()='Save']")]
        public IWebElement SaveElement { get; set; }

        public void Navigate()
        {
            _driver.Navigate().GoToUrl(_url);
        }

        public void Login(string email, string password)
        {
            EmailInput.SendKeys(email);
            NextButton.Click();

            var passwordInput = _wait.Until(ExpectedConditions.ElementExists(_passwordInputLocator));
            var passwordNextButton = _wait.Until(ExpectedConditions.ElementToBeClickable(_passwordNextButtonLocator));

            passwordInput.SendKeys(password);
            passwordNextButton.Click();
        }

        public void ChangeNickname()
        {
            _driver.Navigate().GoToUrl("https://myaccount.google.com/profile/name/edit?continue=https://myaccount.google.com/personal-info?hl%3Den_GB&hl=en_GB&pli=1&rapt=AEjHL4OIbeT3a-S7irP7cUVhwhUA6kOo0wi7TKQAWnam1nCkaqeZocpN_C9QW56QLX_1_AWjAOLHyl6_ExB6QvNxxkiFH-cnsA");

            NameInput.Click();
            NameInput.Clear();
            NameInput.SendKeys("Test Name");

            SaveElement.Click();
        }

        public string GetEmptyPasswordErrorMessage()
        {
            var expectedElement = _wait.Until(ExpectedConditions.ElementExists(_emptyPasswordLabelLocator));

            return expectedElement.Text;
        }

        public string GetEmptyEmailErrorMessage()
        {
            var expectedElement = _wait.Until(ExpectedConditions.ElementExists(_emptyEmailLabelLocator));

            return expectedElement.Text;
        }

        public string GetSuccessfulSentMessageNotification()
        {
            var expectedElement = _wait.Until(ExpectedConditions.ElementExists(_successfulSentLabelLocator));

            return expectedElement.Text;
        }

        public string GetSuccessfulLoginMessage()
        {
            var expectedElement = _wait.Until(ExpectedConditions.ElementExists(_successfulLoginLabelLocator));

            return expectedElement.Text;
        }

        public string GetSuccessfulNicknameChangedMessage()
        {
            var nicknameField = _wait.Until(ExpectedConditions.ElementExists(_nicknameFieldLocator));
            return nicknameField.Text;
        }
    }
}