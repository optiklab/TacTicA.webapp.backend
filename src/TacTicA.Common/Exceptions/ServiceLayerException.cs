using System.Collections;
using System.Globalization;
using System.Runtime.CompilerServices;
using System.Runtime.Serialization;
using System.Text;

namespace TacTicA.Common.Exceptions;

/// <summary>
/// Represents a base service layer exception.
/// </summary>
[Serializable]
public abstract class ServiceLayerException : ActioException
{
    /// <summary>
    /// Returns a string representation of the <see cref="ServiceLayerException"/> which includes a name-value list of exception properties.
    /// </summary>
    /// <returns>String representation of the object.</returns>
    public override string ToString()
    {
        try
        {
            var sb = new StringBuilder(base.ToString());
            sb.Append("Properties:[");

            foreach (DictionaryEntry property in Data)
            {
                sb.AppendLine();
                sb.Append(CultureInfo.InvariantCulture, $"Name:{property.Key} Value:{property.Value}");
            }

            sb.AppendLine();
            sb.Append(']');
            return sb.ToString();
        }
        catch
        {
            return base.ToString();
        }
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="ServiceLayerException"/> class.
    /// </summary>
    /// <param name="message">The exception message.</param>
    /// <param name="innerException">The exception that is the cause of the current exception, or <b>null</b> if no inner exception is specified.</param>
    protected ServiceLayerException(string message, Exception? innerException = null) : base(message, innerException)
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="ServiceLayerException"/> class.
    /// </summary>
    /// <param name="message">The exception message.</param>
    /// <param name="data">The dictionary of exception properties.</param>
    protected ServiceLayerException(string message, IDictionary data) : base(message)
    {
        SetData(data);
    }

    /// <summary>
    /// Sets the value of an exception property.
    /// </summary>
    /// <typeparam name="T">The type of the property.</typeparam>
    /// <param name="value">The value of the property.</param>
    /// <param name="propertyName">The name of the property.</param>
    protected void SetProperty<T>(T value, [CallerMemberName] string propertyName = "UndefinedKey")
    {
        SetDataItem(propertyName, value);
    }

    /// <summary>
    /// Gets the value of an exception property from the internal <see cref="Exception.Data"/> collection.
    /// </summary>
    /// <typeparam name="T">The type of the property.</typeparam>
    /// <param name="propertyName">The name of the property.</param>
    /// <returns>The value of the property of type <typeparamref name="T"/>.</returns>
    protected T? GetProperty<T>([CallerMemberName] string? propertyName = null)
    {
        if (propertyName == null)
        {
            return default;
        }

        if (!Data.Contains(propertyName))
        {
            return default;
        }

        return Data[propertyName] is T value ? value : default;
    }

    private void SetDataItem<T>(string propertyName, T value)
    {
        Data.Add(propertyName, value);
    }

    private void SetData(IDictionary? data)
    {
        if (data == null)
        {
            return;
        }

        foreach (DictionaryEntry kvp in data)
        {
            Data.Add(kvp.Key, kvp.Value);
        }
    }
}