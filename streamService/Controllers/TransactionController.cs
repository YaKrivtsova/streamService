using Microsoft.AspNetCore.Mvc;
using UnistreamService.Model;
using UnistreamService.Services;

namespace UnistreamService.Controllers;

[ApiController]
[Route("api/v1/Transaction")]
public class TransactionController : ControllerBase
{
    private readonly ITransactionService _service;
    public TransactionController(ITransactionService service)
    {
        _service = service;
    }
    
    /// <summary>
    /// Создание транзакции
    /// </summary>
    /// <param name="request">Объект транзакции</param>
    /// <returns>Дата и время вставки</returns>
    /// <response code="200">Транзакция успешно создана или уже существовала</response>
    /// <response code="400">Ошибка валидации данных (например, сумма отрицательная или дата в будущем)</response>
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ProblemDetails))]
    public async Task<IActionResult> Create(Transaction request)
    {
        var result = await _service.AddAsync(request);
        return Ok(new { insertDateTime = result.InsertDateTime });
    }
    
    /// <summary>
    /// Получает транзакцию по идентификатору.
    /// </summary>
    /// <param name="id">Идентификатор транзакции</param>
    /// <returns>Данные транзакции</returns>
    /// <response code="200">Транзакция найдена</response>
    /// <response code="404">Транзакция не найдена</response>
    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(Transaction))]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetById([FromQuery] Guid id)
    {
        var transaction = await _service.GetByIdAsync(id);
        if (transaction is null)
            return NotFound();

        return Ok(transaction);
    }
}