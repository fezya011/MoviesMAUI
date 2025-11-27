using MyMediator.Interfaces;

namespace MyMediator.Types
{
    /// <summary>
    /// Делегат для вызова следующего поведения
    /// </summary>
    /// <typeparam name="TResponse"></typeparam>
    /// <returns></returns>
    public delegate Task<TResponse> RequestHandlerDelegate<TResponse>();

    /// <summary>
    /// Интерфейс для поведений. Метод принимает команду и ссылку на следующее поведение
    /// </summary>
    /// <typeparam name="TRequest">Тип запроса</typeparam>
    /// <typeparam name="TResponse">Тип ответа</typeparam>
    public interface IPipelineBehavior<in TRequest, TResponse> 
        where TRequest : IRequest<TResponse>
    {
        /// <summary>
        /// Асинхронно обрабатывает запрос в рамках пайплайна.
        /// </summary>
        /// <param name="request">Обрабатываемый запрос.</param>
        /// <param name="next">Делегат, вызывающий следующий шаг в цепочке обработки.</param>
        /// <param name="cancellationToken">Токен отмены операции.</param>
        /// <returns>Задача, представляющая асинхронную операцию и содержащая результат.</returns>
        Task<TResponse> HandleAsync(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken);
    }
}
