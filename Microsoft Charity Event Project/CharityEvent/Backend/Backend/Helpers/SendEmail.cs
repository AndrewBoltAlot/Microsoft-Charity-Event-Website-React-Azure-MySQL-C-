using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;

namespace Backend.Helpers
{
    public static class SendEmail
    {
        //string senderEmail, password;


        private static void sendMail(string messageSubject, string messageBody, string recipientEmail) {
            string senderEmail = "thunderbirds.ucd@gmail.com";
            string password = "thunderbirds!";
            MailMessage message = new MailMessage();
            message.To.Add(recipientEmail);
            message.From = new MailAddress(senderEmail);
            message.Body = messageBody;
            message.Subject = messageSubject;
            SmtpClient smtp = new SmtpClient("smtp.gmail.com");
            smtp.EnableSsl = true;
            smtp.Port = 587;
            smtp.DeliveryMethod = SmtpDeliveryMethod.Network;
            smtp.Credentials = new NetworkCredential(senderEmail, password);

            try
            {
                smtp.Send(message);
                Console.Write("Email sent successfully.");

            }
            catch (Exception ex)
            {
                Console.Write("Email failed.");
            }
        }
        public static void verifyEmailMessage(string recipientEmail, string verificationCode, string accountType)
        {
            string messageSubject = "Your CharityEvent Account";
            string messageBody = $"Hey! \n \n Your {accountType} email is still not verified. \n \n To proceed please enter the verification code in the profile section:  {verificationCode} " +
                                 $"\n \n Please note this is an automated email.";
            sendMail(messageSubject, messageBody, recipientEmail);
        }

        public static void twoFactorAuthenticationMessage(string recipientEmail, string verificationCode)
        {
            string messageSubject = "Your CharityEvent Account";
            string messageBody = $"{verificationCode} is your Charity event verification code.";
            sendMail(messageSubject, messageBody, recipientEmail);
        }

        public static void notificationMessage(string recipientEmail, string name, string title, int position, double prize)
        {
            string messageSubject = "Your CharityEvent Account";
            string messageBody = $"Hey {name} \n \n Congratulations you finished in {position} position in {title} event. \n \n Total winnings : € {prize}";
            sendMail(messageSubject, messageBody, recipientEmail);
        }

        public static void eliminatedMessage(string recipientEmail, string name, string title, double prize)
        {
            string messageSubject = $"Eliminated from {title}";
            string messageBody = $"Hey {name} \n \n You have been eliminated from {title}. \n \n Total winnings : € {prize}";
            sendMail(messageSubject, messageBody, recipientEmail);
        }

        public static void winnerMessage(string recipientEmail, string name, string title, double prize)
        {
            string messageSubject = "Winner";
            string messageBody = $"Hey {name} \n \n Congratulations you are the winner of {title} event. \n \n Total winnings : € {prize}";
            sendMail(messageSubject, messageBody, recipientEmail);
        }

        public static void enteredCompetitionMessage(String recipientEmail, string name, string title, double cost)
        {
            string messageSubject = $"Successfully Joined {title}";
            string messageBody = $"Hey {name} \n \n You have successfully joined {title} event. \n \n Total cost : € {cost}";
            sendMail(messageSubject, messageBody, recipientEmail);
        }

        public static void resetPasswordMessage(String recipientEmail, string ResetPasswordLink)
        {
            string messageSubject = $"Reset Password";
            string messageBody = $"Forgot your password? No problem - just click the link below to create a new one. \n \n {ResetPasswordLink}";
            sendMail(messageSubject, messageBody, recipientEmail);
        }

    }
}
