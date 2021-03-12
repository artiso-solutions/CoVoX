using System;

namespace Covox
{
    public delegate void ErrorHandler(Exception ex);

    public interface IExposeErrors
    {
        event ErrorHandler OnError;
    }
}
