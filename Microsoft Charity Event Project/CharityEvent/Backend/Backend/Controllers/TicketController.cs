using Backend.Models;
using Microsoft.AspNetCore.Mvc;
using MySql.Data.MySqlClient;
using Microsoft.Extensions.Configuration;
using QRCoder;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;
using Backend.Helpers;

namespace Backend.Controllers
{
    [ApiController]
    [Route("api")]
    public class TicketController : Controller
    {
        private readonly IConfiguration _configuration;
        public TicketController(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        [HttpPost("TicketGenerate")]
        public IActionResult Post(TicketModel ticket)
        {
            String databaseConnection = _configuration["DBConnection"];
            //Connecting to the database
            using (MySqlConnection mySqlConnection = new MySqlConnection(databaseConnection))
            {
                mySqlConnection.Open();

                MySqlCommand insertIntoTicketCommand = mySqlConnection.CreateCommand();
                MySqlCommand insertIntoTicketEventCommand = mySqlConnection.CreateCommand();
                MySqlTransaction queryTransaction;
                queryTransaction = mySqlConnection.BeginTransaction();

                insertIntoTicketCommand.Connection = mySqlConnection;
                insertIntoTicketCommand.Transaction = queryTransaction;

                try
                {
                    //Inserting Participant_email and Price into the Ticket table
                    insertIntoTicketCommand.CommandText = "insert into Ticket(Participant_email,Price)values(@Participant_email,@Price)";
                    insertIntoTicketCommand.Parameters.AddWithValue("@Participant_email", ticket.Email);
                    insertIntoTicketCommand.Parameters.AddWithValue("@Price", ticket.Price);
                  
                    insertIntoTicketCommand.ExecuteNonQuery();
                    
                    long ticket_id = insertIntoTicketCommand.LastInsertedId;
                    //Inserting Oraginser details into organiser table.
                    insertIntoTicketEventCommand.CommandText = "insert into Ticket_event(Event_id,Ticket_id,Selection)values(@Event_id,@Ticket_id,@Selection)";
                    insertIntoTicketEventCommand.Parameters.AddWithValue("@Event_id", ticket.EventId);
                    insertIntoTicketEventCommand.Parameters.AddWithValue("@Ticket_id", ticket_id);
                    insertIntoTicketEventCommand.Parameters.AddWithValue("@Selection", ticket.Selection);

                    insertIntoTicketEventCommand.ExecuteNonQuery();

                    queryTransaction.Commit();
                    
                    SendEmail.enteredCompetitionMessage(ticket.Email, ticket.participantName, ticket.EventTitle, ticket.Price);

                    return Ok("Successful");  //Status 200

                }
                catch (Exception e)
                {
                    try
                    {
                        queryTransaction.Rollback();
                    }
                    catch (SqlException ex)
                    {
                        if (queryTransaction.Connection != null)
                        {
                            Console.WriteLine("An exception of type " + ex.GetType() +
                            " was encountered while attempting to roll back the transaction.");
                        }
                    }

                    Console.WriteLine("An exception of type " + e.GetType() +
                    " was encountered while inserting the data.");
                    Console.WriteLine("Neither record was written to database.");
                    return BadRequest("TESTING");    //Status 400
                }
                finally
                {
                    mySqlConnection.Close();
                }
            }
        }
    }
}
