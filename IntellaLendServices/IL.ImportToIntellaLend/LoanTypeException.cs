using System;

namespace IL.ImportToIntellaLend
{
    public class LoanTypeNotFoundException : Exception
    {

        public LoanTypeNotFoundException()
        { }

        public LoanTypeNotFoundException(string message)
            : base(message)
        { }

        public LoanTypeNotFoundException(string message, Exception StackTrace)
            : base(message, StackTrace)
        { }
    }
}
