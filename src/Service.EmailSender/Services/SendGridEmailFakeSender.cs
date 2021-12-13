using System;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Service.EmailSender.Domain.Models;

namespace Service.EmailSender.Services
{
	public class SendGridEmailFakeSender : ISendGridEmailSender
	{
		public async ValueTask<OperationResult<bool>> SendMailAsync(EmailModel emailModel)
		{
			Console.WriteLine($"Email sended (fake), contents: {JsonConvert.SerializeObject(emailModel)}");

			return await ValueTask.FromResult(new OperationResult<bool>(true));
		}
	}
}