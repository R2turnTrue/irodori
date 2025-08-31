
namespace Irodori.Error;

public interface IState{}

/** Is error value */
public interface IError : IState { }

public class ISuccess : IState
{
    public ISuccess() { }
}


public class GeneralNullExceptionError : IError
{

}