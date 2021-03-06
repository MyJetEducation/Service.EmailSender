namespace Service.EmailSender.Models
{
	public class OperationResult<T>
	{
		public OperationResult()
		{
		}

		public OperationResult(T data)
		{
			Value = data;
		}

		public static OperationResult<bool> ErrorResult(string message) => new() {ErrorMessage = message, Value = false};

		public bool Error => ErrorMessage != null;
		public string ErrorMessage { get; set; }
		public T Value { get; set; }
	}
}