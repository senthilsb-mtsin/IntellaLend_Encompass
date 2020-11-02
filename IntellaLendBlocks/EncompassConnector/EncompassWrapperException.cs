using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EncompassConnector
{
    public class EncompassWrapperException : Exception
    {
        public EncompassWrapperException() { }

        public EncompassWrapperException(string message) : base(message)
        { }

        public EncompassWrapperException(string message, Exception innerException) : base(message, innerException)
        { }
    }

    public class EncompassWrapperLoginFailedException : Exception
    {
        public EncompassWrapperLoginFailedException() { }

        public EncompassWrapperLoginFailedException(string message) : base(message)
        { }

        public EncompassWrapperLoginFailedException(string message, Exception innerException) : base(message, innerException)
        { }
    }
}
