using Microsoft.EntityFrameworkCore;
using UnistreamService.Domain.Entities;

namespace UnistreamService.Data;

public class AppDbContext: DbContext
{
    public AppDbContext(DbContextOptions options) : base(options)
    {
    }
    public DbSet<TransactionEntity> Transaction { get; set; }
}