using AutoMapper;
using Microsoft.Extensions.Options;
using UnistreamService.Domain.Entities;
using UnistreamService.Model;
using UnistreamService.Repositories;

namespace UnistreamService.Services;

public class TransactionService: ITransactionService
{
    private readonly ITransactionRepository _repository;
    private readonly IMapper _mapper;
    private readonly TransactionSettings _settings;
    
    public TransactionService(ITransactionRepository repository, IMapper mapper,  IOptions<TransactionSettings> settings)
    {
        _repository = repository;
        _mapper = mapper;
        _settings = settings.Value;
    }
    public async Task<Transaction?> GetByIdAsync(Guid id)
    {
        var entity = await _repository.Get(id);
        return entity is not null ? _mapper.Map<Transaction>(entity) : null;
    }

    public async Task<TransactionEntity> AddAsync(Transaction request)
    {
        var existing = await _repository.Get(request.Id);
        if (existing is not null)
            return existing;
        if (request.Amount <= 0)
            throw new ArgumentException("Amount must be positive");
        if (request.TransactionDate > DateTime.UtcNow)
            throw new ArgumentException("Transaction date cannot be in the future");
        var entity = _mapper.Map<TransactionEntity>(request);
        entity.InsertDateTime = DateTime.UtcNow;
        await _repository.Create(entity);
        await EnsureTransactionLimitAsync();
        return entity;
    }

    public async Task EnsureTransactionLimitAsync()
    {
        await _repository.EnsureTransactionLimit(_settings.MaxStoredTransactions);
    }
}