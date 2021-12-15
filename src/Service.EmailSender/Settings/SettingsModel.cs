using MyJetWallet.Sdk.Service;
using MyYamlParser;

namespace Service.EmailSender.Settings
{
	public class SettingsModel
	{
		[YamlProperty("EmailSender.SeqServiceUrl")]
		public string SeqServiceUrl { get; set; }

		[YamlProperty("EmailSender.ZipkinUrl")]
		public string ZipkinUrl { get; set; }

		[YamlProperty("EmailSender.ElkLogs")]
		public LogElkSettings ElkLogs { get; set; }

		[YamlProperty("EmailSender.SendGridSettingsApiKey")]
		public string SendGridSettingsApiKey { get; set; }

		[YamlProperty("EmailSender.From")]
		public string From { get; set; }

		[YamlProperty("EmailSender.KeyValueServiceUrl")]
		public string KeyValueServiceUrl { get; set; }

		[YamlProperty("EmailSender.UserInfoCrudServiceUrl")]
		public string UserInfoCrudServiceUrl { get; set; }
	}
}