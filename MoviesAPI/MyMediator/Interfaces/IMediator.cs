namespace MyMediator.Interfaces
{
    /// <summary>
    /// Базовый интерфейс медиатора
    /// </summary>
    public interface IMediator
    {
        /// <summary>
        /// Основной метод. Находит хэндлер для обработки команды и вызывает обработку
        /// </summary>
        /// <typeparam name="TResponse">Тип ответа</typeparam>
        /// <param name="request">Объект команды</param>
        /// <param name="ct">Стандартный токен отмены операции</param>
        /// <returns></returns>
        Task<TResponse> SendAsync<TResponse>(IRequest<TResponse> request, CancellationToken ct = default);

        /// <summary>
        /// Перегрузка метода для поддержки команд без возвращаемого результата
        /// </summary>
        /// <param name="command">Объект команды</param>
        /// <param name="ct">Стандартный токен отмены операции</param>
        /// <returns></returns>
        Task SendAsync(IRequest command, CancellationToken ct = default);
    }
}
