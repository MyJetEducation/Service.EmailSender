using System;
using System.Net;
using System.Threading.Tasks;
using Newtonsoft.Json;
using SendGrid;
using SendGrid.Helpers.Mail;
using Service.EmailSender.Domain.Models;

namespace Service.EmailSender.Services
{
	public class SendGridEmailSender : ISendGridEmailSender
	{
		private readonly SendGridClient _client;

		public SendGridEmailSender() => _client = new SendGridClient(Program.Settings.SendGridSettingsApiKey);

		public async ValueTask<OperationResult<bool>> SendMailAsync(EmailModel emailModel)
		{
			try
			{
				var from = new OperationResult<string>(Program.Settings.From);

				if (from.Error)
					return OperationResult<bool>.ErrorResult(from.ErrorMessage);

				var msg = new SendGridMessage
				{
					From = new EmailAddress(from.Value, emailModel.Subject),
					Subject = emailModel.Subject,
					TemplateId = emailModel.SendGridTemplateId
				};

				msg.AddTo(emailModel.To);
				msg.SetTemplateData(emailModel.Data);

				Response response = await _client.SendEmailAsync(msg);

				if (response.StatusCode != HttpStatusCode.Accepted)
					return OperationResult<bool>.ErrorResult("SendGrid returned: " + JsonConvert.SerializeObject(response));
			}
			catch (Exception exception)
			{
				return OperationResult<bool>.ErrorResult(exception.ToString());
			}

			return new OperationResult<bool>(true);
		}
	}
}