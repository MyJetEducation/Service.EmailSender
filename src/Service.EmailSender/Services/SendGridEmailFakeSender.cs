using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Service.Core.Grpc.Models;
using Service.EmailSender.Domain.Models;
using Service.KeyValue.Grpc;
using Service.KeyValue.Grpc.Models;
using Service.UserInfo.Crud.Grpc;
using Service.UserInfo.Crud.Grpc.Models;

namespace Service.EmailSender.Services
{
	public class SendGridEmailFakeSender : ISendGridEmailSender
	{
		private readonly IKeyValueService _keyValueService;
		private readonly IUserInfoService _userInfoService;
		private readonly ILogger<SendGridEmailFakeSender> _logger;

		public SendGridEmailFakeSender(IKeyValueService keyValueService, ILogger<SendGridEmailFakeSender> logger, IUserInfoService userInfoService)
		{
			_keyValueService = keyValueService;
			_logger = logger;
			_userInfoService = userInfoService;
		}

		public async ValueTask<OperationResult<bool>> SendMailAsync(EmailModel emailModel)
		{
			string mailContents = JsonConvert.SerializeObject(emailModel);

			string email = emailModel.To;
			UserInfoResponse userInfo = await _userInfoService.GetUserInfoByLoginAsync(new UserInfoAuthRequest {UserName = email});
			if (userInfo == null)
			{
				_logger.LogError("User with email {email} not found, email not sended.", email);
				return await ValueTask.FromResult(new OperationResult<bool>(false));
			}

			CommonGrpcResponse response = await _keyValueService.Put(new ItemsPutGrpcRequest
			{
				UserId = userInfo.UserInfo.UserId,
				Items = new[]
				{
					new KeyValueGrpcModel
					{
						Key = "email",
						Value = mailContents
					}
				}
			});

			_logger.LogDebug($"Email sended (fake), contents: {mailContents}");

			return await ValueTask.FromResult(new OperationResult<bool>(response.IsSuccess));
		}
	}
}