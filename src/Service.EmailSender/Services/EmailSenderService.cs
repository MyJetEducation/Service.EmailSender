using System.Diagnostics;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using MyJetWallet.Sdk.Service;
using Service.Core.Client.Extensions;
using Service.Core.Client.Models;
using Service.EmailSender.Domain.Models;
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
			using Activity activity = MyTelemetry.StartActivity("Send recovery password message");

			string email = request.Email;

			OperationResult<bool> sendingResult = await _emailSender.SendMailAsync(new EmailModel
			{
				To = email,
				Subject = "Recovery Password",
				Data = new HashEmailDataModel
				{
					Hash = request.Hash
				}
			});

			string emailMasked = email.Mask();

			if (sendingResult.Error)
			{
				_logger.LogError("Unable to send recovery password message to: {email}. Error message: {errorMessage}", emailMasked, sendingResult.ErrorMessage);
				return CommonGrpcResponse.Fail;
			}

			_logger.LogInformation("Sent recovery password message to {email}", emailMasked);
			return CommonGrpcResponse.Success;
		}

		public async ValueTask<CommonGrpcResponse> SendRegistrationConfirmEmailAsync(RegistrationConfirmGrpcRequest request)
		{
			using Activity activity = MyTelemetry.StartActivity("Send registration confirm message");

			string email = request.Email;

			OperationResult<bool> sendingResult = await _emailSender.SendMailAsync(new EmailModel
			{
				To = email,
				Subject = "Registration Confirm",
				Data = new HashEmailDataModel
				{
					Hash = request.Hash
				}
			});

			string emailMasked = email.Mask();

			if (sendingResult.Error)
			{
				_logger.LogError("Unable to send registration confirm message to: {email}. Error message: {errorMessage}", emailMasked, sendingResult.ErrorMessage);
				return CommonGrpcResponse.Fail;
			}

			_logger.LogInformation("Sent registration confirm message to {email}", emailMasked);
			return CommonGrpcResponse.Success;
		}

		public async ValueTask<CommonGrpcResponse> SendChangeEmailAsync(ChangeEmailGrpcRequest request)
		{
			using Activity activity = MyTelemetry.StartActivity("Send change email message");

			string email = request.Email;

			OperationResult<bool> sendingResult = await _emailSender.SendMailAsync(new EmailModel
			{
				To = email,
				Subject = "Change Email",
				Data = new HashEmailDataModel
				{
					Hash = request.Hash
				}
			});

			string emailMasked = email.Mask();

			if (sendingResult.Error)
			{
				_logger.LogError("Unable to send change email message to: {email}. Error message: {errorMessage}", emailMasked, sendingResult.ErrorMessage);
				return CommonGrpcResponse.Fail;
			}

			_logger.LogInformation("Sent change email message to {email}", emailMasked);
			return CommonGrpcResponse.Success;
		}
	}
}