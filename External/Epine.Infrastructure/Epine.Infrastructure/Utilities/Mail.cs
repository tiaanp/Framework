
using System;
using System.IO;
using Epine.Infrastructure.Extensions;
using Epine.Infrastructure.Properties;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;

namespace Epine.Infrastructure.Utilities {

	/// <summary>
	/// 
	/// </summary>
	public static class Mail {

		/// <summary>
		/// 
		/// </summary>
		/// <param name="message"></param>
		/// <returns></returns>
		public static async Task Send(MailMessage message) {

			message.Sender = new MailAddress(Settings.Default.FromEmail);
			message.From = new MailAddress(Settings.Default.FromEmail);



			using (var smtp = new SmtpClient(Settings.Default.SMTPHost, 25)) {
				var credential = 
					new NetworkCredential(
						Settings.Default.SMTPUserName, 
						Settings.Default.SMTPPassword);

				
				smtp.DeliveryMethod = SmtpDeliveryMethod.Network;
				smtp.UseDefaultCredentials = false;
				smtp.EnableSsl = false;				
				smtp.Timeout = 3000000;
				smtp.Credentials = credential;

				await smtp.SendMailAsync(message).ConfigureAwait(false);
			}
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="to"></param>
		/// <param name="subject"></param>
		/// <param name="body"></param>
		/// <returns></returns>
		public static async Task Send(string to, string subject, string body)
		{

		    if (to.IsNullOrEmpty())
		        return;
		        


            string[] emails = to.Split(';');

			using (var mail = new MailMessage()) {
				foreach (string email in emails) {
                    if (email.IsNotNullOrEmpty())
                    {
                        mail.To.Add(email);
                    }
				}

				mail.Subject = subject;
				mail.IsBodyHtml = true;
				mail.Body = body;
				mail.Priority = MailPriority.Normal;

				await Mail.Send(mail).ConfigureAwait(false);
                ;
            }
        }

	    public static async Task Send(string to, string subject, string body,string attachmentName, Stream attachment)
	    {

	        if (to.IsNullOrEmpty())
	            return;



	        string[] emails = to.Split(';');

	        using (var mail = new MailMessage())
	        {
	            foreach (string email in emails)
	            {
	                if (email.IsNotNullOrEmpty())
	                {
	                    mail.To.Add(email);
	                }
	            }
	            attachment.TryResetStream();

                mail.Subject = subject;
	            mail.IsBodyHtml = true;
	            mail.Body = body;
	            mail.Priority = MailPriority.Normal;
                mail.Attachments.Add(new Attachment(attachment, attachmentName));

	            await Mail.Send(mail).ConfigureAwait(false);
	            ;
	        }
	    }
    }
}
