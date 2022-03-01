using System;
using System.Threading.Tasks;
using ProtoBuf.Grpc.Client;
using Service.EmailSender.Client;
using Service.EmailSender.Grpc;
using Service.EmailSender.Grpc.Models;

namespace TestApp
{
	internal class Program
	{
		private static async Task Main()
		{
			GrpcClientFactory.AllowUnencryptedHttp2 = true;

			Console.Write("Press enter to start");
			Console.ReadLine();

			var factory = new EmailSenderClientFactory("http://localhost:5001");
			IEmailSenderService client = factory.GetEmailSender();

			await client.SendRecoveryPasswordEmailAsync(new RecoveryInfoGrpcRequest {Email = "some@email1", Hash = "some_hash2"});
			await client.SendRegistrationConfirmEmailAsync(new RegistrationConfirmGrpcRequest {Email = "some@email2", Hash = "some_hash2"});
			await client.SendChangeEmailAsync(new ChangeEmailGrpcRequest {Email = "some@email2", Hash = "some_hash2"});

			Console.WriteLine("End");
			Console.ReadLine();
		}
	}
}