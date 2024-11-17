using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Friendbook.Business.Exceptions.LoginRegisterExceptions
{
   

    public class InvalidEmailFormatException : Exception
    {
        public int StatusCode { get; }
        public string Field { get; }

        public InvalidEmailFormatException(int statusCode, string field, string message) : base(message)
        {
            StatusCode = statusCode;
            Field = field;
        }
    }

    public class WeakPasswordException : Exception
    {
        public int StatusCode { get; }
        public string Field { get; }

        public WeakPasswordException(int statusCode, string field, string message) : base(message)
        {
            StatusCode = statusCode;
            Field = field;
        }
    }

    public class UserCreationException : Exception
    {
        public int StatusCode { get; }
        public string Field { get; }

        public UserCreationException(int statusCode, string field, string message) : base(message)
        {
            StatusCode = statusCode;
            Field = field;
        }
    }

}
