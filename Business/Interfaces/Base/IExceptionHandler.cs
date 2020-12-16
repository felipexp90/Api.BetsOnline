using System;

namespace Business.Interfaces.Base
{
    public interface IExceptionHandler
    {
        string GetMessage(Exception ex);
    }
}
