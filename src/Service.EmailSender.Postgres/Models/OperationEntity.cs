namespace Service.EmailSender.Postgres.Models
{
	public class OperationEntity
	{
		public int? Id { get; set; }

		public DateTime Date { get; set; }

		public string Value { get; set; }
	}
}