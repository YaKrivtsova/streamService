using UnistreamService.Domain.Entities;
using UnistreamService.Model;

namespace UnistreamService.Repositories;

public interface ITransactionRepository
{
    public Task<TransactionEntity> Get(Guid id);
    public Task Create(TransactionEntity transaction);
    public Task EnsureTransactionLimit(int maxCount);
}