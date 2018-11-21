using System;
using System.Runtime.Serialization;

namespace App.Web.Controllers
{
    [Serializable]
    internal class HttpNotFoundException : Exception
    {
        public HttpNotFoundException()
        {
        }

        public HttpNotFoundException(string message) : base(message)
        {
        }

        public HttpNotFoundException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected HttpNotFoundException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}