using Backend.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Helpers;
using Microsoft.Extensions.Configuration;
using Backend.Helpers;
using System.Text;
using Microsoft.AspNetCore.Http;
using Azure.Storage.Blobs;
using System.IO;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;
using System.Configuration;
using Azure.Storage.Blobs.Models;

namespace Backend.Controllers
{
    [ApiController]
    [Route("api")]
    public class EventController : Controller
    {
        private readonly IConfiguration _configuration;
        public EventController(IConfiguration configuration) 
        {
            _configuration = configuration;
        }

        [HttpPost("CreateEvent")]
        public IActionResult Post(EventModel @event)
        {
            String eventSelections = GetEventSelections(@event);

            String databaseConnection = _configuration["DBConnection"];
            //Connecting to the database
            using (MySqlConnection mySqlConnection = new MySqlConnection(databaseConnection))
            {
                mySqlConnection.Open();

                MySqlCommand insertIntoEventLocationCommand = mySqlConnection.CreateCommand();
                MySqlCommand updateImagePathCommand = mySqlConnection.CreateCommand();
                MySqlCommand insertIntoEventCommand = mySqlConnection.CreateCommand();
                MySqlTransaction QueryTransaction;
                QueryTransaction = mySqlConnection.BeginTransaction();
                insertIntoEventLocationCommand.Connection = mySqlConnection;
                insertIntoEventLocationCommand.Transaction = QueryTransaction;
                string invte_id = Hash.generateVerificationCode();
                

                try
                {

                    //Inserting Oraginser details into organiser table.
                    insertIntoEventCommand.CommandText = "insert into event(Email,Title,Type,Created_date,Cost,Registration_begin,Registration_end,Privacy,IBAN,Image_path,Description," +
                                                                            "MaxNumberOfParticipants,PayoutSplitPercentageForWinner,Invite_id)values" +
                    "(@Email,@Title,@Type,@Created_date,@Cost,@Registration_begin,@Registration_end,@Privacy,@IBAN,@Image_path,@Description,@MaxNumberOfParticipants,@PayoutSplitPercentageForWinner,@Invite_id)";

                    insertIntoEventCommand.Parameters.AddWithValue("@Email", @event.Email);
                    insertIntoEventCommand.Parameters.AddWithValue("@Title", @event.Title);
                    insertIntoEventCommand.Parameters.AddWithValue("@Type", @event.Type);
                    DateTime CreatedAt = DateTime.Now;
                    insertIntoEventCommand.Parameters.AddWithValue("@Created_date", CreatedAt);
                    insertIntoEventCommand.Parameters.AddWithValue("@Cost", @event.Cost);
                    insertIntoEventCommand.Parameters.AddWithValue("@Registration_begin", @event.Registration_begin);
                    insertIntoEventCommand.Parameters.AddWithValue("@Registration_end", @event.Registration_end);
                    insertIntoEventCommand.Parameters.AddWithValue("@Privacy", @event.Privacy);
                    insertIntoEventCommand.Parameters.AddWithValue("@IBAN", @event.IBAN);
                    insertIntoEventCommand.Parameters.AddWithValue("@Image_path", @event.Image_path);
                    insertIntoEventCommand.Parameters.AddWithValue("@Description", @event.Description);
                    insertIntoEventCommand.Parameters.AddWithValue("@MaxNumberOfParticipants", @event.MaxNumberOfParticipants);
                    insertIntoEventCommand.Parameters.AddWithValue("@PayoutSplitPercentageForWinner", @event.PayoutSplitPercentageForWinner);
                    insertIntoEventCommand.Parameters.AddWithValue("@Invite_id", invte_id);
                    insertIntoEventCommand.ExecuteNonQuery();
                    long eventID = insertIntoEventCommand.LastInsertedId;

                    insertIntoEventCommand.CommandText = eventSelections;
                    insertIntoEventCommand.ExecuteNonQuery();


                    QueryTransaction.Commit();


                    return Ok(eventID);  //Status 200


                }
                catch (Exception e)
                {
                    try
                    {
                        QueryTransaction.Rollback();
                    }
                    catch (SqlException ex)
                    {
                        if (QueryTransaction.Connection != null)
                        {
                            Console.WriteLine("An exception of type " + ex.GetType() +
                            " was encountered while attempting to roll back the transaction.");
                        }
                    }

                    Console.WriteLine("An exception of type " + e.GetType() +
                    " was encountered while inserting the data.");
                    Console.WriteLine("Neither record was written to database.");
                    return BadRequest("Bad");    //Status 400
                }
                finally
                {
                    mySqlConnection.Close();
                }
            }
        }

        private static String GetEventSelections(EventModel @event)
        {
            StringBuilder sCommand = new StringBuilder("INSERT INTO event_selections (Event_id, Selection) VALUES ");
            List<string> Rows = new List<string>();
            foreach (String s in @event.AvailableSelections)
            {
                Rows.Add(String.Format("({0},'{1}')", MySqlHelper.EscapeString("LAST_INSERT_ID()"), MySqlHelper.EscapeString(s)));
            }
            sCommand.Append(string.Join(",", Rows));
            sCommand.Append(";");
            return sCommand.ToString();
        }

        [HttpGet("getevents/{type}")]
        public IActionResult getEventsByType(String type)
        {
            List<EventModel> events = new List<EventModel>();

            String connection = _configuration["DBConnection"];
            using (MySqlConnection con = new MySqlConnection(connection))
            {
                con.Open();

                MySqlCommand selectevents = new MySqlCommand("SELECT count(te.ticket_id) AS NumberOfParticipants, event.Event_id, event.Email, Title, Type, Cost, Registration_end, Registration_begin, Privacy, IBAN, " +
                                                             "Image_path, Description, MaxNumberOfParticipants, PayoutSplitPercentageForWinner, CompetitionStarted, Company_name FROM event JOIN organiser o ON o.Email = event.email " +
                                                             "LEFT JOIN ticket_event te ON te.Event_id = event.Event_id WHERE Type = @type GROUP BY event.Event_id", con);        
                selectevents.Parameters.AddWithValue("@type", type);
                MySqlDataReader reader = selectevents.ExecuteReader();
                while (reader.Read())
                {
                    events.Add(new EventModel
                    {
                        EventId = reader.GetInt32("Event_id"),
                        Organiser = reader["Company_name"].ToString(),
                        Email = reader["Email"].ToString(),
                        Title = reader["Title"].ToString(),
                        Type = reader["Type"].ToString(),
                        Cost = reader.GetDouble("Cost"),
                        Registration_begin = reader.GetDateTime("Registration_begin"),
                        Registration_end = reader.GetDateTime("Registration_end"),
                        IBAN = reader["IBAN"].ToString(),
                        Image_path = reader["Image_path"].ToString(),
                        Description = reader["Description"].ToString(),
                        MaxNumberOfParticipants = reader.GetInt32("MaxNumberOfParticipants"),
                        PayoutSplitPercentageForWinner = reader.GetInt32("PayoutSplitPercentageForWinner"),
                        NumberOfParticipants = reader.GetInt32("NumberOfParticipants"),
                        CompetitionStarted = reader.GetBoolean("CompetitionStarted"),
                        Privacy = reader.GetBoolean("Privacy")
                    });
                }

                if (!reader.HasRows)
                {
                    return BadRequest("No events of such type");
                }
                reader.Close();
            }
            return Ok(events);
        }




        [HttpGet("searchEvents/{type}/{search}")]
        public IActionResult searchForEvent(string type, string search)
        {
            List<EventModel> events = new List<EventModel>();

            String connection = _configuration["DBConnection"];
            using (MySqlConnection con = new MySqlConnection(connection))
            {
                con.Open();

                MySqlCommand selectevents = new MySqlCommand("SELECT count(te.ticket_id) AS NumberOfParticipants, event.Event_id, event.Email, Title, Type, Cost, Registration_end, Registration_begin, Privacy, IBAN, " +
                                                             "Image_path, Description, MaxNumberOfParticipants, PayoutSplitPercentageForWinner, CompetitionStarted, Company_name FROM event JOIN organiser o ON o.Email = event.email " +
                                                             "LEFT JOIN ticket_event te ON te.Event_id = event.Event_id WHERE Type = @type AND Title LIKE @search GROUP BY event.Event_id", con);
                selectevents.Parameters.AddWithValue("@type", type);
                selectevents.Parameters.AddWithValue("@search", "%"+search+"%");
                MySqlDataReader reader = selectevents.ExecuteReader();
                while (reader.Read())
                {
                    events.Add(new EventModel
                    {
                        EventId = reader.GetInt32("Event_id"),
                        Organiser = reader["Company_name"].ToString(),
                        Email = reader["Email"].ToString(),
                        Title = reader["Title"].ToString(),
                        Type = reader["Type"].ToString(),
                        Cost = reader.GetDouble("Cost"),
                        Registration_begin = reader.GetDateTime("Registration_begin"),
                        Registration_end = reader.GetDateTime("Registration_end"),
                        IBAN = reader["IBAN"].ToString(),
                        Image_path = reader["Image_path"].ToString(),
                        Description = reader["Description"].ToString(),
                        MaxNumberOfParticipants = reader.GetInt32("MaxNumberOfParticipants"),
                        PayoutSplitPercentageForWinner = reader.GetInt32("PayoutSplitPercentageForWinner"),
                        NumberOfParticipants = reader.GetInt32("NumberOfParticipants"),
                        CompetitionStarted = reader.GetBoolean("CompetitionStarted")
                    });
                }

                if (!reader.HasRows)
                {
                    return BadRequest("No events of such type");
                }
                reader.Close();
            }
            return Ok(events);
        }


        [HttpGet("notifyEventParticipants/{id}")]
        public IActionResult notifyEventParticipants(int id)
        {
            List<EventParticipantsModel> EventParticipants = new List<EventParticipantsModel>();
            String connection = _configuration["DBConnection"];
            using (MySqlConnection con = new MySqlConnection(connection))
            {
                con.Open();

                MySqlCommand selectParticipants = new MySqlCommand("SELECT e.Title, First_name, p.Email, Position, Prize FROM ticket_event JOIN ticket t on ticket_event.Ticket_id = t.Ticket_id " +
                "JOIN participant p on t.Participant_email = p.Email JOIN event e on ticket_event.Event_id = e.Event_id WHERE e.Event_id = @id", con);
                selectParticipants.Parameters.AddWithValue("@id", id);
                MySqlDataReader reader = selectParticipants.ExecuteReader();
                while (reader.Read())
                {
                    EventParticipants.Add(new EventParticipantsModel
                    {
                        Email = reader["Email"].ToString(),
                        Title = reader["Title"].ToString(),
                        Name = reader["First_name"].ToString(),
                        Position = reader.GetInt32("Position"),
                        Prize = reader.GetDouble("Prize")
                    });
                }

                if (!reader.HasRows)
                {
                    return BadRequest("No event or participants in this event.");
                }
                reader.Close();
            }

            EmailEventParticipants(EventParticipants);
            return Ok(EventParticipants);
        }


        [HttpPost("EliminatePlayer")]
        public IActionResult EliminatePlayer(NotifyPlayerModel @player)
        {

            String databaseConnection = _configuration["DBConnection"];
            //Connecting to the database
            using (MySqlConnection mySqlConnection = new MySqlConnection(databaseConnection))
            {
                mySqlConnection.Open();

                MySqlCommand updatePlayerEliminatedCommand = mySqlConnection.CreateCommand();
                MySqlTransaction QueryTransaction;
                QueryTransaction = mySqlConnection.BeginTransaction();
                updatePlayerEliminatedCommand.Connection = mySqlConnection;

                try
                {
                    updatePlayerEliminatedCommand.CommandText = "UPDATE ticket_event SET Eliminated = 1 WHERE event_id = @EventId AND ticket_id = @TicketId; ";
                    updatePlayerEliminatedCommand.Parameters.AddWithValue("@EventId", @player.EventId);
                    updatePlayerEliminatedCommand.Parameters.AddWithValue("@TicketId", @player.TicketId);
                    updatePlayerEliminatedCommand.ExecuteNonQuery();

                    QueryTransaction.Commit();
                    return Ok("Player has been seccesfully eliminated from the event.");  //Status 200

                }
                catch (Exception e)
                {
                    try
                    {
                        QueryTransaction.Rollback();
                    }
                    catch (SqlException ex)
                    {
                        if (QueryTransaction.Connection != null)
                        {
                            Console.WriteLine("An exception of type " + ex.GetType() +
                            " was encountered while attempting to roll back the transaction.");
                        }
                    }

                    Console.WriteLine("An exception of type " + e.GetType() +
                    " was encountered while inserting the data.");
                    Console.WriteLine("Neither record was written to database.");
                    return BadRequest("Bad");    //Status 400
                }
                finally
                {
                    mySqlConnection.Close();
                }
            }
        }

        [HttpPost("SelectWinner")]
        public IActionResult SelectWinner(NotifyPlayerModel @player)
        {

            String databaseConnection = _configuration["DBConnection"];
            //Connecting to the database
            using (MySqlConnection mySqlConnection = new MySqlConnection(databaseConnection))
            {
                mySqlConnection.Open();

                MySqlCommand updatePlayerEliminatedCommand = mySqlConnection.CreateCommand();
                MySqlCommand updateCompetitionStartedCommand = mySqlConnection.CreateCommand();
                MySqlTransaction QueryTransaction;
                QueryTransaction = mySqlConnection.BeginTransaction();
                updatePlayerEliminatedCommand.Connection = mySqlConnection;

                try
                {
                    updatePlayerEliminatedCommand.CommandText = "UPDATE ticket_event SET Position = 1 WHERE event_id = @EventId AND Ticket_id = @TicketId";
                    updatePlayerEliminatedCommand.Parameters.AddWithValue("@EventId", @player.EventId);
                    updatePlayerEliminatedCommand.Parameters.AddWithValue("@TicketId", @player.TicketId);
                    updatePlayerEliminatedCommand.ExecuteNonQuery();

                    updateCompetitionStartedCommand.CommandText = "UPDATE event SET CompetitionCompleted = 1 WHERE event_id = @EventId";
                    updateCompetitionStartedCommand.Parameters.AddWithValue("@EventId", @player.EventId);
                    updateCompetitionStartedCommand.ExecuteNonQuery();

                    QueryTransaction.Commit();
                    return Ok("Player has won the event");  //Status 200

                }
                catch (Exception e)
                {
                    try
                    {
                        QueryTransaction.Rollback();
                    }
                    catch (SqlException ex)
                    {
                        if (QueryTransaction.Connection != null)
                        {
                            Console.WriteLine("An exception of type " + ex.GetType() +
                            " was encountered while attempting to roll back the transaction.");
                        }
                    }

                    Console.WriteLine("An exception of type " + e.GetType() +
                    " was encountered while inserting the data.");
                    Console.WriteLine("Neither record was written to database.");
                    return BadRequest("Bad");    //Status 400
                }
                finally
                {
                    mySqlConnection.Close();
                }
            }
        }



        [HttpGet("geteventpage/{id}")]
        public IActionResult getSpecificEventDisplayPage(int id)
        {
            EventSignUpPageModel details = new EventSignUpPageModel();
            List<String> availableSelectionsList = new List<string>();

            String connection = _configuration["DBConnection"];
            using (MySqlConnection con = new MySqlConnection(connection))
            {

                int numberOfParticipants = 0;
                numberOfParticipants = getNumberOfParticipants(id, con, numberOfParticipants);

                getEventAvailableSelections(id, availableSelectionsList, con);
                con.Open();
                MySqlCommand selectevent = new MySqlCommand("SELECT o.Company_name, Title, Description, Type, Cost, Image_path, MaxNumberOfParticipants, " +
                                                             "Registration_begin, Registration_end, PayoutSplitPercentageForWinner, CompetitionCompleted, CompetitionStarted FROM event JOIN organiser o ON o.Email = event.email " +
                                                             "WHERE Event_id = @id", con);
                selectevent.Parameters.AddWithValue("@id", id);
                MySqlDataReader reader = selectevent.ExecuteReader();
                            
                reader.Read();

                details.OrganiserName = reader["Company_name"].ToString();
                details.Title = reader["Title"].ToString();
                details.Description = reader["Description"].ToString();
                details.Type = reader["Type"].ToString();
                details.Cost = reader.GetDouble("Cost");
                details.Image_path = reader["Image_path"].ToString();
                details.NumberOfParticipants = numberOfParticipants;
                details.MaxNumberOfParticipants = reader.GetInt32("MaxNumberOfParticipants");
                details.Registration_end = reader.GetDateTime("Registration_end");
                details.PayoutSplitPercentageForWinner = reader.GetInt32("PayoutSplitPercentageForWinner");
                details.AvailableSelections = availableSelectionsList;
                details.CompetitionCompleted = reader.GetBoolean("CompetitionCompleted");
                details.CompetitionStarted = reader.GetBoolean("CompetitionStarted");

                if (!reader.HasRows)
                {
                    return BadRequest("No events of such type");
                }
                reader.Close();
            }
            return Ok(details);
        }


        [HttpGet("geteventid/{id}")]
        public IActionResult geteventid(string id)
        {
            int event_id;

            String connection = _configuration["DBConnection"];
            using (MySqlConnection con = new MySqlConnection(connection))
            {

                try
                {
                    con.Open();
                    MySqlCommand selectevent = new MySqlCommand("SELECT event_id FROM event WHERE Invite_id = @Invite_id;", con);
                    selectevent.Parameters.AddWithValue("@Invite_id", id);
                    MySqlDataReader reader = selectevent.ExecuteReader();

                    reader.Read();

                    event_id = reader.GetInt32("event_id");
                    reader.Close();
                    return Ok(event_id);
                }
                catch {
                    return BadRequest("Bad");
                }

            }
            
        }




        [HttpGet("getPlayerSelections/{event_id}")]
        public IActionResult playerSelections(String event_id)
        {
        List<EmailSelectionModel> emailAndSelection = new List<EmailSelectionModel>();
            String connection = _configuration["DBConnection"];
            using (MySqlConnection con = new MySqlConnection(connection))
            {
                con.Open();

                MySqlCommand selectevents = new MySqlCommand("SELECT Participant_email, Selection, t.Ticket_id, Eliminated, Title, Company_name FROM ticket_event " +
                                                             "JOIN ticket t ON t.Ticket_id = ticket_event.Ticket_id " +
                                                             "JOIN event e ON e.Event_id = ticket_event.Event_id " +
                                                             "JOIN organiser o ON o.Email = e.Email WHERE e.Event_id= @event_id", con);
                selectevents.Parameters.AddWithValue("@event_id", event_id);
                MySqlDataReader reader = selectevents.ExecuteReader();
                while (reader.Read())
                {
                    emailAndSelection.Add(new EmailSelectionModel
                    {
                        Email = reader["Participant_email"].ToString(),
                        Selection = reader["Selection"].ToString(),
                        Eliminated = reader.GetBoolean("Eliminated"),
                        Ticket_id = reader.GetInt32("Ticket_id"),
                        EventTitle = reader["Title"].ToString(),
                        EventOrganiser = reader["Company_name"].ToString()
                    });
                }

                if (!reader.HasRows)
                {
                    return BadRequest("No players in this event");
                }
                reader.Close();
            }
            return Ok(emailAndSelection);
        }


        [HttpPost("notifyEliminatedParticipant")]
        public IActionResult notifyEliminatedParticipant(NotifyPlayerModel notify)
        {
            EventParticipantsModel EventParticipant = new EventParticipantsModel();
            String connection = _configuration["DBConnection"];
            using (MySqlConnection con = new MySqlConnection(connection))
            {
                con.Open();

                MySqlCommand selectParticipants = new MySqlCommand("SELECT e.Title, First_name, p.Email, Position, Prize FROM ticket_event JOIN ticket t on ticket_event.Ticket_id = t.Ticket_id " +
                    "JOIN participant p on t.Participant_email = p.Email JOIN event e on ticket_event.Event_id = e.Event_id WHERE p.email = @Email and e.Event_id= @EventId and t.Ticket_id= @TicketId;", con);
                selectParticipants.Parameters.AddWithValue("@Email", notify.Email);
                selectParticipants.Parameters.AddWithValue("@EventId", notify.EventId);
                selectParticipants.Parameters.AddWithValue("@TicketId", notify.TicketId);
                MySqlDataReader reader = selectParticipants.ExecuteReader();
                while (reader.Read())
                {
                    EventParticipant.Email = reader["Email"].ToString();
                    EventParticipant.Title = reader["Title"].ToString();
                    EventParticipant.Name = reader["First_name"].ToString();
                    EventParticipant.Position = reader.GetInt32("Position");
                    EventParticipant.Prize = reader.GetDouble("Prize");
 
                }

                if (!reader.HasRows)
                {
                    return BadRequest("No event or participants in this event.");
                }
                reader.Close();
            }
            SendEmail.eliminatedMessage(EventParticipant.Email, EventParticipant.Name, EventParticipant.Title, EventParticipant.Prize);
            return Ok(EventParticipant);
        }



        [HttpPost("notifyWinningParticipant")]
        public IActionResult notifyWinningParticipant(NotifyPlayerModel notify)
        {
            EventParticipantsModel EventParticipant = new EventParticipantsModel();
            String connection = _configuration["DBConnection"];
            using (MySqlConnection con = new MySqlConnection(connection))
            {
                con.Open();

                MySqlCommand selectParticipants = new MySqlCommand("SELECT e.Title, First_name, p.Email, ticket_event.Position, ((count(te.ticket_id)*e.cost)/100)*e.PayoutSplitPercentageForWinner as Prize FROM ticket_event " + 
                                                                    "JOIN ticket t on ticket_event.Ticket_id = t.Ticket_id JOIN participant p on t.Participant_email = p.Email " +
                                                                    "JOIN event e on ticket_event.Event_id = e.Event_id " +
                                                                    "LEFT JOIN ticket_event te ON e.Event_id = te.Event_id " +
                                                                    "WHERE e.Event_id= @EventId AND p.email = @email AND t.Ticket_id= @TicketId ", con);
                selectParticipants.Parameters.AddWithValue("@email", notify.Email);
                selectParticipants.Parameters.AddWithValue("@EventId", notify.EventId);
                selectParticipants.Parameters.AddWithValue("@TicketId", notify.TicketId);
                MySqlDataReader reader = selectParticipants.ExecuteReader();
                while (reader.Read())
                {
                    EventParticipant.Email = reader["Email"].ToString();
                    EventParticipant.Title = reader["Title"].ToString();
                    EventParticipant.Name = reader["First_name"].ToString();
                    EventParticipant.Position = reader.GetInt32("Position");
                    EventParticipant.Prize = reader.GetDouble("Prize");
                }

                if (!reader.HasRows)
                {
                    return BadRequest("No event or participants in this event.");
                }
                reader.Close();
            }
            
            SendEmail.winnerMessage(EventParticipant.Email, EventParticipant.Name, EventParticipant.Title, EventParticipant.Prize);
            return Ok(EventParticipant);
        }

        [HttpPost("startEvent/{eventId}")]
        public IActionResult startEvent(int eventId)
        {
            
            String connection = _configuration["DBConnection"];
            using (MySqlConnection con = new MySqlConnection(connection))
            {
                con.Open();

                MySqlCommand selectParticipants = new MySqlCommand("UPDATE event SET CompetitionStarted = 1 WHERE event_id = @EventId;", con);
                selectParticipants.Parameters.AddWithValue("@EventId", eventId);
                selectParticipants.ExecuteNonQuery();
            }

            return Ok("Event ID: " + eventId + " has Started!");
        }

        [HttpGet("checkeventstatus/{eventId}")]
        public IActionResult checkeventstatus(int eventId)
        {
            String connection = _configuration["DBConnection"];
            using (MySqlConnection con = new MySqlConnection(connection))
            {
                con.Open();

                MySqlCommand status = new MySqlCommand("SELECT count(Position) FROM ticket_event WHERE event_id = @EventId and Position>0;", con);
                status.Parameters.AddWithValue("@EventId", eventId);
                MySqlDataReader statusreader = status.ExecuteReader();
                statusreader.Read();
                if(statusreader.GetInt32("count(Position)") > 0)
                {
                    return Ok("Has ended");
                }
                statusreader.Close();
            }
            return BadRequest("Not ended");
            
        }

        [HttpPost("cancelevent/{eventId}")]
        public IActionResult cancelevent(string eventId)
        {
            String connection = _configuration["DBConnection"];
            using (MySqlConnection con = new MySqlConnection(connection))
            {
                con.Open();

                MySqlCommand cancel = new MySqlCommand("DELETE FROM event where Event_id = @EventId", con);
                cancel.Parameters.AddWithValue("@EventId", eventId);
                cancel.ExecuteNonQuery();               
            }
            return Ok("Event cancelled!");
        }


        [HttpGet("getPickedSelections/{event_id}")]
        public IActionResult PickedSelections(String event_id)
        {
            List<String> PickedSelections = new List<String>();
            String connection = _configuration["DBConnection"];
            using (MySqlConnection con = new MySqlConnection(connection))
            {
                con.Open();

                MySqlCommand selectevents = new MySqlCommand("SELECT Selection FROM ticket_event WHERE Event_id = @event_id", con);
                selectevents.Parameters.AddWithValue("@event_id", event_id);
                MySqlDataReader reader = selectevents.ExecuteReader();
                while (reader.Read())
                {
                    PickedSelections.Add(reader["Selection"].ToString());
                }

                if (!reader.HasRows)
                {
                    return BadRequest("No players in this event");
                }
                reader.Close();
            }
            return Ok(PickedSelections);
        }

        private static void getEventAvailableSelections(int id, List<string> AvailableSelections, MySqlConnection con)
        {
            con.Open();
            MySqlCommand getSelectionsForEvent = new MySqlCommand("SELECT * FROM event_selections where Event_id = @id;", con);
            getSelectionsForEvent.Parameters.AddWithValue("@id", id);
            MySqlDataReader SelectionsForEventReader = getSelectionsForEvent.ExecuteReader();

            while (SelectionsForEventReader.Read())
            {
                AvailableSelections.Add(SelectionsForEventReader["Selection"].ToString());
            }
            con.Close();
        }

        private static int getNumberOfParticipants(int id, MySqlConnection con, int numberOfParticipants)
        {
            con.Open();
            MySqlCommand getParticipants = new MySqlCommand("SELECT count(Event_id) NumberOfParticipants FROM ticket_event where Event_id = @id", con);
            getParticipants.Parameters.AddWithValue("@id", id);
            MySqlDataReader getParticipantsreader = getParticipants.ExecuteReader();

            while (getParticipantsreader.Read())
            {
                numberOfParticipants = getParticipantsreader.GetInt32("NumberOfParticipants");
            }
            con.Close();
            return numberOfParticipants;
        }

        private static void EmailEventParticipants(List<EventParticipantsModel> EventParticipants)
        {
            foreach (EventParticipantsModel participant in EventParticipants)
            {
                SendEmail.notificationMessage(participant.Email, participant.Name, participant.Title, participant.Position, participant.Prize);
            }
        }


    }
}
