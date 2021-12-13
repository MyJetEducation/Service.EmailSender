namespace Service.EmailSender.Services
{
	public static class EmailMaskedHelper
	{
		public static string Mask(this string email)
		{
			if (email.Length <= 8)
				return email.Length <= 5
					? "*****"
					: $"{email[0]}**{email[^1]}";

			return $"{email.Substring(0, 3)}**{email.Substring(email.Length - 4, 3)}";
		}
	}
}