using System;

namespace EmbeddedServer
{
    public class ErrorData
    {
        public ErrorData()
        {
        }
        public ErrorData(string message)
        {
            Message = message;
        }
        public ErrorData(Exception exception)
        {
            Exception = exception;
        }
        public ErrorData(string message, Exception exception)
        {
            Message = message;
            Exception = exception;
        }
        public string Message { get; set; }
        public Exception Exception { get; set; }
    }
}
