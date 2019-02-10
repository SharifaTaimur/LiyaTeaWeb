using reCAPTCHA.MVC;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;
using System.Web.Mvc;
using Recaptcha.Web.Mvc;
using Recaptcha.Web;
using System;

namespace LiyaTea.UI.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }


        #region ContactUs
        [CaptchaValidator(
        PrivateKey = "6LfoWpAUAAAAADLUrBq3r6z9l5wsUzPcSCMBoH_p",
        //ErrorMessage = "Invalid input captcha.",
        RequiredMessage = "The captcha field is required.")]
        public async Task<ActionResult> Email(FormCollection form, bool captchaValid)
        {
            if (ModelState.IsValid && captchaValid)
           {
                var name = form["name"];
                var email = form["email"];
                var messages = form["message"];
                var x = await SendEmail(name, email, messages);
                if (x == "sent")
                    ModelState.AddModelError("Success", "Message sent successfully. We will get back to you shortly!");
                return View("_contact");
            }
            return View("_contact");
        }

        public ViewResult Contact()
        {
            throw new NotImplementedException();
        }

        public async Task<string> SendEmail(string name, string email, string messages)
        {
            var message = new MailMessage(); 
            message.To.Add(new MailAddress("abc@hotmail.com")); // replace with receiver's email id   
            message.From = new MailAddress(email); // replace with sender's email id   


            message.Subject = "Message From" + email;
            message.Body = "Name: " + name + "\n\n From: " + email + "\n\n" + messages;
            message.IsBodyHtml = true;
            using (var smtp = new SmtpClient())
            {
                var credential = new NetworkCredential
                {
                    UserName = "xyz@hotmail.com", // replace with sender's email id   
                    Password = "*****" // replace with password   
                };
                smtp.Credentials = credential;
                smtp.Host = "smtp-mail.outlook.com";
                smtp.Port = 587;
                smtp.EnableSsl = true;
                await smtp.SendMailAsync(message);
                return "sent";

            }
        }
        #endregion



    }
}
