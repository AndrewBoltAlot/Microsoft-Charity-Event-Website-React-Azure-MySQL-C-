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
    public class TicketControllerTest
    {
        IConfiguration _configuration;

        public TicketControllerTest()
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
        public void Post_Generate_Ticket_Successful()
        {
            // Generate a random number
            Random r = new Random();
            var x = r.Next(0, 1000000);
            string s = x.ToString("000000");

            // Arrange
            var mockHttpContext = Substitute.For<HttpContext>();

            TicketController mockController = new TicketController(_configuration);
            mockController.ControllerContext = new ControllerContext
            {
                HttpContext = mockHttpContext
            };

            TicketModel testModel = new TicketModel()
            {
                Email = "nick1996nick2012@gmail.com",
                Price = 20.00,
                EventId = 298,
                Selection = s,
                participantName = "nick zlokapa",
                EventTitle = "Olympics 2020"
            };

            // Act
            var okResult = mockController.Post(testModel) as OkObjectResult;

            // Assert
            Assert.IsType<OkObjectResult>(okResult);
            Assert.NotNull(okResult);
            Assert.Equal(200, okResult.StatusCode);

        }

        [Fact]
        public void Post_Generate_Ticket_Failed()
        {
            // Arrange
            var mockHttpContext = Substitute.For<HttpContext>();

            TicketController mockController = new TicketController(_configuration);
            mockController.ControllerContext = new ControllerContext
            {
                HttpContext = mockHttpContext
            };

            TicketModel testModel = new TicketModel()
            {
                Email = "anurag.thoke@ucdconnect.ie_no_exist",
                Price = 200.00,
                EventId = 203
            };

            // Act
            var badRequestResult = mockController.Post(testModel) as BadRequestObjectResult;

            // Assert
            Assert.IsType<BadRequestObjectResult>(badRequestResult);
            Assert.NotNull(badRequestResult);
            Assert.Equal(400, badRequestResult.StatusCode);

        }
    }
}
