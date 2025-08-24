using Irodori.Error;

namespace Irodori.Type;

public class IrodoriReturn<T, TE> where TE : IError
{
    public T Value { get; }
    public TE? Error { get; }

    private IrodoriReturn(T value, TE? error)
    {
        Value = value;
        Error = error;
    }

    /// <summary>
    /// Unwraps the return value, throwing an exception if it contains an error.
    /// </summary>
    /// <returns>The unwrapped value</returns>
    public T Unwrap()
    {
        if (Error != null)
        {
            if (Error is Exception)
            {
                throw (Exception)(object)Error;
            }

            throw new Exception(Error.ToString());
        }
        
        return Value;
    }

    public static IrodoriReturn<T, TE> Success(T value) => new(value, default);
    public static IrodoriReturn<T, TE> Failure(TE? error) => new(default, error);
}