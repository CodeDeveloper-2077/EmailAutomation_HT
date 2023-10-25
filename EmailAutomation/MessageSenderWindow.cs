using OpenQA.Selenium.Support.UI;
using OpenQA.Selenium;
using SeleniumExtras.WaitHelpers;

namespace EmailAutomation
{
    public class MessageSenderWindow
    {
        private readonly By _composeButtonLocator = By.XPath("//div[text()='Compose']");
        private readonly By _toInputLocator = By.XPath(".//input[@peoplekit-id='BbVjBd']");
        private readonly By _titleInputLocator = By.XPath(".//input[@placeholder='Subject']");
        private readonly By _messageInputLocator = By.XPath(".//div[@contenteditable='true']");
        private readonly By _sendButtonLocator = By.XPath(".//div[text()='Send']");

        private readonly WebDriverWait _wait;

        public MessageSenderWindow(IWebDriver driver)
        {
            _wait = new WebDriverWait(driver, TimeSpan.FromSeconds(3));
        }

        public void SendMessage(string email, string title, string message)
        {
            var composeButton = _wait.Until(ExpectedConditions.ElementToBeClickable(_composeButtonLocator));
            composeButton.Click();

            var toInput = _wait.Until(ExpectedConditions.ElementExists(_toInputLocator));
            var titleInput = _wait.Until(ExpectedConditions.ElementExists(_titleInputLocator));
            var messageInput = _wait.Until(ExpectedConditions.ElementToBeClickable(_messageInputLocator));
            var sendButton = _wait.Until(ExpectedConditions.ElementToBeClickable(_sendButtonLocator));

            toInput.SendKeys(email);
            titleInput.SendKeys(title);
            messageInput.SendKeys(message);

            sendButton.Click();
        }
    }
}
