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
using System;

namespace CharityEvent.UnitTest
{
    public class OrganiserControllerTest
    {
        IConfiguration _configuration;
        public OrganiserControllerTest()
        {
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
        public void Post_SignUp_Successful()
        {
            // Generate a random number
            Random r = new Random();
            var x = r.Next(0, 1000000);
            string s = x.ToString("000000");

            // Arrange
            var mockHttpContext = Substitute.For<HttpContext>();

            OrganiserController mockController = new OrganiserController(_configuration);
            mockController.ControllerContext = new ControllerContext
            {
                HttpContext = mockHttpContext
            };

            OrganiserModel testModel = new OrganiserModel()
            {
                Email = "hsinhaochen" + s + "@gmail.com",
                Password = "cxh199621*",
                CompanyName = "Smartisan",
                Description = "Pursue craftsmanship",
                PhoneNumber = "+353877" + s,
                Address = "UCD",
                Zip = "D04ND61"
  
            };

            // Act
            var okResult = mockController.Post(testModel) as OkObjectResult;

            // Assert
            Assert.IsType<OkObjectResult>(okResult);
            Assert.NotNull(okResult);
            Assert.Equal(200, okResult.StatusCode);
        }

        [Fact]
        public void Post_SignUp_Failed()
        {
            // Arrange
            var mockHttpContext = Substitute.For<HttpContext>();

            OrganiserController mockController = new OrganiserController(_configuration);
            mockController.ControllerContext = new ControllerContext
            {
                HttpContext = mockHttpContext
            };

            OrganiserModel testModel = new OrganiserModel()
            {
                Email = "hsinhaochen334@gmail.com",
                Password = "cxh199621*",
                CompanyName = "Smartisan",
                Description = "Pursue craftsmanship",
                PhoneNumber = "+353877062967",
                Address = "UCD",
                Zip = "D04ND61"
            };

            // Act
            var badRequestResult = mockController.Post(testModel) as BadRequestObjectResult;

            // Assert
            Assert.IsType<BadRequestObjectResult>(badRequestResult);
            Assert.NotNull(badRequestResult);
            Assert.Equal(400, badRequestResult.StatusCode);
        }

        [Fact]
        public void Get_OrganisersEvents_Successful()
        {
            
            // Arrange
            var mockHttpContext = Substitute.For<HttpContext>();

            OrganiserController mockController = new OrganiserController(_configuration);
            mockController.ControllerContext = new ControllerContext
            {
                HttpContext = mockHttpContext
            };

            var mockEmail = "abolt1505@gmail.com";

            // Act
            var okResult = mockController.getOrganisersEventList(mockEmail) as OkObjectResult;

            // Assert
            Assert.IsType<OkObjectResult>(okResult);
            Assert.NotNull(okResult);
            Assert.Equal(200, okResult.StatusCode);
        }

        [Fact]
        public void Get_OrganisersEvents_Failed()
        {
            // Arrange
            var mockHttpContext = Substitute.For<HttpContext>();

            OrganiserController mockController = new OrganiserController(_configuration);
            mockController.ControllerContext = new ControllerContext
            {
                HttpContext = mockHttpContext
            };

            var mockEmail = "420@gmail.com";

            // Act
            var badRequestResult = mockController.getOrganisersEventList(mockEmail) as BadRequestObjectResult;

            // Assert
            Assert.IsType<BadRequestObjectResult>(badRequestResult);
            Assert.NotNull(badRequestResult);
            Assert.Equal(400, badRequestResult.StatusCode);
        }

        [Fact]
        public void Get_OrganisersDetails_Successful()
        {

            // Arrange
            var mockHttpContext = Substitute.For<HttpContext>();

            OrganiserController mockController = new OrganiserController(_configuration);
            mockController.ControllerContext = new ControllerContext
            {
                HttpContext = mockHttpContext
            };

            var mockEmail = "abolt1505@gmail.com";

            // Act
            var okResult = mockController.getOrganisersInfo(mockEmail) as OkObjectResult;

            // Assert
            Assert.IsType<OkObjectResult>(okResult);
            Assert.NotNull(okResult);
            Assert.Equal(200, okResult.StatusCode);
        }

        [Fact]
        public void Post_VerifyEmail_Successful()
        {

            // Arrange
            var mockHttpContext = Substitute.For<HttpContext>();

            OrganiserController mockController = new OrganiserController(_configuration);
            mockController.ControllerContext = new ControllerContext
            {
                HttpContext = mockHttpContext
            };

            VerificationModel testModel = new VerificationModel()
            {
                Email = "hsinhaochen052069@gmail.com",
                VerificationCode = "knpvlSnEN4Gg+eUXUd20QA=="
            };

            // Act
            var okResult = mockController.verifyEmail(testModel) as OkObjectResult;

            // Assert
            Assert.IsType<OkObjectResult>(okResult);
            Assert.NotNull(okResult);
            Assert.Equal(200, okResult.StatusCode);
        }

        [Fact]
        public void Post_VerifyEmail_Failed()
        {
            // Arrange
            var mockHttpContext = Substitute.For<HttpContext>();

            OrganiserController mockController = new OrganiserController(_configuration);
            mockController.ControllerContext = new ControllerContext
            {
                HttpContext = mockHttpContext
            };

            VerificationModel testModel = new VerificationModel()
            {
                Email = "hsinhaochen052069@gmail.com",
                VerificationCode = "wrong_knpvlSnEN4Gg+eUXUd20QA=="
            };

            // Act
            var badRequestResult = mockController.verifyEmail(testModel) as BadRequestObjectResult;

            // Assert
            Assert.IsType<BadRequestObjectResult>(badRequestResult);
            Assert.NotNull(badRequestResult);
            Assert.Equal(400, badRequestResult.StatusCode);
            
        }

        [Fact]
        public void Get_SendEmailVerification_Successful()
        {

            // Arrange
            var mockHttpContext = Substitute.For<HttpContext>();

            OrganiserController mockController = new OrganiserController(_configuration);
            mockController.ControllerContext = new ControllerContext
            {
                HttpContext = mockHttpContext
            };

            var mockEmail = "abolt1505@gmail.com";

            // Act
            var okResult = mockController.sendEmailVerification(mockEmail) as OkObjectResult;

            // Assert
            Assert.IsType<OkObjectResult>(okResult);
            Assert.NotNull(okResult);
            Assert.Equal(200, okResult.StatusCode);
        }

    }
}
