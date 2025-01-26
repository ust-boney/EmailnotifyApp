using System.Net;
using System.Net.Mail;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;

namespace USTTraining.EmailFunctionApp
{
    public class HttpTriggerEmail
    {
        private readonly ILogger<HttpTriggerEmail> _logger;

        public HttpTriggerEmail(ILogger<HttpTriggerEmail> logger)
        {
            _logger = logger;
        }

        [Function("HttpTriggerEmail")]
        public bool Run([HttpTrigger(AuthorizationLevel.Anonymous, "get", "post")] HttpRequest req)
        {
            string name = req.Query["name"].ToString();
            string emailAddress = req.Query["email"].ToString();
            string message= req.Query["message"].ToString();
            
            bool result= SendMail(name, emailAddress, message);

            return result;
            //return new OkObjectResult("Email sent successfully");
           
            //return new OkObjectResult("Unable to send email");
        }

        public bool SendMail(string name, string email, string message){

        MailMessage mailMessage = new MailMessage();
        mailMessage.From = new MailAddress("boney0084@gmail.com");
        mailMessage.To.Add(email);
        mailMessage.Subject = "Received your request";
        mailMessage.Body = $"Hi {name}, {message}";

        SmtpClient smtpClient = new SmtpClient();
        smtpClient.Host = "smtp.gmail.com";
        smtpClient.Port = 587;
            
        smtpClient.UseDefaultCredentials = false;
        smtpClient.Credentials = new NetworkCredential("boney0084@gmail.com", "zpra wdxf izvs clkv");
        smtpClient.EnableSsl = true;
        
        try
        {
            smtpClient.Send(mailMessage);
            _logger.LogInformation("Email sent successfully!");
            return true;
        }
        catch(Exception ex)
        {
            _logger.LogInformation(ex.Message);
            return false;
        }
       
        }
    }
}
