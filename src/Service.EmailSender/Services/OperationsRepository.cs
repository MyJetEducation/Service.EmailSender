using System;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Service.EmailSender.Models;
using Service.EmailSender.Postgres;
using Service.EmailSender.Postgres.Models;
using Service.EmailSender.Postgres.Services;

namespace Service.EmailSender.Services
{
	public class OperationsRepository : IOperationsRepository
	{
		private readonly DbContextOptionsBuilder<DatabaseContext> _dbContextOptionsBuilder;
		private readonly ILogger<OperationsRepository> _logger;

		public OperationsRepository(DbContextOptionsBuilder<DatabaseContext> dbContextOptionsBuilder, ILogger<OperationsRepository> logger)
		{
			_dbContextOptionsBuilder = dbContextOptionsBuilder;
			_logger = logger;
		}

		public async Task Save(string value)
		{
			DatabaseContext context = GetContext();

			try
			{
				await context.Operations.AddAsync(new OperationEntity
				{
					Date = DateTime.UtcNow,
					Value = value
				});
			}
			catch (Exception exception)
			{
				_logger.LogError(exception, exception.Message);
			}

			await context.SaveChangesAsync();
		}

		private DatabaseContext GetContext() => DatabaseContext.Create(_dbContextOptionsBuilder);
	}
}