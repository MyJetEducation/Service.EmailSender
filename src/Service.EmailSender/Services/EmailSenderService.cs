using System.Diagnostics;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using MyJetWallet.Sdk.Service;
using Service.EmailSender.Domain.Models;
using Service.EmailSender.Grpc;
using Service.EmailSender.Grpc.Models;

namespace Service.EmailSender.Services
{
	public class EmailSenderService : IEmailSenderService
	{
		private readonly ISendGridEmailSender _emailSender;
		private readonly ILogger<EmailSenderService> _logger;

		public EmailSenderService(ISendGridEmailSender emailSender, ILogger<EmailSenderService> logger)
		{
			_emailSender = emailSender;
			_logger = logger;
		}

		public async ValueTask<CommonGrpcResponse> SendRecoveryPasswordEmailAsync(RecoveryInfoGrpcRequest request)
		{
			using Activity activity = MyTelemetry.StartActivity("Send Recover Password Email");

			string email = request.Email;

			var emailModel = new EmailModel
			{
				To = email,
				SendGridTemplateId = Program.Settings.SendGridTemplateId,
				Subject = Program.Settings.Subject,
				Data = new
				{
					request.Hash
				}
			};

			OperationResult<bool> sendingResult = await _emailSender.SendMailAsync(emailModel);

			string emailMasked = email.Mask();

			if (sendingResult.Error)
			{
				_logger.LogError("Unable to send RecoveryPasswordEmail to userId {email}. Error message: {errorMessage}", emailMasked, sendingResult.ErrorMessage);
				return CommonGrpcResponse.Fail;
			}

			_logger.LogInformation("Sent RecoveryPasswordEmail to {email}", emailMasked);
			return CommonGrpcResponse.Success;
		}
	}
}