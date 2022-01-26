using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Service.EmailSender.Models;
using Service.EmailSender.Postgres.Services;

namespace Service.EmailSender.Services
{
	public class SendGridEmailFakeSender : ISendGridEmailSender
	{
		private readonly ILogger<SendGridEmailFakeSender> _logger;
		private readonly IOperationsRepository _operationsRepository;

		public SendGridEmailFakeSender(ILogger<SendGridEmailFakeSender> logger, IOperationsRepository operationsRepository)
		{
			_logger = logger;
			_operationsRepository = operationsRepository;
		}

		public async ValueTask<OperationResult<bool>> SendMailAsync(EmailModel emailModel)
		{
			string mailContents = JsonConvert.SerializeObject(emailModel);

			string email = emailModel.To;

			await _operationsRepository.Save(mailContents);

			_logger.LogDebug("Email sended (fake) to {email}, contents: {mailContents}", email, mailContents);

			return new OperationResult<bool>(true);
		}
	}
}