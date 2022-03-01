using System;
using System.Net;
using System.Threading.Tasks;
using Newtonsoft.Json;
using SendGrid;
using SendGrid.Helpers.Mail;
using Service.EmailSender.Domain.Models;
using Service.EmailSender.Models;

namespace Service.EmailSender.Services
{
	public class SendGridEmailSender : ISendGridEmailSender
	{
		private readonly ISendGridClient _sendGridClient;
		private readonly ISettingsManager _settingsManager;

		public SendGridEmailSender(ISendGridClient sendGridClient, ISettingsManager settingsManager)
		{
			_sendGridClient = sendGridClient;
			_settingsManager = settingsManager;
		}

		public async ValueTask<OperationResult<bool>> SendMailAsync(EmailModel emailModel)
		{
			try
			{
				string fromString = _settingsManager.GetValue(model => model.From);

				var msg = new SendGridMessage
				{
					From = new EmailAddress(fromString, emailModel.Subject),
					Subject = emailModel.Subject,
					TemplateId = emailModel.SendGridTemplateId
				};

				msg.AddTo(emailModel.To);
				msg.SetTemplateData(emailModel.Data);

				Response response = await _sendGridClient.SendEmailAsync(msg);

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