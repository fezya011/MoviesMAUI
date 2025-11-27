using Microsoft.Extensions.DependencyInjection;
using MyMediator.Interfaces;
using System.Collections.Concurrent;
using System.Linq.Expressions;
using System.Reflection;

namespace MyMediator.Types
{
    /// <summary>
    /// Статический класс, реализующий кэширование фабрик пайплайнов обработки запросов.
    /// Позволяет запустить цепочку поведений, связанных с выполнением запросов
    /// </summary>
    static class PipelineFactoryCache
    {
        /// <summary>
        /// Кэш делегатов, выполняющих обработку запроса через пайплайн. Ключ - пара: (тип запроса, тип ответа)
        /// </summary>
        private static readonly ConcurrentDictionary<(Type RequestType, Type ResponseType), Delegate> _cache = new();

        /// <summary>
        /// Получает или добавляет в кэш делегат для обработки запроса указанного типа
        /// </summary>
        /// <typeparam name="TResponse">Тип результата обработки запроса.</typeparam>
        /// <param name="requestType">Тип запроса.</param>
        /// <returns>Делегат, выполняющий обработку запроса через зарегистрированный обработчик и цепочку поведений.</returns>
        public static Func<object, IServiceProvider, CancellationToken, Task<TResponse>> GetOrAdd<TResponse>(Type requestType)
        {
            var key = (requestType, typeof(TResponse));
            return (Func<object, IServiceProvider, CancellationToken, Task<TResponse>>)
                _cache.GetOrAdd(key, _ =>
                {
                    var method = typeof(PipelineFactoryCache)
                        .GetMethod(nameof(Build), BindingFlags.Static | BindingFlags.NonPublic)!
                        .MakeGenericMethod(requestType, typeof(TResponse))
                        .Invoke(null, null)!;
                    return (Delegate)method;
                });
        }

        /// <summary>
        /// Создаёт делегат, который обрабатывает запрос и возвращает результат
        /// Делегат интегрирует основной обработчик запроса и все зарегистрированные поведения пайплайна (pipeline behaviors),
        /// применяя их в обратном порядке (от последнего к первому).
        /// </summary>
        /// <typeparam name="TRequest">Тип запроса.</typeparam>
        /// <typeparam name="TResponse">Тип результата обработки запроса.</typeparam>
        /// <returns>Делегат, выполняющий полную обработку запроса через пайплайн.</returns>
        private static Func<object, IServiceProvider, CancellationToken, Task<TResponse>> Build<TRequest, TResponse>()
            where TRequest : IRequest<TResponse>
        {
            var handlerInvoker = CreateHandlerInvoker<TRequest, TResponse>();
            var behaviorInvoker = CreateBehaviorInvoker<TRequest, TResponse>();

            return (requestObj, sp, ct) =>
            {
                var request = (TRequest)requestObj;

                var handler = sp.GetService<IRequestHandler<TRequest, TResponse>>();
                if (handler == null)
                    throw new InvalidOperationException($"Handler for {typeof(TRequest)} not registered.");

                var behaviors = sp.GetService<IEnumerable<IPipelineBehavior<TRequest, TResponse>>>()
                       ?? Array.Empty<IPipelineBehavior<TRequest, TResponse>>();

                RequestHandlerDelegate<TResponse> next = () => handlerInvoker(handler, request, ct);

                foreach (var behavior in behaviors.Reverse())
                {
                    var b = behavior;
                    var n = next;
                    next = () => behaviorInvoker(b, request, n, ct);
                }

                return next();
            };
        }

        /// <summary>
        /// Создаёт скомпилированный делегат для вызова метода 'HandleAsync' у обработчика запроса (IRequestHandler)"/>
        /// Использует Expression Trees для генерации эффективного вызова без рефлексии во время выполнения.
        /// </summary>
        /// <typeparam name="TRequest">Тип запроса.</typeparam>
        /// <typeparam name="TResponse">Тип результата обработки запроса.</typeparam>
        /// <returns>Делегат, вызывающий 'HandleAsync' у обработчика запроса.</returns>
        private static Func<IRequestHandler<TRequest, TResponse>, TRequest, CancellationToken, Task<TResponse>>
            CreateHandlerInvoker<TRequest, TResponse>() where TRequest : IRequest<TResponse>
        {
            var h = Expression.Parameter(typeof(IRequestHandler<TRequest, TResponse>), "h");
            var r = Expression.Parameter(typeof(TRequest), "r");
            var c = Expression.Parameter(typeof(CancellationToken), "c");
            var call = Expression.Call(h, nameof(IRequestHandler<TRequest, TResponse>.HandleAsync), null, r, c);
            return Expression.Lambda<Func<IRequestHandler<TRequest, TResponse>, TRequest, CancellationToken, Task<TResponse>>>(call, h, r, c).Compile();
        }

        /// <summary>
        /// Создаёт скомпилированный делегат для вызова метода 'HandleAsync' у обработчика запроса (IPipelineBehavior)"/>
        /// Использует Expression Trees для генерации эффективного вызова без рефлексии во время выполнения.
        /// </summary>
        /// <typeparam name="TRequest">Тип запроса.</typeparam>
        /// <typeparam name="TResponse">Тип результата обработки запроса.</typeparam>
        /// <returns>Делегат, вызывающий 'HandleAsync' у обработчика запроса.</returns>
        private static Func<IPipelineBehavior<TRequest, TResponse>, TRequest, RequestHandlerDelegate<TResponse>, CancellationToken, Task<TResponse>>
            CreateBehaviorInvoker<TRequest, TResponse>() where TRequest : IRequest<TResponse>
        {
            var b = Expression.Parameter(typeof(IPipelineBehavior<TRequest, TResponse>), "b");
            var r = Expression.Parameter(typeof(TRequest), "r");
            var n = Expression.Parameter(typeof(RequestHandlerDelegate<TResponse>), "n");
            var c = Expression.Parameter(typeof(CancellationToken), "c");
            var call = Expression.Call(b, nameof(IPipelineBehavior<TRequest, TResponse>.HandleAsync), null, r, n, c);
            return Expression.Lambda<Func<IPipelineBehavior<TRequest, TResponse>, TRequest, RequestHandlerDelegate<TResponse>, CancellationToken, Task<TResponse>>>(call, b, r, n, c).Compile();
        }
    }
}
