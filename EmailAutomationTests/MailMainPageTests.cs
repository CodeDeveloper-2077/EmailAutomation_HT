using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;
using System.Net.Http.Headers;

namespace EmailAutomationTests
{
    [TestClass]
    public class MailMainPageTests
    {
        public IWebDriver Driver { get; set; }

        public WebDriverWait Wait { get; set; }

        [TestInitialize]
        public void TestInitialize()
        {
            Driver = new ChromeDriver();
            Driver.Manage().Window.Maximize();
            Wait = new WebDriverWait(Driver, TimeSpan.FromMinutes(1.5));
        }

        [TestMethod]
        [DataRow("shevchenkooleh8@gmail.com", "asd12sd45")]
        public void Login_ShouldLogInToAccountWithCorrectData(string email, string password)
        {
            //Arrange
            MailMainPage mainPage = new MailMainPage(Driver, this.GenerationServiceFactory());

            //Act
            mainPage.Navigate();
            mainPage.Login(email, password);

            //Assert
            var expectedElement = Wait.Until(ExpectedConditions.ElementExists(By.XPath("//*[@id=\":2u\"]/div")));
            Assert.IsTrue(expectedElement.Text.Contains("Inbox"));
        }

        [TestMethod]
        [DataRow("shevchenkooleh8@gmail.com", "hjfskhfsduw")]
        public void Login_ShouldNotLogInToAccountWithWrongOrEmptyCredentials(string email, string password)
        {
            //Arrange
            MailMainPage mainPage = new MailMainPage(Driver, this.GenerationServiceFactory());

            //Act
            mainPage.Navigate();
            mainPage.Login(email, password);

            //Assert
            var expectedElement = Wait.Until(ExpectedConditions.ElementExists(By.XPath("//*[@id=\"yDmH0d\"]/c-wiz/div/div[2]/div/div[1]/div/form/span/section[2]/div/div/div[1]/div[2]/div[2]/span")));
            Assert.IsTrue(expectedElement.Text.Contains("Wrong password. Try again or click Forgot password to reset it."));
        }

        [TestMethod]
        [DataRow("", "")]
        public void Login_ShouldNotLogInToAccountWithEmptyCredentials(string email, string password)
        {
            //Arrange
            MailMainPage mainPage = new MailMainPage(Driver, this.GenerationServiceFactory());

            //Act
            mainPage.Navigate();
            mainPage.EmailInput.SendKeys(email);
            mainPage.NextButton.Click();

            //Assert
            var expectedElement = Wait.Until(ExpectedConditions.ElementExists(By.XPath("//*[@id=\"yDmH0d\"]/c-wiz/div/div[2]/div/div[1]/div/form/span/section/div/div/div[1]/div/div[2]")));
            Assert.IsTrue(expectedElement.Text.Contains("Enter an email or phone number"));
        }

        [TestMethod]
        [DataRow("shevchenkooleh8@gmail.com", "asd12sd45", "shevchenkooleh442@gmail.com")]
        public void SendMessage_ShouldSendMessageToAnotherMailbox(string firstEmail, string password, string secondEmail)
        {
            //Arrange
            MailMainPage mainPage = new MailMainPage(Driver, this.GenerationServiceFactory());

            //Act
            mainPage.Navigate();
            mainPage.Login(firstEmail, password);
            mainPage.SendMessage(secondEmail);

            //Assert
            var expectedElement = Wait.Until(ExpectedConditions.ElementExists(By.XPath(".//span[text()='Message sent']")));
            Assert.IsNotNull(expectedElement);

            Driver.Quit();
            TestInitialize();
            mainPage = new MailMainPage(Driver, this.GenerationServiceFactory());
            mainPage.Navigate();
            mainPage.Login(secondEmail, password);
            mainPage.SendMessage(firstEmail, title: "New Alias", message: "New Alias for User");
        }

        [TestMethod]
        [DataRow("shevchenkooleh8@gmail.com", "asd12sd45")]
        public void ChangeNickname_ShouldChangeUserAlias(string email, string password)
        {
            //Arrange
            var mainPage = new MailMainPage(Driver, this.GenerationServiceFactory());

            //Act
            mainPage.Navigate();
            mainPage.Login(email, password);
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
        }

        private MessageGenerationService GenerationServiceFactory()
        {
            return new MessageGenerationService();
        }
    }
}