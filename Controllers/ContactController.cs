using MailKit;
using Org.BouncyCastle.Bcpg.OpenPgp;
using ProjectShoeShop.Services.MailService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ProjectShoeShop.Controllers
{
    public class ContactController : Controller
    {
        private IEmailService emailService;
        private string ToEmail;
        // GET: Contact
        public ActionResult ContactUs()
        {
            return View();
        }
        [HttpPost]
        public ActionResult SendEmail(FormCollection f)
        {
            string subject = "Exclusive Offer: Unleash Your Style with Our Latest Shoe Collection!";
            string message = "Dear [Customer's " + f["Name"].ToString() + "], We hope this message finds you well. At our Shoe Shop, we are excited to introduce our newest shoe collection that is sure to elevate your style and keep you stepping confidently through every occasion.";

            emailService = new EmailService();
            ToEmail = f["Email"].ToString();
            if (f["Name"] != null && f["Email"] != null)
            {
                MailRequest mailRequest = new MailRequest();
                mailRequest.Subject = subject;
                mailRequest.Body = message;
                mailRequest.ToEmail = ToEmail;
                emailService.SendEmail(mailRequest);
            }
            return RedirectToAction("ContactUs", "Contact");
        }
    }
}