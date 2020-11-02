using System;

namespace IntellaLend.ExceptionHandler
{
    public class AngularWebException : Exception
    {

        private string CustomerStackTrace;

        public AngularWebException() { }

        public AngularWebException(string message)
            : base(message)
        { }

        public AngularWebException(string message, string stackTrace)
            : base(message)
        {
            this.CustomerStackTrace = stackTrace;
        }

        public AngularWebException(string message, Exception StackTrace)
            : base(message, StackTrace)
        { }

        public override string StackTrace
        {
            get
            {
                return this.CustomerStackTrace;
            }
        }
    }
}
