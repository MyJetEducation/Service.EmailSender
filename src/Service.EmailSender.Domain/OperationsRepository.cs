using System;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Service.EmailSender.Domain.Models;
using Service.EmailSender.Postgres;

namespace Service.EmailSender.Domain
{
	public class OperationsRepository : IOperationsRepository
	{
		private readonly DbContextOptionsBuilder<DatabaseContext> _dbContextOptionsBuilder;

		public OperationsRepository(DbContextOptionsBuilder<DatabaseContext> dbContextOptionsBuilder) => _dbContextOptionsBuilder = dbContextOptionsBuilder;

		public async Task Save(string value)
		{
			DatabaseContext context = GetContext();

			await context.Operations.AddAsync(new OperationEntity
			{
				Date = DateTime.UtcNow,
				Value = value
			});

			await context.SaveChangesAsync();
		}

		private DatabaseContext GetContext() => DatabaseContext.Create(_dbContextOptionsBuilder);
	}
}