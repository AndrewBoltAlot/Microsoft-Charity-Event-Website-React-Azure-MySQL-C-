using Backend.Helpers;
using Backend.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Helpers;

namespace Backend.Controllers
{
    [ApiController]
    [Route("api")]
    public class ParticipantController : Controller
    {
        private readonly IConfiguration _configuration;
        public ParticipantController(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        [HttpPost("ParticipantSignup")]
        public IActionResult Post(ParticipantModel user)
        {
            String databaseConnection = _configuration["DBConnection"];
            //Connecting to the database
            using (MySqlConnection mySqlConnection = new MySqlConnection(databaseConnection))
            {
                string salt, hashedPassword, verificationCode;
                Hash.HashPassword(user.Password, out salt, out hashedPassword);
                verificationCode = Crypto.GenerateSalt();
                mySqlConnection.Open();
                MySqlCommand insertIntoAuthenticationCommand = mySqlConnection.CreateCommand();
                MySqlCommand insertIntoParticipantCommand = mySqlConnection.CreateCommand();
                MySqlTransaction queryTransaction;
                queryTransaction = mySqlConnection.BeginTransaction();
                insertIntoAuthenticationCommand.Connection = mySqlConnection;
                insertIntoAuthenticationCommand.Transaction = queryTransaction;

                try
                {
                    insertIntoAuthenticationCommand.CommandText = "insert into authentication(Email,Password,Salt)values(@Email,@Password,@Salt)";
                    insertIntoAuthenticationCommand.Parameters.AddWithValue("@Email", user.Email);
                    insertIntoAuthenticationCommand.Parameters.AddWithValue("@Password", hashedPassword);
                    insertIntoAuthenticationCommand.Parameters.AddWithValue("@Salt", salt);
                    insertIntoAuthenticationCommand.ExecuteNonQuery();

                    //Inserting Oraginser details into organiser table.
                    long accountId = insertIntoAuthenticationCommand.LastInsertedId;
                    insertIntoParticipantCommand.CommandText = "insert into participant(Email,First_name,Last_name,Phone_number,DOB,Address,Zip,Created_date,VerificationCode,AccountID)values" +
                    "(@Email,@FirstName,@LastName,@PhoneNumber,@DOB,@Address,@Zip,@CreatedAt,@VerificationCode,@AccountID)";
                    insertIntoParticipantCommand.Parameters.AddWithValue("@Email", user.Email);
                    insertIntoParticipantCommand.Parameters.AddWithValue("@FirstName", user.FirstName);
                    insertIntoParticipantCommand.Parameters.AddWithValue("@LastName", user.LastName);
                    insertIntoParticipantCommand.Parameters.AddWithValue("@PhoneNumber", user.PhoneNumber);
                    insertIntoParticipantCommand.Parameters.AddWithValue("@DOB", user.DOB.Date);
                    insertIntoParticipantCommand.Parameters.AddWithValue("@Address", user.Address);
                    insertIntoParticipantCommand.Parameters.AddWithValue("@Zip", user.Zip);
                    insertIntoParticipantCommand.Parameters.AddWithValue("@VerificationCode", verificationCode);
                    DateTime CreatedAt = DateTime.Now;
                    insertIntoParticipantCommand.Parameters.AddWithValue("@CreatedAt", CreatedAt);
                    insertIntoParticipantCommand.Parameters.AddWithValue("@AccountID", accountId);
                    insertIntoParticipantCommand.ExecuteNonQuery();
                    queryTransaction.Commit();
                    SendEmail.verifyEmailMessage(user.Email, verificationCode, "Participant");
                    return Ok("Succesfully Inserted");  //Status 200

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
                    return BadRequest("error 404");    //Status 400
                }
                finally
                {
                    mySqlConnection.Close();
                }

            }
        }


        [HttpGet("ParticipantEvents/{email}")]
        public IActionResult getParticipantEventList(String email)
        {
            List<EventModel> events = new List<EventModel>();
            String connection = _configuration["DBConnection"];
            using (MySqlConnection con = new MySqlConnection(connection))
            {
                con.Open();
                MySqlCommand selectevents = new MySqlCommand("SELECT DISTINCT e.Event_id, Email, Title, Type, Cost, Registration_begin, Registration_end, " +
                                                             "Image_path, Description, NumberOfParticipants, MaxNumberOfParticipants, PayoutSplitPercentageForWinner, " +
                                                              "CompetitionStarted, e.Invite_id, e.CompetitionCompleted FROM event e JOIN ticket_event te ON te.Event_id = e.Event_id " +
                                                              "JOIN ticket t ON t.Ticket_id = te.Ticket_id WHERE t.Participant_email = @email", con);

                selectevents.Parameters.AddWithValue("@email", email);
                MySqlDataReader reader = selectevents.ExecuteReader();
                while (reader.Read())
                {
                    events.Add(new EventModel
                    {
                        EventId = reader.GetInt32("Event_id"),
                        Email = reader["Email"].ToString(),
                        Title = reader["Title"].ToString(),
                        Type = reader["Type"].ToString(),
                        Cost = reader.GetDouble("Cost"),
                        Registration_begin = reader.GetDateTime("Registration_begin"),
                        Registration_end = reader.GetDateTime("Registration_end"),
                        Image_path = reader["Image_path"].ToString(),
                        Description = reader["Description"].ToString(),
                        NumberOfParticipants = reader.GetInt32("NumberOfParticipants"),
                        MaxNumberOfParticipants = reader.GetInt32("MaxNumberOfParticipants"),
                        PayoutSplitPercentageForWinner = reader.GetInt32("PayoutSplitPercentageForWinner"),
                        CompetitionStarted = reader.GetBoolean("CompetitionStarted"),
                        Invite_id = reader["Invite_id"].ToString(),
                        CompetitionCompleted = reader.GetBoolean("CompetitionCompleted")
                    });
                }

                if (!reader.HasRows)
                {
                    return BadRequest("Participant has NOT entered any events");
                }
                reader.Close();
            }
            return Ok(events);
        }

        [HttpGet("ParticipantsDetails/{email}")]
        public IActionResult getOrganisersInfo(String email)
        {
            ParticipantModel participant = new ParticipantModel();
            String connection = _configuration["DBConnection"];
            using (MySqlConnection con = new MySqlConnection(connection))
            {
                con.Open();

                MySqlCommand selectevents = new MySqlCommand("SELECT * FROM participant WHERE Email = @email", con);
                selectevents.Parameters.AddWithValue("@email", email);
                MySqlDataReader reader = selectevents.ExecuteReader();
                reader.Read();

                participant.Email = reader["Email"].ToString();
                participant.FirstName = reader["First_name"].ToString();
                participant.LastName = reader["Last_name"].ToString();
                participant.PhoneNumber = reader["Phone_number"].ToString();
                participant.DOB = reader.GetDateTime("DOB");
                participant.Address = reader["Address"].ToString();
                participant.Zip = reader["Zip"].ToString();
                participant.Verified = reader.GetBoolean("Verified");            
                reader.Close();
            }
            return Ok(participant);
        }

  
        [HttpPost("PverifyEmail")]
        public IActionResult verifyEmail(VerificationModel verification)
        {
            String databaseConnection = _configuration["DBConnection"];

            using (MySqlConnection mySqlConnection = new MySqlConnection(databaseConnection))
            {
                mySqlConnection.Open();
                //Retrieving Email and Password from database.
                MySqlCommand selectAuthenticationCommand = new MySqlCommand("SELECT VerificationCode FROM participant WHERE email = @Email;", mySqlConnection);
                selectAuthenticationCommand.Parameters.AddWithValue("@Email", verification.Email);
                MySqlCommand verifyEmail = new MySqlCommand("UPDATE participant SET verified = 1 WHERE email = @email", mySqlConnection);
                verifyEmail.Parameters.AddWithValue("@email", verification.Email);
                MySqlDataReader reader = selectAuthenticationCommand.ExecuteReader();

                reader.Read();
                string vcode = reader["VerificationCode"].ToString();
                reader.Close();
                if (vcode == verification.VerificationCode)
                {
                    verifyEmail.ExecuteNonQuery();
                    return Ok("vcode match!");
                }

            }
            return BadRequest("NO MATCH!");    //Status 400

        }

        [HttpGet("PsendEmailVerification/{email}")]
        public IActionResult sendEmailVerification(String email)
        {
            string verificationCode;
            String connection = _configuration["DBConnection"];
            using (MySqlConnection con = new MySqlConnection(connection))
            {

                con.Open();

                MySqlCommand selectevents = new MySqlCommand("SELECT verificationCode FROM participant WHERE email = @email", con);
                selectevents.Parameters.AddWithValue("@email", email);
                MySqlDataReader reader = selectevents.ExecuteReader();
                reader.Read();

                verificationCode = reader["verificationCode"].ToString();
                reader.Close();
            }
            SendEmail.verifyEmailMessage(email, verificationCode, "Participant");
            return Ok("verification Code sent");
        }

    }
}


