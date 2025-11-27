using MyMediator.Interfaces;

namespace MyMediator.Types
{
    /// <summary>
    /// Основной класс медиатора. Содержит словарь соответствий типа Команда - Обработчик
    /// </summary>
    public class Mediator : IMediator
    {
        private readonly IServiceProvider _serviceProvider;

        public Mediator(IServiceProvider serviceProvider)
            => _serviceProvider = serviceProvider;

        /// <summary>
        /// Перегрузка для команд, реализующих IRequest<Unit>
        /// </summary>
        /// <param name="command">Команда</param>
        /// <param name="ct">Стандартный токен отмены</param>
        /// <returns></returns>
        public Task SendAsync(IRequest command, CancellationToken ct = default)
            => SendAsync<Unit>(command, ct);

        /// <summary>
        /// Команда передаетcя в пайплайн, который выполнит требуемые поведения, а в конце запустит обработчик команды
        /// </summary>
        /// <typeparam name="TResponse">Тип ответа</typeparam>
        /// <param name="request">Команда</param>
        /// <param name="ct">Стандартный токен отмены</param>
        /// <returns>Результат обработки команды</returns>
        /// <exception cref="InvalidOperationException"></exception>
        public Task<TResponse> SendAsync<TResponse>(IRequest<TResponse> request, CancellationToken ct = default)
        {
            var factory = PipelineFactoryCache.GetOrAdd<TResponse>(request.GetType());
            return factory(request, _serviceProvider, ct);
        }
    }
}
