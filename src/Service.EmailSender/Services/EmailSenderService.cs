using System.Diagnostics;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using MyJetWallet.Sdk.Service;
using Service.Core.Client.Extensions;
using Service.Core.Client.Models;
using Service.EmailSender.Grpc;
using Service.EmailSender.Grpc.Models;
using Service.EmailSender.Models;

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

			OperationResult<bool> sendingResult = await _emailSender.SendMailAsync(new EmailModel
			{
				To = email,
				Subject = "Recovery Password",
				Data = new
				{
					request.Hash
				}
			});

			string emailMasked = email.Mask();

			if (sendingResult.Error)
			{
				_logger.LogError("Unable to send RecoveryPasswordEmail to userId {email}. Error message: {errorMessage}", emailMasked, sendingResult.ErrorMessage);
				return CommonGrpcResponse.Fail;
			}

			_logger.LogInformation("Sent RecoveryPasswordEmail to {email}", emailMasked);
			return CommonGrpcResponse.Success;
		}

		public async ValueTask<CommonGrpcResponse> SendRegistrationConfirmEmailAsync(RegistrationConfirmGrpcRequest request)
		{
			using Activity activity = MyTelemetry.StartActivity("Send Registration Confirm Email");

			string email = request.Email;

			OperationResult<bool> sendingResult = await _emailSender.SendMailAsync(new EmailModel
			{
				To = email,
				Subject = "Registration Confirm",
				Data = new
				{
					request.Hash
				}
			});

			string emailMasked = email.Mask();

			if (sendingResult.Error)
			{
				_logger.LogError("Unable to send RegistrationConfirmEmail to userId {email}. Error message: {errorMessage}", emailMasked, sendingResult.ErrorMessage);
				return CommonGrpcResponse.Fail;
			}

			_logger.LogInformation("Sent RegistrationConfirmEmail to {email}", emailMasked);
			return CommonGrpcResponse.Success;
		}
	}
}