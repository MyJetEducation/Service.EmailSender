using System;
using MySettingsReader;
using Service.EmailSender.Settings;

namespace Service.EmailSender.Services
{
	public class SettingsManager : ISettingsManager
	{
		public Func<T> GetFunc<T>(Func<SettingsModel, T> getter) => () =>
		{
			var settings = SettingsReader.GetSettings<SettingsModel>(Program.SettingsFileName);

			return getter.Invoke(settings);
		};

		public T GetValue<T>(Func<SettingsModel, T> getter) => GetFunc(getter).Invoke();

		public SettingsModel Settings => Program.Settings;
	}

	public interface ISettingsManager
	{
		Func<T> GetFunc<T>(Func<SettingsModel, T> getter);

		T GetValue<T>(Func<SettingsModel, T> getter);

		SettingsModel Settings { get; }
	}
}