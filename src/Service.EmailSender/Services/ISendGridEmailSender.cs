using System.Threading.Tasks;
using Service.EmailSender.Domain.Models;
using Service.EmailSender.Models;

namespace Service.EmailSender.Services
{
	public interface ISendGridEmailSender
	{
		ValueTask<OperationResult<bool>> SendMailAsync(EmailModel emailModel);
	}
}