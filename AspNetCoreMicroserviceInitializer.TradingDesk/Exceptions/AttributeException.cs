namespace AspNetCoreMicroserviceInitializer.TradingDesk.Exceptions;

/// <summary>
/// <see cref="Exception"/> для отображения ошибок, происходящих в атрибутах <see cref="Attribute"/>.
/// </summary>
public class AttributeException : Exception
{
    /// <summary>
    /// Конструктор с базовым сообщением.
    /// </summary>
    public AttributeException() : base ("Ошибка обработки атрибута.")
    {
    }

    /// <summary>
    /// Конструктор с кастомным сообщением.
    /// </summary>
    /// <param name="message">Сообщение.</param>
    public AttributeException(string message) : base(message)
    {
    }

    /// <summary>
    /// Конструктор с кастомным сообщением и ошибкой.
    /// </summary>
    /// <param name="message">Сообщение.</param>
    /// <param name="innerException">Ошибка типа <see cref="Exception"/>.</param>
    public AttributeException(string message, Exception innerException) : base(message, innerException)
    {
    }
}
