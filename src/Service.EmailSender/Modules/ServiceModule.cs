using Autofac;
using Service.EmailSender.Domain.Models;
using Service.EmailSender.Services;
using Service.KeyValue.Client;
using Service.UserInfo.Crud.Client;

namespace Service.EmailSender.Modules
{
	public class ServiceModule : Module
	{
		protected override void Load(ContainerBuilder builder)
		{
			builder.RegisterKeyValueClient(Program.Settings.KeyValueServiceUrl);
			builder.RegisterUserInfoCrudClient(Program.Settings.UserInfoCrudServiceUrl);

			//builder.RegisterType<SendGridEmailSender>().As<ISendGridEmailSender>().SingleInstance();

			//TODO: delete fake sendgrid sender
			builder.RegisterType<SendGridEmailFakeSender>().As<ISendGridEmailSender>().SingleInstance();
		}
	}
}