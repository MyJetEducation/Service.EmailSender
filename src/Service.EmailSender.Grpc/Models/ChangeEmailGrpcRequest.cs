using System.Runtime.Serialization;

namespace Service.EmailSender.Grpc.Models
{
	[DataContract]
	public class ChangeEmailGrpcRequest
	{
		[DataMember(Order = 1)]
		public string Email { get; set; }

		[DataMember(Order = 2)]
		public string Hash { get; set; }
	}
}