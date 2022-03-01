using System;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using Moq;
using NUnit.Framework;
using SendGrid;
using SendGrid.Helpers.Mail;
using Service.EmailSender.Domain.Models;
using Service.EmailSender.Models;
using Service.EmailSender.Services;
using Service.EmailSender.Settings;

namespace Service.EmailSender.Tests
{
	public class SendGridEmailSenderTests
	{
		private Mock<ISendGridClient> _sendGridClient;
		private Mock<ISettingsManager> _settingsManager;

		private SendGridEmailSender _sut;

		[SetUp]
		public void Setup()
		{
			_sendGridClient = new Mock<ISendGridClient>();
			_settingsManager = new Mock<ISettingsManager>();

			_settingsManager
				.Setup(manager => manager.GetValue(It.IsAny<Func<SettingsModel, string>>()))
				.Returns("from@string.ru");

			_sut = new SendGridEmailSender(_sendGridClient.Object, _settingsManager.Object);
		}

		[Test]
		public async Task SendMailAsync_prepare_mail_and_call_client()
		{
			_sendGridClient
				.Setup(client => client.SendEmailAsync(It.IsAny<SendGridMessage>(), It.IsAny<CancellationToken>()))
				.Callback<SendGridMessage, CancellationToken>((message, _) =>
				{
					Assert.IsNotNull(message);
					Assert.AreEqual("subj", message.Subject);
					Assert.AreEqual("template", message.TemplateId);
					Assert.AreEqual("from@string.ru", message.From.Email);
					Assert.AreEqual("subj", message.From.Name);
				})
				.Returns(Task.FromResult(new Response(HttpStatusCode.Accepted, null, null)));

			OperationResult<bool> result = await _sut.SendMailAsync(new EmailModel
			{
				To = "to",
				Subject = "subj",
				SendGridTemplateId = "template",
				Data = "data"
			});

			Assert.IsNotNull(result);
			Assert.IsTrue(result.Value);

			_sendGridClient.Verify(client => client.SendEmailAsync(It.IsAny<SendGridMessage>(), default), Times.Once);
		}

		[Test]
		public async Task SendMailAsync_return_false_if_call_is_not_succeed()
		{
			_sendGridClient
				.Setup(client => client.SendEmailAsync(It.IsAny<SendGridMessage>(), It.IsAny<CancellationToken>()))
				.Returns(Task.FromResult(new Response(HttpStatusCode.NotExtended, null, null)));

			OperationResult<bool> result = await _sut.SendMailAsync(new EmailModel
			{
				To = "to",
				Subject = "subj",
				SendGridTemplateId = "template",
				Data = "data"
			});

			Assert.IsNotNull(result);
			Assert.IsFalse(result.Value);
			Assert.AreEqual("SendGrid returned: {\"StatusCode\":510,\"IsSuccessStatusCode\":false,\"Body\":null,\"Headers\":null}", result.ErrorMessage);

			_sendGridClient.Verify(client => client.SendEmailAsync(It.IsAny<SendGridMessage>(), default), Times.Once);
		}

		[Test]
		public async Task SendMailAsync_does_not_throws_exception()
		{
			_sendGridClient
				.Setup(client => client.SendEmailAsync(It.IsAny<SendGridMessage>(), It.IsAny<CancellationToken>()))
				.Throws(new Exception("message"));

			OperationResult<bool> result = await _sut.SendMailAsync(new EmailModel
			{
				To = "to",
				Subject = "subj",
				SendGridTemplateId = "template",
				Data = "data"
			});

			Assert.DoesNotThrow(() =>
			{
				Assert.IsNotNull(result);
				Assert.IsFalse(result.Value);
				Assert.IsTrue(result.ErrorMessage.Contains("System.Exception: message"));
			});
		}
	}
}