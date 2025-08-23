using Irodori.Error;

namespace Irodori.Windowing.Sdl2;

public class SdlGetProcAddressFailedException(string msg) : Exception(msg), IProcAddressError
{
}