using Xunit;
using Backend.Controllers;
using Backend.Helpers;
using Backend.Models;
using Microsoft.Extensions.Configuration;
using System.Web;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Net;
using NSubstitute;
using MySql.Data.MySqlClient;
using System;

namespace CharityEvent.UnitTest
{
    public class LoginControllerTest
    {

        JwtService _service;
        IConfiguration _configuration;
        private IRequestCookieCollection _sCookieCollection;
        private HttpContext _sHttpContext;
        private HttpRequest _sHttpRequest;

        public LoginControllerTest()
        {
            _service = new JwtService();

            Dictionary<string, string> myConfiguration = new Dictionary<string, string>
            {
                {
                    "DBConnection",
                    "server=mysqlcharityevent.mysql.database.azure.com;user=thunderbirds@mysqlcharityevent;database=charityevent2;port=3306;password=CharityEvent!"
                }
            };

            _configuration = new ConfigurationBuilder()
                                 .AddInMemoryCollection(myConfiguration)
                                 .Build();
        }

        [Fact]
        public void Post_Login_Successful()
        {

            var mockHttpContext = Substitute.For<HttpContext>();

            LoginController _controller = new LoginController(_service, _configuration);
            _controller.ControllerContext = new ControllerContext
            {
                HttpContext = mockHttpContext
            };

            LoginModel testUser = new LoginModel()
            {
                Email = "hsinhaochen146560@gmail.com",
                Password = "cxh199621*"
            };

            var okResult = _controller.Post(testUser) as OkObjectResult;

            Assert.IsType<OkObjectResult>(okResult);
            Assert.NotNull(okResult);
            Assert.Equal(200, okResult.StatusCode);
        }

        [Fact]
        public void Post_Login_Failed()
        {
            var mockHttpContext = Substitute.For<HttpContext>();

            LoginController _controller = new LoginController(_service, _configuration);
            _controller.ControllerContext = new ControllerContext
            {
                HttpContext = mockHttpContext
            };

            // Arrange
            LoginModel testUser = new LoginModel()
            {
                Email = "hsinhaochen146560@gmail.com",
                Password = "cxh199621*_wrong"
            };

            // Act
            var badRequestResult = _controller.Post(testUser) as BadRequestObjectResult;

            // Assert
            Assert.IsType<BadRequestObjectResult>(badRequestResult);
            Assert.NotNull(badRequestResult);
            Assert.Equal(400, badRequestResult.StatusCode);
        }

        [Fact]
        public void Post_Logout_Successful()
        {

            var mockHttpContext = Substitute.For<HttpContext>();

            LoginController _controller = new LoginController(_service, _configuration);
            _controller.ControllerContext = new ControllerContext
            {
                HttpContext = mockHttpContext
            };

            var okResult = _controller.logout() as OkObjectResult;

            Assert.IsType<OkObjectResult>(okResult);
            Assert.NotNull(okResult);
            Assert.Equal(200, okResult.StatusCode);
        }

        //[Fact]
        //public void Post_Logout_Failed()
        //{
        //    var mockHttpContext = Substitute.For<HttpContext>();

        //    LoginController _controller = new LoginController(_service, _configuration);
        //    _controller.ControllerContext = new ControllerContext
        //    {
        //        HttpContext = mockHttpContext
        //    };

        //    var badRequestResult = _controller.logout() as BadRequestObjectResult;

        //    Assert.IsType<BadRequestObjectResult>(badRequestResult);
        //    Assert.NotNull(badRequestResult);
        //    Assert.Equal(400, badRequestResult.StatusCode);
        //}

        [Fact]
        public void Post_Verify2FA_Successful()
        {

            var mockHttpContext = Substitute.For<HttpContext>();

            LoginController _controller = new LoginController(_service, _configuration);
            _controller.ControllerContext = new ControllerContext
            {
                HttpContext = mockHttpContext
            };

            // Arrange
            VerificationModel test2FACode = new VerificationModel()
            {
                Email = "nick1996nick2012@gmail.com",
                VerificationCode = "100764"
            };

            // Act
            var okResult = _controller.verify2FA(test2FACode) as OkObjectResult;

            // Assert
            Assert.IsType<OkObjectResult>(okResult);
            Assert.NotNull(okResult);
            Assert.Equal(200, okResult.StatusCode);
        }

        [Fact]
        public void Post_Verify2FA_Failed()
        {
            var mockHttpContext = Substitute.For<HttpContext>();

            LoginController _controller = new LoginController(_service, _configuration);
            _controller.ControllerContext = new ControllerContext
            {
                HttpContext = mockHttpContext
            };

            // Arrange
            VerificationModel test2FACode = new VerificationModel()
            {
                Email = "nick1996nick2012@gmail.com",
                VerificationCode = "100765"
            };

            // Act
            var badRequestResult = _controller.verify2FA(test2FACode) as BadRequestObjectResult;

            // Assert
            Assert.IsType<BadRequestObjectResult>(badRequestResult);
            Assert.NotNull(badRequestResult);
            Assert.Equal(400, badRequestResult.StatusCode);
        }

        [Fact]
        public void Post_SendResetPasswordLink_Successful()
        {

            var mockHttpContext = Substitute.For<HttpContext>();

            LoginController _controller = new LoginController(_service, _configuration);
            _controller.ControllerContext = new ControllerContext
            {
                HttpContext = mockHttpContext
            };

            var testEmail = "hsinhaochen146560@gmail.com";

            var okResult = _controller.sendResetPasswordLink(testEmail) as OkObjectResult;

            // Assert
            Assert.IsType<OkObjectResult>(okResult);
            Assert.NotNull(okResult);
            Assert.Equal(200, okResult.StatusCode);
        }

        [Fact]
        public void Post_ResetPassword_Successful()
        {

            var mockHttpContext = Substitute.For<HttpContext>();

            LoginController _controller = new LoginController(_service, _configuration);
            _controller.ControllerContext = new ControllerContext
            {
                HttpContext = mockHttpContext
            };

            // Arrange
            AuthenticationModel testResetPassword = new AuthenticationModel()
            {
                Email = "hsinhaochen146560@gmail.com",
                Password = "cxh199621*",
                NewPassword = "cxh199621*",
                VerificationCode = "235731"
            };

            // Act
            var okResult = _controller.ResetPass(testResetPassword) as OkObjectResult;

            // Assert
            Assert.IsType<OkObjectResult>(okResult);
            Assert.NotNull(okResult);
            Assert.Equal(200, okResult.StatusCode);
        }

        [Fact]
        public void Post_ResetPassword_Failed()
        {
            var mockHttpContext = Substitute.For<HttpContext>();

            LoginController _controller = new LoginController(_service, _configuration);
            _controller.ControllerContext = new ControllerContext
            {
                HttpContext = mockHttpContext
            };

            // Arrange
            AuthenticationModel testResetPassword = new AuthenticationModel()
            {
                Email = "hsinhaochen146560@gmail.com",
                Password = "wrong_cxh199621*",
                NewPassword = "cxh199621*",
                VerificationCode = "235731"
            };

            // Act
            var badRequestResult = _controller.ResetPass(testResetPassword) as BadRequestObjectResult;

            // Assert
            Assert.IsType<BadRequestObjectResult>(badRequestResult);
            Assert.NotNull(badRequestResult);
            Assert.Equal(400, badRequestResult.StatusCode);
        }

        // Need to change VerificationCode each run
        //[Fact]
        //public void Post_ResetForgotPassword_Successful()
        //{

        //    var mockHttpContext = Substitute.For<HttpContext>();

        //    LoginController _controller = new LoginController(_service, _configuration);
        //    _controller.ControllerContext = new ControllerContext
        //    {
        //        HttpContext = mockHttpContext
        //    };

        //    // Arrange
        //    AuthenticationModel testResetPassword = new AuthenticationModel()
        //    {
        //        Email = "hsinhaochen146560@gmail.com",
        //        Password = "cxh199621*",
        //        NewPassword = "cxh199621*",
        //        VerificationCode = "wlTsYBumBNfrX59XFdNPX6xcmQiCcK"
        //    };

        //    // Act
        //    var okResult = _controller.resetForgotPassword(testResetPassword) as OkObjectResult;

        //    // Assert
        //    Assert.IsType<OkObjectResult>(okResult);
        //    Assert.NotNull(okResult);
        //    Assert.Equal(200, okResult.StatusCode);
        //}

        [Fact]
        public void Post_ResetForgotPassword_Failed()
        {
            var mockHttpContext = Substitute.For<HttpContext>();

            LoginController _controller = new LoginController(_service, _configuration);
            _controller.ControllerContext = new ControllerContext
            {
                HttpContext = mockHttpContext
            };

            // Arrange
            AuthenticationModel testResetPassword = new AuthenticationModel()
            {
                Email = "hsinhaochen146560@gmail.com",
                Password = "wrong_cxh199621*",
                NewPassword = "cxh199621*",
                VerificationCode = "wrong_svGudLgDe6WvahMWI8lm1LsWxOEsnI"
            };

            // Act
            var badRequestResult = _controller.resetForgotPassword(testResetPassword) as BadRequestObjectResult;

            // Assert
            Assert.IsType<BadRequestObjectResult>(badRequestResult);
            Assert.NotNull(badRequestResult);
            Assert.Equal(400, badRequestResult.StatusCode);
        }

        [Fact]
        public void Get_NumberOfUserAccounts_Successful()
        {

            var mockHttpContext = Substitute.For<HttpContext>();

            LoginController _controller = new LoginController(_service, _configuration);
            _controller.ControllerContext = new ControllerContext
            {
                HttpContext = mockHttpContext
            };

            // Arrange
            var testAccount = "hsinhaochen146560@gmail.com";

            // Act
            var okResult = _controller.numberOfAccountsRegistered(testAccount) as OkObjectResult;

            // Assert
            Assert.IsType<OkObjectResult>(okResult);
            Assert.NotNull(okResult);
            Assert.Equal(200, okResult.StatusCode);
        }

        //[Fact]
        //public void Post_Enable2FA()
        //{

        //    var mockHttpContext = Substitute.For<HttpContext>();

        //    LoginController _controller = new LoginController(_service, _configuration);
        //    _controller.ControllerContext = new ControllerContext
        //    {
        //        HttpContext = mockHttpContext
        //    };

        //    var testEmail = "hsinhaochen334@gmail.com";

        //    var okResult = _controller.enable2FA(testEmail) as OkObjectResult;

        //    Assert.IsType<OkObjectResult>(okResult);
        //    Assert.NotNull(okResult);
        //    Assert.Equal(200, okResult.StatusCode);
        //}

        //[Fact]
        //public void Post_Disable2FA()
        //{
        //    var mockHttpContext = Substitute.For<HttpContext>();

        //    LoginController _controller = new LoginController(_service, _configuration);
        //    _controller.ControllerContext = new ControllerContext
        //    {
        //        HttpContext = mockHttpContext
        //    };

        //    var testEmail = "hsinhaochen334@gmail.com";

        //    var badRequestResult = _controller.diable2FA(testEmail) as BadRequestObjectResult;

        //    Assert.IsType<BadRequestObjectResult>(badRequestResult);
        //    Assert.NotNull(badRequestResult);
        //    Assert.Equal(400, badRequestResult.StatusCode);
        //}

        //[Fact]
        //public void Get_User_Successful()
        //{
        //    _sCookieCollection = Substitute.For<IRequestCookieCollection>();
        //    _sHttpRequest = Substitute.For<HttpRequest>();
        //    var mockHttpContext = Substitute.For<HttpContext>();
        //    mockHttpContext.Request.Returns(_sHttpRequest);
        //    var cookieList = new List<KeyValuePair<string, string>>
        //    {
        //        new KeyValuePair<string,string>("token", _service.generate("abc@gmail.com"))
        //    };
        //    _sCookieCollection.GetEnumerator().Returns(cookieList.GetEnumerator());
        //    mockHttpContext.Request.Cookies.Returns(_sCookieCollection);
        //    //var cookies = new CookieCollection();
        //    //cookies.Add(new Cookie("token", _service.generate("abc@gmail.com")));
        //    //_sCookieCollection.ContainsKey("token").Returns(true);
        //    //_sHttpRequest.Cookies.Returns(_sCookieCollection);
        //    LoginController _controller = new LoginController(_service, _configuration);
        //    _controller.ControllerContext = new ControllerContext
        //    {
        //        HttpContext = mockHttpContext
        //    };
        //    // TODO: mock cookies for validate.
        //    var okResult = _controller.User("Organiser") as OkObjectResult;

        //    Assert.IsType<OkObjectResult>(okResult);
        //    Assert.NotNull(okResult);
        //    Assert.Equal(200, okResult.StatusCode);
        //}

    }   
}
