using OpenQA.Selenium;
using OpenQA.Selenium.DevTools.V117.Storage;
using OpenQA.Selenium.Support.UI;
using SeleniumExtras.PageObjects;
using SeleniumExtras.WaitHelpers;

namespace EmailAutomation
{
    public class MailMainPage
    {
        private readonly IWebDriver _driver;
        private readonly WebDriverWait _wait;
        private readonly MessageGenerationService _generationService;
        private readonly string url = "https://www.gmail.com";

        public MailMainPage(IWebDriver driver, MessageGenerationService generationService)
        {
            _driver = driver;
            _wait = new WebDriverWait(_driver, TimeSpan.FromMinutes(1));
            PageFactory.InitElements(_driver, this);
            _generationService = generationService; 
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

            var passwordInput = _wait.Until(ExpectedConditions.ElementExists(By.XPath("//*[@id=\"password\"]/div[1]/div/div[1]/input")));
            var passwordNextButton = _wait.Until(ExpectedConditions.ElementToBeClickable(By.XPath("//*[@id=\"passwordNext\"]/div/button/span")));

            passwordInput.SendKeys(password);
            passwordNextButton.Click();
        }

        public void SendMessage(string email, string? title = default, string? message = default)
        {
            var composeButton = _wait.Until(ExpectedConditions.ElementToBeClickable(By.XPath("/html/body/div[7]/div[3]/div/div[2]/div[1]/div[1]/div/div")));
            composeButton.Click();

            var toInput = _wait.Until(ExpectedConditions.ElementExists(By.XPath(".//input[@peoplekit-id='BbVjBd']")));
            var titleInput = _wait.Until(ExpectedConditions.ElementExists(By.XPath(".//input[@placeholder='Subject']")));
            var messageInput = _wait.Until(ExpectedConditions.ElementToBeClickable(By.XPath(".//div[@contenteditable='true']")));
            var sendButton = _wait.Until(ExpectedConditions.ElementToBeClickable(By.XPath(".//div[text()='Send']")));

            int minTitleLength = 10, maxTitleLength = 40;
            int minMessageLength = 40, maxMessageLength = 150;

            if (string.IsNullOrEmpty(title))
            {
                title = _generationService.GenerateRandomMessage(minTitleLength, maxTitleLength);
            }

            if (string.IsNullOrEmpty(message))
            {
                message = _generationService.GenerateRandomMessage(minMessageLength, maxMessageLength);
            }

            toInput.SendKeys(email);
            titleInput.SendKeys(title);
            messageInput.SendKeys(message);

            sendButton.Click();
        }
    }
}