using System.Threading.Tasks;

namespace Service.EmailSender.Domain.Models
{
	public interface ISendGridEmailSender
	{
		ValueTask<OperationResult<bool>> SendMailAsync(EmailModel emailModel);
	}
}