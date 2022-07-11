using System.Runtime.Serialization;

namespace tutoring_online_be.Security;

public class AppException
{
    public class ValidationFailException : Exception
    {
        public ValidationFailException()
        {
        }

        protected ValidationFailException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }

        public ValidationFailException(string? message) : base(message)
        {
        }

        public ValidationFailException(string? message, Exception? innerException) : base(message, innerException)
        {
        }
    }

    public class OtherErrorException : Exception
    {
        public OtherErrorException()
        {
        }

        protected OtherErrorException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }

        public OtherErrorException(string? message) : base(message)
        {
        }

        public OtherErrorException(string? message, Exception? innerException) : base(message, innerException)
        {
        }
    }
    
}