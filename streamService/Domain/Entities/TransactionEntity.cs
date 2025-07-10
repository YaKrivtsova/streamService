using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace UnistreamService.Domain.Entities;

[Table( "transaction")]
public class TransactionEntity
{
    [Column("id")]
    [Key]
    public Guid Id { get; set; }

    [Column("transactionDate")]
    public DateTime TransactionDate { get; set; }

    [Column("amount")]
    public decimal Amount { get; set; }

    [Column("insertDateTime")]
    public DateTime InsertDateTime { get; set; }
}