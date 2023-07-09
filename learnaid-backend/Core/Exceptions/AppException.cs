using System.Net;

namespace learnaid_backend.Core.Exceptions
{
    public class AppException : Exception
    {
        public Guid GUID { get; }
        public HttpStatusCode StatusCode { get; }

        public AppException(string message, HttpStatusCode statusCode)
            : base(message)
        {
            GUID = Guid.NewGuid();
            StatusCode = statusCode;
        }
    }
}
