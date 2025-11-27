namespace MyMediator.Types
{
    /// <summary>
    /// Специальный тип данных вместо void
    /// </summary>
    public readonly struct Unit
    {
        /// <summary>
        /// Это значение возвращают все хэндлеры для команд, реализующих IRequest<Unit>
        /// </summary>
        public static readonly Unit Value = new();
    }
}
