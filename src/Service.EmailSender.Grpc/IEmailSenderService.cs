using System.ServiceModel;
using System.Threading.Tasks;
using Service.EmailSender.Grpc.Models;

namespace Service.EmailSender.Grpc
{
	[ServiceContract]
	public interface IEmailSenderService
	{
		[OperationContract]
		ValueTask<CommonGrpcResponse> SendRecoveryPasswordEmailAsync(RecoveryInfoGrpcRequest request);
	}
}