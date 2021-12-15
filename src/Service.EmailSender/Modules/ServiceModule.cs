using Autofac;
using Service.EmailSender.Domain;
using Service.EmailSender.Domain.Models;
using Service.EmailSender.Services;

namespace Service.EmailSender.Modules
{
	public class ServiceModule : Module
	{
		protected override void Load(ContainerBuilder builder)
		{
			builder.RegisterType<OperationsRepository>().AsImplementedInterfaces().SingleInstance();

			//builder.RegisterType<SendGridEmailSender>().As<ISendGridEmailSender>().SingleInstance();

			//TODO: delete fake sendgrid sender
			builder.RegisterType<SendGridEmailFakeSender>().As<ISendGridEmailSender>().SingleInstance();
		}
	}
}