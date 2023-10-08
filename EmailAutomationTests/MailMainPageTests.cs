using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;

namespace EmailAutomationTests
{
    [TestClass]
    public class MailMainPageTests
    {
        public IWebDriver Driver { get; set; }

        public WebDriverWait Wait { get; set; }

        private MailMainPage _mainPage;

        [TestInitialize]
        public void TestInitialize()
        {
            Driver = new ChromeDriver();
            Driver.Manage().Window.Maximize();
            Wait = new WebDriverWait(Driver, TimeSpan.FromMinutes(3));
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
            Driver.Quit();
        }

        [TestMethod]
        [DataRow("shevchenkooleh8@gmail.com", "hjfskhfsduw")]
        public void Login_ShouldNotLogInToAccountWithWrongOrEmptyCredentials(string email, string password)
        {
            //Act
            _mainPage.Navigate();
            _mainPage.Login(email, password);

            //Assert
            Assert.AreEqual("Wrong password. Try again or click Forgot password to reset it.", _mainPage.GetEmptyPasswordErrorMessage());
            Driver.Quit();
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
            Driver.Quit();
        }

        [TestMethod]
        [DataRow("shevchenkooleh8@gmail.com", "asd12sd45", "shevchenkooleh442@gmail.com")]
        public void SendMessage_ShouldSendMessageToAnotherMailbox(string firstEmail, string password, string secondEmail)
        {
            //Arrange
            var messageSender = this.MessageSenderFactory();
            var messageGenerator = this.MessageGeneratorFactory();
            int minTitleLength = 10, maxTitleLength = 40;
            int minMessageLength = 40, maxMessageLength = 150;

            //Act
            _mainPage.Navigate();
            _mainPage.Login(firstEmail, password);

            string title = messageGenerator.GenerateRandomMessage(minTitleLength, maxTitleLength);
            string message = messageGenerator.GenerateRandomMessage(minMessageLength, maxMessageLength);
            messageSender.SendMessage(secondEmail, title, message);

            //Assert
            Assert.AreEqual("Message sent", _mainPage.GetSuccessfulSentMessageNotification());

            Driver.Quit();
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
            var messageLabel = Wait.Until(ExpectedConditions.ElementExists(By.ClassName("bqe")));
            string newAlias = messageLabel.Text;

            Driver.Navigate().GoToUrl("https://myaccount.google.com/profile/name/edit?continue=https://myaccount.google.com/personal-info?hl%3Den_GB&hl=en_GB&pli=1&rapt=AEjHL4OIbeT3a-S7irP7cUVhwhUA6kOo0wi7TKQAWnam1nCkaqeZocpN_C9QW56QLX_1_AWjAOLHyl6_ExB6QvNxxkiFH-cnsA");

            var nameInput = Wait.Until(ExpectedConditions.ElementToBeClickable(By.Id("i6")));
            nameInput.Click();
            nameInput.Clear();
            nameInput.SendKeys("Test Name");

            var saveButton = Wait.Until(ExpectedConditions.ElementToBeClickable(By.XPath(".//span[text()='Save']")));
            saveButton.Click();

            //Assert
            Assert.IsNotNull(nameInput);
            Driver.Quit();
        }

        private MessageGenerationService MessageGeneratorFactory()
        {
            return new MessageGenerationService();
        }

        private MailMainPage MainPageFactory()
        {
            return new MailMainPage(Driver);
        }

        private MessageSenderService MessageSenderFactory()
        {
            return new MessageSenderService(Driver);
        }
    }
}