using OpenQA.Selenium.Support.UI;
using OpenQA.Selenium;
using SeleniumExtras.WaitHelpers;

namespace EmailAutomation
{
    public class MessageSenderService
    {
        private readonly WebDriverWait _wait;

        public MessageSenderService(IWebDriver driver)
        {
            _wait = new WebDriverWait(driver, TimeSpan.FromSeconds(3));
        }

        public void SendMessage(string email, string title, string message)
        {
            var composeButton = _wait.Until(ExpectedConditions.ElementToBeClickable(By.XPath("/html/body/div[7]/div[3]/div/div[2]/div[1]/div[1]/div/div")));
            composeButton.Click();

            var toInput = _wait.Until(ExpectedConditions.ElementExists(By.XPath(".//input[@peoplekit-id='BbVjBd']")));
            var titleInput = _wait.Until(ExpectedConditions.ElementExists(By.XPath(".//input[@placeholder='Subject']")));
            var messageInput = _wait.Until(ExpectedConditions.ElementToBeClickable(By.XPath(".//div[@contenteditable='true']")));
            var sendButton = _wait.Until(ExpectedConditions.ElementToBeClickable(By.XPath(".//div[text()='Send']")));

            toInput.SendKeys(email);
            titleInput.SendKeys(title);
            messageInput.SendKeys(message);

            sendButton.Click();
        }
    }
}
