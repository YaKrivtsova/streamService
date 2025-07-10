using AutoMapper;
using UnistreamService.Domain.Entities;
using UnistreamService.Model;

namespace UnistreamService.AutoMapper.Profiles;

public class TransactionProfile: Profile
{
    public TransactionProfile()
    {
        CreateMap<Transaction, TransactionEntity>()
            .ForMember(dest => dest.InsertDateTime, opt => opt.Ignore());
        CreateMap<TransactionEntity, Transaction>();
    }
}