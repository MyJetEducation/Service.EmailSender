using System.ServiceModel;
using System.Threading.Tasks;
using Service.Core.Grpc.Models;
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