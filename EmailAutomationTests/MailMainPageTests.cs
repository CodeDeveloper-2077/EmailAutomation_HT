using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;

namespace EmailAutomationTests
{
    [TestClass]
    public class MailMainPageTests
    {
        private IWebDriver _driver;

        private MailMainPage _mainPage;

        [TestInitialize]
        public void TestInitialize()
        {
            _driver = new ChromeDriver();
            _driver.Manage().Window.Maximize();
            _mainPage = this.MainPageFactory();
        }

        [TestMethod]
        [DataRow("shevchenkooleh8@gmail.com", "asd12sd45")]
        public void Login_ShouldLogInToAccountWithCorrectData(string email, string password)
        {
            //Act
            _mainPage.Navigate();
            _mainPage.Login(email, password);

            //Assert
            Assert.AreEqual("Inbox", _mainPage.GetSuccessfulLoginMessage());
            _driver.Quit();
        }

        [TestMethod]
        [DataRow("shevchenkooleh8@gmail.com", "hjfskhfsduw")]
        public void Login_ShouldNotLogInToAccountWithWrongCredentials(string email, string password)
        {
            //Act
            _mainPage.Navigate();
            _mainPage.Login(email, password);

            //Assert
            Assert.AreEqual("Wrong password. Try again or click Forgot password to reset it.", _mainPage.GetEmptyPasswordErrorMessage());
            _driver.Quit();
        }

        [TestMethod]
        [DataRow("", "")]
        public void Login_ShouldNotLogInToAccountWithEmptyCredentials(string email, string password)
        {
            //Act
            _mainPage.Navigate();
            _mainPage.EmailInput.SendKeys(email);
            _mainPage.NextButton.Click();

            //Assert
            Assert.AreEqual("Enter an email or phone number", _mainPage.GetEmptyEmailErrorMessage());
            _driver.Quit();
        }

        [TestMethod]
        [DataRow("shevchenkooleh8@gmail.com", "asd12sd45", "shevchenkooleh442@gmail.com")]
        public void SendMessage_ShouldSendMessageToAnotherMailbox(string firstEmail, string password, string secondEmail)
        {
            //Arrange
            var messageSender = this.MessageSenderFactory();
            var messageGenerator = this.MessageGeneratorFactory(10, 40);

            //Act
            _mainPage.Navigate();
            _mainPage.Login(firstEmail, password);

            string title = messageGenerator.GenerateRandomMessage();

            messageGenerator = this.MessageGeneratorFactory(40, 150);
            string message = messageGenerator.GenerateRandomMessage();

            messageSender.SendMessage(secondEmail, title, message);

            //Assert
            Assert.AreEqual("Message sent", _mainPage.GetSuccessfulSentMessageNotification());

            _driver.Quit();
            TestInitialize();
            messageSender = this.MessageSenderFactory();
            _mainPage.Navigate();
            _mainPage.Login(secondEmail, password);
            messageSender.SendMessage(firstEmail, title: "New Alias", message: "New Alias for User");
        }

        [TestMethod]
        [DataRow("shevchenkooleh8@gmail.com", "asd12sd45")]
        public void ChangeNickname_ShouldChangeUserAlias(string email, string password)
        {
            //Act
            _mainPage.Navigate();
            _mainPage.Login(email, password);
            Thread.Sleep(1500);
            _mainPage.ChangeNickname();

            //Assert
            Assert.AreEqual("Test Name Shevchenko", _mainPage.GetSuccessfulNicknameChangedMessage(), "Nickname hasn't been changed!");
            _driver.Quit();
        }

        private MessageGenerationService MessageGeneratorFactory(int minLength, int maxLength)
        {
            return new MessageGenerationService(minLength, maxLength);
        }

        private MailMainPage MainPageFactory()
        {
            return new MailMainPage(_driver);
        }

        private MessageSenderWindow MessageSenderFactory()
        {
            return new MessageSenderWindow(_driver);
        }
    }
}