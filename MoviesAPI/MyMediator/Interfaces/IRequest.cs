using MyMediator.Types;

namespace MyMediator.Interfaces
{
    /// <summary>
    /// Основной интерфейс для классов команд
    /// </summary>
    /// <typeparam name="TResponse">Тип результата при обработке команды</typeparam>
    public interface IRequest<out TResponse> { }

    /// <summary>
    /// Вариант интерфейса для поддержки команд без возвращаемого результата
    /// </summary>
    public interface IRequest : IRequest<Unit> { }
}
