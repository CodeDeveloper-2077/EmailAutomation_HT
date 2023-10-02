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

        private MessageGenerationService GenerationServiceFactory()
        {
            return new MessageGenerationService();
        }
    }
}