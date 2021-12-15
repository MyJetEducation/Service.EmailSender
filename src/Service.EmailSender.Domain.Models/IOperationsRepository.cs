using System.Threading.Tasks;

namespace Service.EmailSender.Domain.Models
{
	public interface IOperationsRepository
	{
		Task Save(string value);
	}
}