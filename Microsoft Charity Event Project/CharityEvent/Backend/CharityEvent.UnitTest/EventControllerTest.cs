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

namespace CharityEvent.UnitTest
{
    public class EventControllerTest
    {
        IConfiguration _configuration;

        public EventControllerTest()
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
        public void Post_CreateEvent_Successful()
        {
            // Arrange
            var mockHttpContext = Substitute.For<HttpContext>();

            EventController mockController = new EventController(_configuration);
            mockController.ControllerContext = new ControllerContext
            {
                HttpContext = mockHttpContext
            };

            EventModel testModel = new EventModel()
            {
                Email = "test_company@company.com",
                Title = "Test_title",
                Type = "Test_type",
                Cost = 10,
                Registration_begin = new System.DateTime(2021, 5, 11, 10, 0, 0),
                Registration_end = new System.DateTime(2021, 6, 1, 6, 0, 0),
                Privacy = false,
                IBAN = "IE30AIBK11111111111111",
                Image_path = "/image/test.png",
                Description = "test description.",
                MaxNumberOfParticipants = 20,
                AvailableSelections = new List<string> { "1", "2", "3"},
                PayoutSplitPercentageForWinner = 1
            };

            // Act
            var okResult = mockController.Post(testModel) as OkObjectResult;

            // Assert
            Assert.IsType<OkObjectResult>(okResult);
            Assert.NotNull(okResult);
            Assert.Equal(200, okResult.StatusCode);
        }

        [Fact]
        public void Post_CreateEvent_Failed()
        {
            // Arrange
            var mockHttpContext = Substitute.For<HttpContext>();

            EventController mockController = new EventController(_configuration);
            mockController.ControllerContext = new ControllerContext
            {
                HttpContext = mockHttpContext
            };

            EventModel testModel = new EventModel()
            {
                Email = "not_a_company@company.com",
                Title = "Test_title",
                Type = "Test_type",
                Cost = 10,
                Registration_begin = new System.DateTime(2021, 5, 11, 10, 0, 0),
                Registration_end = new System.DateTime(2021, 6, 1, 6, 0, 0),
                Privacy = false,
                IBAN = "IE30AIBK11111111111111",
                Image_path = "/image/test.png",
                Description = "test description.",
                MaxNumberOfParticipants = 20,
                AvailableSelections = new List<string> { "1", "2", "3" },
                PayoutSplitPercentageForWinner = 1
            };

            // Act
            var badResult = mockController.Post(testModel) as BadRequestObjectResult;

            // Assert
            Assert.IsType<BadRequestObjectResult>(badResult);
            Assert.NotNull(badResult);
            Assert.Equal(400, badResult.StatusCode);
        }

        [Fact]
        public void Get_Event_By_Type_Successful()
        {
            // Arrange
            var mockHttpContext = Substitute.For<HttpContext>();

            EventController mockController = new EventController(_configuration);
            mockController.ControllerContext = new ControllerContext
            {
                HttpContext = mockHttpContext
            };

            string mockType = "Test_type";

            // Act
            var okResult = mockController.getEventsByType(mockType) as OkObjectResult;

            // Assert
            Assert.IsType<OkObjectResult>(okResult);
            Assert.NotNull(okResult);
            Assert.Equal(200, okResult.StatusCode);
        }

        [Fact]
        public void Get_Event_By_Type_Failed()
        {
            // Arrange
            var mockHttpContext = Substitute.For<HttpContext>();

            EventController mockController = new EventController(_configuration);
            mockController.ControllerContext = new ControllerContext
            {
                HttpContext = mockHttpContext
            };

            string mockType = "Invalid_Test_type";

            // Act
            var badResult = mockController.getEventsByType(mockType) as BadRequestObjectResult;

            // Assert
            Assert.IsType<BadRequestObjectResult>(badResult);
            Assert.NotNull(badResult);
            Assert.Equal(400, badResult.StatusCode);
        }

        [Fact]
        public void Get_SearchEventsByTypeAndSearch_Successful()
        {
            // Arrange
            var mockHttpContext = Substitute.For<HttpContext>();

            EventController mockController = new EventController(_configuration);
            mockController.ControllerContext = new ControllerContext
            {
                HttpContext = mockHttpContext
            };

            string mockType = "Test_type";
            string mockSearch = "Test_title";

            // Act
            var okResult = mockController.searchForEvent(mockType, mockSearch) as OkObjectResult;

            // Assert
            Assert.IsType<OkObjectResult>(okResult);
            Assert.NotNull(okResult);
            Assert.Equal(200, okResult.StatusCode);
        }

        [Fact]
        public void Get_SearchEventsByTypeAndSearch_Failed()
        {
            // Arrange
            var mockHttpContext = Substitute.For<HttpContext>();

            EventController mockController = new EventController(_configuration);
            mockController.ControllerContext = new ControllerContext
            {
                HttpContext = mockHttpContext
            };

            string mockType = "Invalid_Test_type";
            string mockSearch = "Invalid_Test_title";

            // Act
            var badResult = mockController.searchForEvent(mockType, mockSearch) as BadRequestObjectResult;

            // Assert
            Assert.IsType<BadRequestObjectResult>(badResult);
            Assert.NotNull(badResult);
            Assert.Equal(400, badResult.StatusCode);
        }

        [Fact]
        public void Get_NotifyEventParticipants_Successful()
        {

            // Arrange
            var mockHttpContext = Substitute.For<HttpContext>();

            EventController mockController = new EventController(_configuration);
            mockController.ControllerContext = new ControllerContext
            {
                HttpContext = mockHttpContext
            };

            int mockEventId = 273;

            // Act
            var okResult = mockController.notifyEventParticipants(mockEventId) as OkObjectResult;

            // Assert
            Assert.IsType<OkObjectResult>(okResult);
            Assert.NotNull(okResult);
            Assert.Equal(200, okResult.StatusCode);
        }

        [Fact]
        public void Get_NotifyEventParticipants_Failed()
        {
            // Arrange
            var mockHttpContext = Substitute.For<HttpContext>();

            EventController mockController = new EventController(_configuration);
            mockController.ControllerContext = new ControllerContext
            {
                HttpContext = mockHttpContext
            };

            int mockEventId = 272;

            // Act
            var badResult = mockController.notifyEventParticipants(mockEventId) as BadRequestObjectResult;

            // Assert
            Assert.IsType<BadRequestObjectResult>(badResult);
            Assert.NotNull(badResult);
            Assert.Equal(400, badResult.StatusCode);
        }

        [Fact]
        public void Post_EliminatePlayer_Successful()
        {
            // Arrange
            var mockHttpContext = Substitute.For<HttpContext>();

            EventController mockController = new EventController(_configuration);
            mockController.ControllerContext = new ControllerContext
            {
                HttpContext = mockHttpContext
            };

            NotifyPlayerModel testModel = new NotifyPlayerModel()
            {
                EventId = 273,
                TicketId = 207,
                Email = "test_company@company.com",
                Title = "Test_title",
                Prize = 0.00
            };

            // Act
            var okResult = mockController.EliminatePlayer(testModel) as OkObjectResult;

            // Assert
            Assert.IsType<OkObjectResult>(okResult);
            Assert.NotNull(okResult);
            Assert.Equal(200, okResult.StatusCode);
        }

        [Fact]
        public void Post_SelectWinner_Successful()
        {
            // Arrange
            var mockHttpContext = Substitute.For<HttpContext>();

            EventController mockController = new EventController(_configuration);
            mockController.ControllerContext = new ControllerContext
            {
                HttpContext = mockHttpContext
            };

            NotifyPlayerModel testModel = new NotifyPlayerModel()
            {
                EventId = 273,
                TicketId = 207,
                Email = "test_company@company.com",
                Title = "Test_title",
                Prize = 0.00
            };

            // Act
            var okResult = mockController.SelectWinner(testModel) as OkObjectResult;

            // Assert
            Assert.IsType<OkObjectResult>(okResult);
            Assert.NotNull(okResult);
            Assert.Equal(200, okResult.StatusCode);
        }

        [Fact]
        public void Get_EventPageById_Successful()
        {

            // Arrange
            var mockHttpContext = Substitute.For<HttpContext>();

            EventController mockController = new EventController(_configuration);
            mockController.ControllerContext = new ControllerContext
            {
                HttpContext = mockHttpContext
            };

            int mockEventId = 273;

            // Act
            var okResult = mockController.getSpecificEventDisplayPage(mockEventId) as OkObjectResult;

            // Assert
            Assert.IsType<OkObjectResult>(okResult);
            Assert.NotNull(okResult);
            Assert.Equal(200, okResult.StatusCode);
        }

        //[Fact]
        //public void Get_Get_EventPageById_Failed()
        //{
        //    // Arrange
        //    var mockHttpContext = Substitute.For<HttpContext>();

        //    EventController mockController = new EventController(_configuration);
        //    mockController.ControllerContext = new ControllerContext
        //    {
        //        HttpContext = mockHttpContext
        //    };

        //    int mockEventId = 270;

        //    // Act
        //    var badResult = mockController.getSpecificEventDisplayPage(mockEventId) as BadRequestObjectResult;

        //    // Assert
        //    Assert.IsType<BadRequestObjectResult>(badResult);
        //    Assert.NotNull(badResult);
        //    Assert.Equal(400, badResult.StatusCode);
        //}

        [Fact]
        public void Get_EventId_Successful()
        {

            // Arrange
            var mockHttpContext = Substitute.For<HttpContext>();

            EventController mockController = new EventController(_configuration);
            mockController.ControllerContext = new ControllerContext
            {
                HttpContext = mockHttpContext
            };

            string mockInviteId = "c";

            // Act
            var okResult = mockController.geteventid(mockInviteId) as OkObjectResult;

            // Assert
            Assert.IsType<OkObjectResult>(okResult);
            Assert.NotNull(okResult);
            Assert.Equal(200, okResult.StatusCode);
        }

        [Fact]
        public void Get_Get_EventId_Failed()
        {
            // Arrange
            var mockHttpContext = Substitute.For<HttpContext>();

            EventController mockController = new EventController(_configuration);
            mockController.ControllerContext = new ControllerContext
            {
                HttpContext = mockHttpContext
            };

            string mockInviteId = "a";

            // Act
            var badResult = mockController.geteventid(mockInviteId) as BadRequestObjectResult;

            // Assert
            Assert.IsType<BadRequestObjectResult>(badResult);
            Assert.NotNull(badResult);
            Assert.Equal(400, badResult.StatusCode);
        }

        [Fact]
        public void Get_PlayerSelections_Successful()
        {

            // Arrange
            var mockHttpContext = Substitute.For<HttpContext>();

            EventController mockController = new EventController(_configuration);
            mockController.ControllerContext = new ControllerContext
            {
                HttpContext = mockHttpContext
            };

            string mockEventId = "298";

            // Act
            var okResult = mockController.playerSelections(mockEventId) as OkObjectResult;

            // Assert
            Assert.IsType<OkObjectResult>(okResult);
            Assert.NotNull(okResult);
            Assert.Equal(200, okResult.StatusCode);
        }

        [Fact]
        public void Get_PlayerSelections_Failed()
        {
            // Arrange
            var mockHttpContext = Substitute.For<HttpContext>();

            EventController mockController = new EventController(_configuration);
            mockController.ControllerContext = new ControllerContext
            {
                HttpContext = mockHttpContext
            };

            string mockEventId = "null";

            // Act
            var badResult = mockController.playerSelections(mockEventId) as BadRequestObjectResult;

            // Assert
            Assert.IsType<BadRequestObjectResult>(badResult);
            Assert.NotNull(badResult);
            Assert.Equal(400, badResult.StatusCode);
        }

        [Fact]
        public void Post_NotifyEliminatedParticipant_Successful()
        {
            // Arrange
            var mockHttpContext = Substitute.For<HttpContext>();

            EventController mockController = new EventController(_configuration);
            mockController.ControllerContext = new ControllerContext
            {
                HttpContext = mockHttpContext
            };

            NotifyPlayerModel testModel = new NotifyPlayerModel()
            {
                EventId = 273,
                TicketId = 207,
                Email = "nick1996nick2012@gmail.com",
                Title = "Euro 2021",
                Prize = 4.00,
                name = "nick"
            };

            // Act
            var okResult = mockController.notifyEliminatedParticipant(testModel) as OkObjectResult;

            // Assert
            Assert.IsType<OkObjectResult>(okResult);
            Assert.NotNull(okResult);
            Assert.Equal(200, okResult.StatusCode);
        }

        [Fact]
        public void Post_NotifyEliminatedParticipant_Failed()
        {
            // Arrange
            var mockHttpContext = Substitute.For<HttpContext>();

            EventController mockController = new EventController(_configuration);
            mockController.ControllerContext = new ControllerContext
            {
                HttpContext = mockHttpContext
            };

            NotifyPlayerModel testModel = new NotifyPlayerModel()
            {
                EventId = 200,
                TicketId = 207,
                Email = "nick1996nick2012@gmail.com",
                Title = "Euro 2021",
                Prize = 4.00,
                name = "nick"
            };

            // Act
            var badResult = mockController.notifyEliminatedParticipant(testModel) as BadRequestObjectResult;

            // Assert
            Assert.IsType<BadRequestObjectResult>(badResult);
            Assert.NotNull(badResult);
            Assert.Equal(400, badResult.StatusCode);
        }

        [Fact]
        public void Post_NotifyWinningParticipant_Successful()
        {
            // Arrange
            var mockHttpContext = Substitute.For<HttpContext>();

            EventController mockController = new EventController(_configuration);
            mockController.ControllerContext = new ControllerContext
            {
                HttpContext = mockHttpContext
            };

            NotifyPlayerModel testModel = new NotifyPlayerModel()
            {
                EventId = 273,
                TicketId = 207,
                Email = "nick1996nick2012@gmail.com",
                Title = "Euro 2021",
                Prize = 4.00,
                name = "nick"
            };

            // Act
            var okResult = mockController.notifyWinningParticipant(testModel) as OkObjectResult;

            // Assert
            Assert.IsType<OkObjectResult>(okResult);
            Assert.NotNull(okResult);
            Assert.Equal(200, okResult.StatusCode);
        }

        //[Fact]
        //public void Post_NotifyWinningParticipant_Failed()
        //{
        //    // Arrange
        //    var mockHttpContext = Substitute.For<HttpContext>();

        //    EventController mockController = new EventController(_configuration);
        //    mockController.ControllerContext = new ControllerContext
        //    {
        //        HttpContext = mockHttpContext
        //    };

        //    NotifyPlayerModel testModel = new NotifyPlayerModel()
        //    {
        //        EventId = 273,
        //        TicketId = 207,
        //        Email = "nick1996nick2012@gmail.com",
        //        Title = "Euro 2021",
        //        Prize = 4.00,
        //        name = "nick"
        //    };

        //    // Act
        //    var badResult = mockController.notifyWinningParticipant(testModel) as BadRequestObjectResult;

        //    // Assert
        //    Assert.IsType<BadRequestObjectResult>(badResult);
        //    Assert.NotNull(badResult);
        //    Assert.Equal(400, badResult.StatusCode);
        //}

        [Fact]
        public void Post_StartEvent_Successful()
        {
            // Arrange
            var mockHttpContext = Substitute.For<HttpContext>();

            EventController mockController = new EventController(_configuration);
            mockController.ControllerContext = new ControllerContext
            {
                HttpContext = mockHttpContext
            };

            var mockEventId = 134;

            // Act
            var okResult = mockController.startEvent(mockEventId) as OkObjectResult;

            // Assert
            Assert.IsType<OkObjectResult>(okResult);
            Assert.NotNull(okResult);
            Assert.Equal(200, okResult.StatusCode);
        }

        [Fact]
        public void Get_CheckEventStatus_Successful()
        {

            // Arrange
            var mockHttpContext = Substitute.For<HttpContext>();

            EventController mockController = new EventController(_configuration);
            mockController.ControllerContext = new ControllerContext
            {
                HttpContext = mockHttpContext
            };

            var mockEventId = 273;

            // Act
            var okResult = mockController.checkeventstatus(mockEventId) as OkObjectResult;

            // Assert
            Assert.IsType<OkObjectResult>(okResult);
            Assert.NotNull(okResult);
            Assert.Equal(200, okResult.StatusCode);
        }

        [Fact]
        public void Get_Get_CheckEventStatus_Failed()
        {
            // Arrange
            var mockHttpContext = Substitute.For<HttpContext>();

            EventController mockController = new EventController(_configuration);
            mockController.ControllerContext = new ControllerContext
            {
                HttpContext = mockHttpContext
            };

            var mockEventId = 298;

            // Act
            var badResult = mockController.checkeventstatus(mockEventId) as BadRequestObjectResult;

            // Assert
            Assert.IsType<BadRequestObjectResult>(badResult);
            Assert.NotNull(badResult);
            Assert.Equal(400, badResult.StatusCode);
        }

        [Fact]
        public void Post_Cancel_Event_Successful()
        {
            // Arrange
            var mockHttpContext = Substitute.For<HttpContext>();

            EventController mockController = new EventController(_configuration);
            mockController.ControllerContext = new ControllerContext
            {
                HttpContext = mockHttpContext
            };

            var mockEventId = "392";

            // Act
            var okResult = mockController.cancelevent(mockEventId) as OkObjectResult;

            // Assert
            Assert.IsType<OkObjectResult>(okResult);
            Assert.NotNull(okResult);
            Assert.Equal(200, okResult.StatusCode);
        }

        [Fact]
        public void Get_PickedSelections_Successful()
        {

            // Arrange
            var mockHttpContext = Substitute.For<HttpContext>();

            EventController mockController = new EventController(_configuration);
            mockController.ControllerContext = new ControllerContext
            {
                HttpContext = mockHttpContext
            };

            string mockEventId = "298";

            // Act
            var okResult = mockController.PickedSelections(mockEventId) as OkObjectResult;

            // Assert
            Assert.IsType<OkObjectResult>(okResult);
            Assert.NotNull(okResult);
            Assert.Equal(200, okResult.StatusCode);
        }

        [Fact]
        public void Get_PickedSelections_Failed()
        {
            // Arrange
            var mockHttpContext = Substitute.For<HttpContext>();

            EventController mockController = new EventController(_configuration);
            mockController.ControllerContext = new ControllerContext
            {
                HttpContext = mockHttpContext
            };

            string mockEventId = "134";

            // Act
            var badResult = mockController.PickedSelections(mockEventId) as BadRequestObjectResult;

            // Assert
            Assert.IsType<BadRequestObjectResult>(badResult);
            Assert.NotNull(badResult);
            Assert.Equal(400, badResult.StatusCode);
        }
    }
}


