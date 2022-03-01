using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;
using Service.Core.Client.Models;
using Service.EmailSender.Domain.Models;
using Service.EmailSender.Grpc.Models;
using Service.EmailSender.Models;
using Service.EmailSender.Services;

namespace Service.EmailSender.Tests
{
	public class EmailSenderServiceTests
	{
		private EmailSenderService _sut;
		private Mock<ISendGridEmailSender> _sender;

		[SetUp]
		public void Setup()
		{
			_sender = new Mock<ISendGridEmailSender>();

			_sut = new EmailSenderService(_sender.Object, new Mock<ILogger<EmailSenderService>>().Object);
		}

		[Test]
		public async Task SendRecoveryPasswordEmailAsync_send_email()
		{
			_sender
				.Setup(sender => sender.SendMailAsync(It.IsAny<EmailModel>()))
				.Callback<EmailModel>(model =>
				{
					Assert.AreEqual("email@domain.com", model.To);
					Assert.AreEqual("Recovery Password", model.Subject);
					Assert.AreEqual("123", ((HashEmailDataModel) model.Data).Hash);
				})
				.Returns(ValueTask.FromResult(new OperationResult<bool>(true)));

			CommonGrpcResponse result = await _sut.SendRecoveryPasswordEmailAsync(new RecoveryInfoGrpcRequest
			{
				Hash = "123",
				Email = "email@domain.com"
			});

			_sender.Verify(sender => sender.SendMailAsync(It.IsAny<EmailModel>()), Times.Once);

			Assert.IsTrue(result.IsSuccess);
		}

		[Test]
		public async Task SendRecoveryPasswordEmailAsync_return_error_if_not_email_sended()
		{
			_sender
				.Setup(sender => sender.SendMailAsync(It.IsAny<EmailModel>()))
				.Returns(ValueTask.FromResult(new OperationResult<bool>(false) {ErrorMessage = "error"}));

			CommonGrpcResponse result = await _sut.SendRecoveryPasswordEmailAsync(new RecoveryInfoGrpcRequest
			{
				Hash = "123",
				Email = "email@domain.com"
			});

			_sender.Verify(sender => sender.SendMailAsync(It.IsAny<EmailModel>()), Times.Once);

			Assert.IsFalse(result.IsSuccess);
		}

		[Test]
		public async Task SendRegistrationConfirmEmailAsync_send_email()
		{
			_sender
				.Setup(sender => sender.SendMailAsync(It.IsAny<EmailModel>()))
				.Callback<EmailModel>(model =>
				{
					Assert.AreEqual("email@domain.com", model.To);
					Assert.AreEqual("Registration Confirm", model.Subject);
					Assert.AreEqual("123", ((HashEmailDataModel) model.Data).Hash);
				})
				.Returns(ValueTask.FromResult(new OperationResult<bool>(true)));

			CommonGrpcResponse result = await _sut.SendRegistrationConfirmEmailAsync(new RegistrationConfirmGrpcRequest
			{
				Hash = "123",
				Email = "email@domain.com"
			});

			_sender.Verify(sender => sender.SendMailAsync(It.IsAny<EmailModel>()), Times.Once);

			Assert.IsTrue(result.IsSuccess);
		}

		[Test]
		public async Task SendRegistrationConfirmEmailAsync_return_error_if_not_email_sended()
		{
			_sender
				.Setup(sender => sender.SendMailAsync(It.IsAny<EmailModel>()))
				.Returns(ValueTask.FromResult(new OperationResult<bool>(false) {ErrorMessage = "error"}));

			CommonGrpcResponse result = await _sut.SendRegistrationConfirmEmailAsync(new RegistrationConfirmGrpcRequest
			{
				Hash = "123",
				Email = "email@domain.com"
			});

			_sender.Verify(sender => sender.SendMailAsync(It.IsAny<EmailModel>()), Times.Once);

			Assert.IsFalse(result.IsSuccess);
		}

		[Test]
		public async Task SendChangeEmailAsync_send_email()
		{
			_sender
				.Setup(sender => sender.SendMailAsync(It.IsAny<EmailModel>()))
				.Callback<EmailModel>(model =>
				{
					Assert.AreEqual("email@domain.com", model.To);
					Assert.AreEqual("Change Email", model.Subject);
					Assert.AreEqual("123", ((HashEmailDataModel) model.Data).Hash);
				})
				.Returns(ValueTask.FromResult(new OperationResult<bool>(true)));

			CommonGrpcResponse result = await _sut.SendChangeEmailAsync(new ChangeEmailGrpcRequest
			{
				Hash = "123",
				Email = "email@domain.com"
			});

			_sender.Verify(sender => sender.SendMailAsync(It.IsAny<EmailModel>()), Times.Once);

			Assert.IsTrue(result.IsSuccess);
		}

		[Test]
		public async Task SendChangeEmailAsync_return_error_if_not_email_sended()
		{
			_sender
				.Setup(sender => sender.SendMailAsync(It.IsAny<EmailModel>()))
				.Returns(ValueTask.FromResult(new OperationResult<bool>(false) {ErrorMessage = "error"}));

			CommonGrpcResponse result = await _sut.SendChangeEmailAsync(new ChangeEmailGrpcRequest
			{
				Hash = "123",
				Email = "email@domain.com"
			});

			_sender.Verify(sender => sender.SendMailAsync(It.IsAny<EmailModel>()), Times.Once);

			Assert.IsFalse(result.IsSuccess);
		}
	}
}