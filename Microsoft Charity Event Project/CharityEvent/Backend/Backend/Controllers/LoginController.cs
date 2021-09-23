using Backend.Helpers;
using Backend.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using MySql.Data.MySqlClient; // Install MySql.Data Package
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Helpers;
using System.Data.SqlClient;

namespace Backend.Controllers
{
    [ApiController]
    [Route("api")]
    public class LoginController : ControllerBase
    {
        private readonly JwtService _jwtservice;
        private readonly IConfiguration _configuration;
        public LoginController(JwtService jwtservice, IConfiguration configuration)
        {
            _jwtservice = jwtservice;
            _configuration = configuration;
        }
        //Getting a POST request on /login
        [HttpPost("login")]
        public IActionResult Post(LoginModel user)
        {
            String databaseConnection = _configuration["DBConnection"];
            //Creating a database connection. Make sure you enter the right port and password of the database server.
            using (MySqlConnection mySqlConnection = new MySqlConnection(databaseConnection))
            {
                string TwoFA = generate2FA();
                mySqlConnection.Open();
                //Retrieving Email and Password from database.
                MySqlCommand selectAuthenticationCommand = new MySqlCommand("select Email, Password, Salt from authentication where Email=@Email;", mySqlConnection);
                selectAuthenticationCommand.Parameters.AddWithValue("@Email", user.Email);
                MySqlDataReader reader = selectAuthenticationCommand.ExecuteReader();
                //To update to login date&time
                MySqlCommand updateAuthenticationCommand = new MySqlCommand("update authentication set Login_date=@date where Email=@Email;", mySqlConnection);
                DateTime date = DateTime.Now;
                updateAuthenticationCommand.Parameters.AddWithValue("@date", date);
                updateAuthenticationCommand.Parameters.AddWithValue("@Email", user.Email);

                MySqlCommand insert2FACode = new MySqlCommand("update authentication set 2FA_code=@2FA where Email=@Email;", mySqlConnection);
                insert2FACode.Parameters.AddWithValue("@2FA", TwoFA);
                insert2FACode.Parameters.AddWithValue("@Email", user.Email);

               


                LoginModel checkUser = new LoginModel();
                if (!reader.HasRows)
                {
                    return BadRequest("Wrong id or password");
                }
                reader.Read();
                checkUser.Email = reader["Email"].ToString();
                if (user.Email.Equals(checkUser.Email))
                {
                    bool passwordVerification = Hash.VerifyPassword(user.Password, reader);
                    reader.Close();
                    if (passwordVerification)
                    {
                        updateAuthenticationCommand.ExecuteNonQuery();
                        insert2FACode.ExecuteNonQuery();
                        SendEmail.twoFactorAuthenticationMessage(user.Email, TwoFA);

                        return Ok(new
                        {
                            message = "Successful"
                        });//Status 200                        
                    }
                }
                return BadRequest("Wrong id or password");    //Status 400
            }
        }


        [HttpGet("user/{type}")]
        public IActionResult User(String type)
        {
            String connection = _configuration["DBConnection"];
            try
            {
                var token = Request.Cookies["token"];

                var verifiedtoken = _jwtservice.validate(token);

                var email = verifiedtoken.Issuer;

                using (MySqlConnection mySqlConnection = new MySqlConnection(connection))
                {
                    mySqlConnection.Open();

                    if (type.Equals("Participant"))
                    {
                        ParticipantModel user = new ParticipantModel();
                        user = SelectParticipant(email, mySqlConnection);
                        return Ok(user);
                    }
                    else if (type.Equals("Organiser"))
                    {
                        OrganiserModel user = new OrganiserModel();
                        user = SelectOrganiser(email, mySqlConnection);
                        return Ok(user);
                    }
                    else { return Unauthorized("Invalid user type"); }

                }
            }
            catch (Exception e)
            {
                return Unauthorized();
            }
        }

        private static ParticipantModel SelectParticipant(string email, MySqlConnection mySqlConnection)
        {
            MySqlCommand selectParticipantCommand = new MySqlCommand("select * from Participant where Email=@Email;", mySqlConnection);
            selectParticipantCommand.Parameters.AddWithValue("@Email", email);
            MySqlDataReader reader = selectParticipantCommand.ExecuteReader();
            reader.Read();
            ParticipantModel user = new ParticipantModel();
            user.Email = reader["Email"].ToString();
            user.FirstName = reader["First_name"].ToString();
            user.LastName = reader["Last_name"].ToString();
            user.PhoneNumber = reader["Phone_number"].ToString();
            user.DOB = reader.GetDateTime("DOB");
            user.Address = reader["Address"].ToString();
            user.Zip = reader["Zip"].ToString();
            user.Verified = reader.GetBoolean("Verified");
            return user;
        }
        private static OrganiserModel SelectOrganiser(string email, MySqlConnection mySqlConnection)
        {
            MySqlCommand selectParticipantCommand = new MySqlCommand("select * from Organiser where Email=@Email;", mySqlConnection);
            selectParticipantCommand.Parameters.AddWithValue("@Email", email);
            MySqlDataReader reader = selectParticipantCommand.ExecuteReader();
            reader.Read();
            OrganiserModel organiser = new OrganiserModel();
            organiser.Email = reader["Email"].ToString();
            organiser.CompanyName = reader["Company_name"].ToString();
            organiser.Description = reader["Company_description"].ToString();
            organiser.PhoneNumber = reader["Phone_number"].ToString();
            organiser.Address = reader["Address"].ToString();
            organiser.Zip = reader["Zip"].ToString();
            organiser.Verified = reader.GetBoolean("Verified");
            return organiser;
        }

        [HttpPost("logout")]
        public IActionResult logout()
        {
            try
            {
                Response.Cookies.Delete("token");
                return Ok("Logged out");
            }
            catch (Exception e)
            {
                return BadRequest();
            }
        }

        [HttpPost("verify2FA")]
        public IActionResult verify2FA(VerificationModel verification)
        {
            String databaseConnection = _configuration["DBConnection"];

            using (MySqlConnection mySqlConnection = new MySqlConnection(databaseConnection))
            {
                mySqlConnection.Open();
                //Retrieving Email and Password from database.
                MySqlCommand selectAuthenticationCommand = new MySqlCommand("SELECT 2FA_code FROM authentication WHERE email = @Email;", mySqlConnection);
                selectAuthenticationCommand.Parameters.AddWithValue("@Email", verification.Email);
                MySqlDataReader reader = selectAuthenticationCommand.ExecuteReader();

                reader.Read();
                string twoFA = reader["2FA_code"].ToString();
                reader.Close();
                if (twoFA == verification.VerificationCode)
                {
                    var token = _jwtservice.generate(verification.Email);
                    Response.Cookies.Append("token", token, new CookieOptions
                    {                     
                        HttpOnly = true
                    });
                    return Ok(new
                    {
                        message = "Successful"
                    });//Status 200     
                }

            }
            return BadRequest("2FA do NOT match!");    //Status 400

        }


        [HttpPost("sendResetPasswordLink/{email}")]
        public IActionResult sendResetPasswordLink(String Email)
        {
            
            String connection = _configuration["DBConnection"];
            using (MySqlConnection con = new MySqlConnection(connection))
            {
                string verificationCode;
                verificationCode = Hash.generateVerificationCode();
                string resetPasswordLink = "http://20.101.8.254/ResetPassword?email=" + Email + "&verificationCode=" + verificationCode;  //Remove th comments when deploying and vice versa.
                //string resetPasswordLink = "http://localhost:3000/ResetPassword?email=" + Email + "&verificationCode=" + verificationCode;  //Comment this when deploying and vice versa.
                con.Open();
                MySqlCommand selectevents = new MySqlCommand("UPDATE authentication SET Reset_Password_Identifier = @Reset_Password_Identifier WHERE email = @email", con);
                selectevents.Parameters.AddWithValue("@email", Email);
                selectevents.Parameters.AddWithValue("@Reset_Password_Identifier", verificationCode);
                selectevents.ExecuteNonQuery();


                SendEmail.resetPasswordMessage(Email, resetPasswordLink);
            }

            return Ok("ResetPasswordEmailSent");
        }



        [HttpPost("resetPassword")]
        public IActionResult ResetPass(AuthenticationModel user)
        {
            String databaseConnection = _configuration["DBConnection"];

            using (MySqlConnection mySqlConnection = new MySqlConnection(databaseConnection))
            {

                string salt, hashedPassword;
                Hash.HashPassword(user.NewPassword, out salt, out hashedPassword);

                mySqlConnection.Open();
                //Retrieving Email and Password from database.
                MySqlCommand selectAuthenticationCommand = new MySqlCommand("select Email, Password, Salt from authentication where Email=@Email;", mySqlConnection);
                selectAuthenticationCommand.Parameters.AddWithValue("@Email", user.Email);
                MySqlDataReader reader = selectAuthenticationCommand.ExecuteReader();

                reader.Read();
                bool passwordVerification = Hash.VerifyPassword(user.Password, reader);
                reader.Close();
                if (passwordVerification)
                {
                    updatePassword(user, salt, hashedPassword, selectAuthenticationCommand);
                    return Ok("Successfully changed password");
                }

            }
            return BadRequest("Inncorrect password");    //Status 400

        }


        [HttpPost("resetForgotPassword")]
        public IActionResult resetForgotPassword(AuthenticationModel user)
        {
            String databaseConnection = _configuration["DBConnection"];

            using (MySqlConnection mySqlConnection = new MySqlConnection(databaseConnection))
            {

                string salt, hashedPassword;
                Hash.HashPassword(user.NewPassword, out salt, out hashedPassword);

                mySqlConnection.Open();
                //Retrieving Email and Password from database.
                MySqlCommand selectAuthenticationCommand = new MySqlCommand("SELECT Reset_Password_Identifier FROM authentication WHERE email = @Email", mySqlConnection);
                selectAuthenticationCommand.Parameters.AddWithValue("@Email", user.Email);
                MySqlDataReader reader = selectAuthenticationCommand.ExecuteReader();

                reader.Read();
                
                string resetPasswordCode = reader["Reset_Password_Identifier"].ToString(); //read from database


                reader.Close();
                if (user.VerificationCode == resetPasswordCode)
                {
                    updatePassword(user, salt, hashedPassword, selectAuthenticationCommand);
                    return Ok("Successfully changed password");
                }

            }
            return BadRequest("Inncorrect verification Code");    //Status 400

        }


        [HttpPost("enable2FA/{email}")]
        public IActionResult enable2FA(String email)
        {
            String connection = _configuration["DBConnection"];
            using (MySqlConnection con = new MySqlConnection(connection))
            {

                con.Open();
                MySqlCommand selectevents = new MySqlCommand("UPDATE authentication SET TFA_enabled = 1 WHERE email = @email", con);
                selectevents.Parameters.AddWithValue("@email", email);
                selectevents.ExecuteNonQuery();
            }

            return Ok("2FA enabled");
        }

        [HttpPost("disable2FA/{email}")]
        public IActionResult diable2FA(String email)
        {
            String connection = _configuration["DBConnection"];
            using (MySqlConnection con = new MySqlConnection(connection))
            {

                con.Open();
                MySqlCommand selectevents = new MySqlCommand("UPDATE authentication SET TFA_enabled = 0 WHERE email = @email", con);
                selectevents.Parameters.AddWithValue("@email", email);
                selectevents.ExecuteNonQuery();
            }

            return Ok("2FA disabled");
        }


        [HttpGet("getNumberOfUserAccounts/{email}")]
        public IActionResult numberOfAccountsRegistered(String email)
        {
            int numAccounts;
            String connection = _configuration["DBConnection"];
            using (MySqlConnection con = new MySqlConnection(connection))
            {

                con.Open();

                MySqlCommand selectevents = new MySqlCommand("SELECT count(p.email) as numAccounts FROM participant p JOIN organiser o ON o.Email = p.Email  WHERE p.email = @email", con);
                selectevents.Parameters.AddWithValue("@email", email);
                MySqlDataReader reader = selectevents.ExecuteReader();
                reader.Read();

                numAccounts = reader.GetInt32("numAccounts");
                reader.Close();
            }

            return Ok(numAccounts);
        }

        private static void updatePassword(ILogin user, string salt, string hashedPassword, MySqlCommand insertIntoAuthenticationCommand)
        {
            
            insertIntoAuthenticationCommand.CommandText = "UPDATE authentication SET Password = @Password, Salt = @Salt WHERE email = @Email";
            insertIntoAuthenticationCommand.Parameters.AddWithValue("@Password", hashedPassword);
            insertIntoAuthenticationCommand.Parameters.AddWithValue("@Salt", salt);
            insertIntoAuthenticationCommand.ExecuteNonQuery();
        }

        private static string generate2FA() {
            Random r = new Random();
            var x = r.Next(0, 1000000);
            string s = x.ToString("000000");
            return s;
        }



    }
}


