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
    public class ParticipantControllerTest
    {
        IConfiguration _configuration;
        public ParticipantControllerTest()
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

            ParticipantController mockController = new ParticipantController(_configuration);
            mockController.ControllerContext = new ControllerContext
            {
                HttpContext = mockHttpContext
            };

            ParticipantModel testModel = new ParticipantModel()
            {
                Email = "hsinhaochen" + s + "@gmail.com",
                Password = "cxh199621*",
                FirstName = "Xinhao",
                LastName = "Chen",
                PhoneNumber = "+353877" + s,
                DOB = new System.DateTime(1996, 2, 1),
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

            ParticipantController mockController = new ParticipantController(_configuration);
            mockController.ControllerContext = new ControllerContext
            {
                HttpContext = mockHttpContext
            };

            ParticipantModel testModel = new ParticipantModel()
            {
                Email = "test_participant@service.com",
                Password = "test_pwd",
                FirstName = "test_first_name",
                LastName = "test_last_name",
                PhoneNumber = "+353 874122221",
                DOB = new System.DateTime(1998, 6, 1),
                Address = "A test location",
                Zip = "D04A7P6"
            };

            // Act
            var badRequestResult = mockController.Post(testModel) as BadRequestObjectResult;

            // Assert
            Assert.IsType<BadRequestObjectResult>(badRequestResult);
            Assert.NotNull(badRequestResult);
            Assert.Equal(400, badRequestResult.StatusCode);
        }

        [Fact]
        public void Get_ParticipantEvents_Successful()
        {

            // Arrange
            var mockHttpContext = Substitute.For<HttpContext>();

            ParticipantController mockController = new ParticipantController(_configuration);
            mockController.ControllerContext = new ControllerContext
            {
                HttpContext = mockHttpContext
            };

            var mockEmail = "nick1996nick2012@gmail.com";

            // Act
            var okResult = mockController.getParticipantEventList(mockEmail) as OkObjectResult;

            // Assert
            Assert.IsType<OkObjectResult>(okResult);
            Assert.NotNull(okResult);
            Assert.Equal(200, okResult.StatusCode);
        }

        [Fact]
        public void Get_ParticipantEvents_Failed()
        {
            // Arrange
            var mockHttpContext = Substitute.For<HttpContext>();

            ParticipantController mockController = new ParticipantController(_configuration);
            mockController.ControllerContext = new ControllerContext
            {
                HttpContext = mockHttpContext
            };

            var mockEmail = "abolt1405@gmail.com";

            // Act
            var badRequestResult = mockController.getParticipantEventList(mockEmail) as BadRequestObjectResult;

            // Assert
            Assert.IsType<BadRequestObjectResult>(badRequestResult);
            Assert.NotNull(badRequestResult);
            Assert.Equal(400, badRequestResult.StatusCode);
        }

        [Fact]
        public void Get_ParticipantDetails_Successful()
        {

            // Arrange
            var mockHttpContext = Substitute.For<HttpContext>();

            ParticipantController mockController = new ParticipantController(_configuration);
            mockController.ControllerContext = new ControllerContext
            {
                HttpContext = mockHttpContext
            };

            var mockEmail = "nick1996nick2012@gmail.com";

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
            // Generate a random number
            Random r = new Random();
            var x = r.Next(0, 1000000);
            string s = x.ToString("000000");

            // Arrange
            var mockHttpContext = Substitute.For<HttpContext>();

            ParticipantController mockController = new ParticipantController(_configuration);
            mockController.ControllerContext = new ControllerContext
            {
                HttpContext = mockHttpContext
            };

            VerificationModel testModel = new VerificationModel()
            {
                Email = "nick1996nick2012@gmail.com",
                VerificationCode = "O6SOttBODhjnzb/V9oj/6Q=="
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

            ParticipantController mockController = new ParticipantController(_configuration);
            mockController.ControllerContext = new ControllerContext
            {
                HttpContext = mockHttpContext
            };

            VerificationModel testModel = new VerificationModel()
            {
                Email = "nick1996nick2012@gmail.com",
                VerificationCode = "wrong_O6SOttBODhjnzb/V9oj/6Q=="
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

            ParticipantController mockController = new ParticipantController(_configuration);
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

