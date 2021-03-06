using Autofac;
using SendGrid;
using Service.EmailSender.Postgres.Services;
using Service.EmailSender.Services;

namespace Service.EmailSender.Modules
{
	public class ServiceModule : Module
	{
		protected override void Load(ContainerBuilder builder)
		{
			builder.RegisterType<OperationsRepository>().AsImplementedInterfaces().SingleInstance();

			builder.Register(_ => new SendGridClient(Program.Settings.SendGridSettingsApiKey)).As<ISendGridClient>().SingleInstance();

			builder.RegisterType<SettingsManager>().AsImplementedInterfaces().SingleInstance();

			//builder.RegisterType<SendGridEmailSender>().As<ISendGridEmailSender>().SingleInstance();

			//TODO: delete fake sendgrid sender
			builder.RegisterType<SendGridEmailFakeSender>().As<ISendGridEmailSender>().SingleInstance();
		}
	}
}