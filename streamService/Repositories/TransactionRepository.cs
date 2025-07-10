using UnistreamService.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using UnistreamService.Data;
using UnistreamService.Model;

namespace UnistreamService.Repositories;

public class TransactionRepository: ITransactionRepository
{
    private readonly AppDbContext _appDbContext;

    public TransactionRepository(AppDbContext appDbContext)
    {
        _appDbContext = appDbContext;
    }
    public async Task<TransactionEntity> Get(Guid id)
    {
        return await _appDbContext.Transaction.FindAsync(id);
    }

    public async Task Create(TransactionEntity transaction)
    {
        await _appDbContext.Transaction.AddAsync(transaction);
        await _appDbContext.SaveChangesAsync();
    }

    public async Task EnsureTransactionLimit(int maxCount)
    {

        var count = await _appDbContext.Transaction.CountAsync();
        if(count<=maxCount) return;
        var oldTransactions = await _appDbContext.Transaction
            .OrderBy(t => t.InsertDateTime)
            .Take(count - maxCount)
            .ToListAsync();

        _appDbContext.Transaction.RemoveRange(oldTransactions);
        await _appDbContext.SaveChangesAsync();
    }
}