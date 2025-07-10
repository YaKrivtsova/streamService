using UnistreamService.Domain.Entities;
using UnistreamService.Model;

namespace UnistreamService.Services;

public interface ITransactionService
{
    Task<Transaction?> GetByIdAsync(Guid id);
    Task<TransactionEntity> AddAsync(Transaction entity);
    Task EnsureTransactionLimitAsync();
}