namespace Service.EmailSender.Postgres.Services
{
	public interface IOperationsRepository
	{
		Task Save(string value);
	}
}