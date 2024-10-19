using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Friendbook.Business.Exceptions.LoginRegisterExceptions
{
    public class PasswordsDoNotMatchException :Exception
    {
        public int StatusCode { get; set; }
        public string PropertyName { get; set; }
        public PasswordsDoNotMatchException()
        {
        }

        public PasswordsDoNotMatchException(string? message) : base(message)
        {
        }
        public PasswordsDoNotMatchException(int statusCode, string propertyName, string? message) : base(message)
        {
            StatusCode = statusCode;
            PropertyName = propertyName;
        }
    }
}
