using FinanceManagement.Infrastructure.Dto;
using Microsoft.Extensions.Configuration;
using System.Net;
using System.Net.Mail;
using System.Text;

namespace FinanceManagement.MessageService
{
    public interface IMailSender
	{
		void SendHtmlEmail(MessageDto emailData);
	}

    public class MailSender : IMailSender
	{
        private readonly IConfiguration configuration;
		private readonly SmtpClient smtpClient;

		public MailSender(IConfiguration configuration)
        {
            this.configuration = configuration;

			smtpClient = new SmtpClient
			{
				Host = configuration["AppConfiguration:Mails:SmtpHost"],
				Port = int.Parse(configuration["AppConfiguration:Mails:SmtpPort"]),
				DeliveryMethod = SmtpDeliveryMethod.Network,
				UseDefaultCredentials = false,
				Credentials = new NetworkCredential(configuration["AppConfiguration:Mails:MainEmail"], configuration["AppConfiguration:Mails:MainEmailPassword"]),
				EnableSsl = true,
			};
		}

		public void SendHtmlEmail(MessageDto emailData)
		{
			MailAddress from = new MailAddress(configuration["AppConfiguration:Mails:MainEmail"], configuration["AppConfiguration:Mails:MainEmailName"]);
			MailAddress to = new MailAddress(emailData.Receiver);
			MailMessage message = new MailMessage(from, to);
			AlternateView htmlView = AlternateView.CreateAlternateViewFromString(emailData.Content, Encoding.UTF8, "text/html");

			message.AlternateViews.Add(htmlView);
			message.IsBodyHtml = true;
			message.Subject = emailData.Title;
			message.Body = emailData.Content;

			smtpClient.Send(message);
		}
	}
}
