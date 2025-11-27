namespace MyMediator.Interfaces
{
    /// <summary>
    /// Интерфейс для хэндлеров. По CQRS для каждой команды существует отдельный класс-обработчик. 
    /// </summary>
    /// <typeparam name="TRequest">Тип команды</typeparam>
    /// <typeparam name="TResponse">Тип результата</typeparam>
    public interface IRequestHandler<in TRequest, TResponse>
        where TRequest : IRequest<TResponse>
    {
        Task<TResponse> HandleAsync(TRequest request, CancellationToken ct = default);
    }
}
