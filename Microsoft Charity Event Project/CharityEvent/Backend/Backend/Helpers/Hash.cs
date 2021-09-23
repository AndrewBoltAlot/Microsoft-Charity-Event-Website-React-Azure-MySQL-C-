using Backend.Models;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Helpers;

namespace Backend.Helpers
{
    public static class Hash
    {

        public static bool VerifyPassword(string password, MySqlDataReader reader)
        {
            string salt = reader["Salt"].ToString(); //read from database
            string HashedPass = reader["Password"].ToString(); //read from database
            string PlainPass = password;
            PlainPass += salt; // append salt key
            bool result = Crypto.VerifyHashedPassword(HashedPass, PlainPass); //verify password                        
            return result;
        }

        public static void HashPassword(string password, out string salt, out string hashedPassword)
        {
            salt = Crypto.GenerateSalt();
            password = password + salt;
            hashedPassword = Crypto.HashPassword(password);
        }

        public static string generateVerificationCode()
        {
            var chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
            var stringChars = new char[30];
            var random = new Random();

            for (int i = 0; i < stringChars.Length; i++)
            {
                stringChars[i] = chars[random.Next(chars.Length)];
            }

            var finalString = new String(stringChars);

            return finalString;
        }
    }
}
