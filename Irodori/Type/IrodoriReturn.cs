using Irodori.Error;

namespace Irodori.Type;

/** Some state(or errors) may not need actual return values. */
public class IrodoriState
{
    public IState Error;
    protected IrodoriState(IState error)
    {
        Error = error;
    }

    public bool IsError() { return Error is IError; }

    public void Unwrap()
    {
        if (Error is IError)
        {
            if (Error is Exception e)
            {
                throw e;
            }

            throw new Exception(Error.ToString());
        }
    }

    public static IrodoriState Success()
    {
        return new IrodoriState(new ISuccess());
    }
    public static IrodoriState Failure(IError error) => new(error);
    public static IrodoriState NotSure(IState state) => new(state);
}

public class IrodoriReturn<T> : IrodoriState
{

    public T? Value;

    private IrodoriReturn(IState error, T? val) : base(error)
    {
        Value = val;
    }

    /// <summary>
    /// Unwraps the return value, throwing an exception if it contains an error.
    /// </summary>
    /// <returns>The unwrapped value</returns>
    public new T Unwrap()
    {
        base.Unwrap();
        if (this.Value == null) throw new NullReferenceException();
        return this.Value;
    }

    /** This one forgives null. */
    public T? UnwrapWithForgivingNull()
    {
        base.Unwrap();
        return this.Value;
    }

    public static IrodoriReturn<T> Success(T value)
    {
        return new IrodoriReturn<T>(new ISuccess(), value);
    }
    public new static IrodoriReturn<T> Failure(IError error) => new(error, default);
    public new static IrodoriReturn<T> NotSure(IState state) => new(state, default);
}