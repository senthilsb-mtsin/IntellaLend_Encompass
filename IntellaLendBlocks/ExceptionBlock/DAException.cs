using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace MTSEntBlocks.ExceptionBlock
{
    public class DAException : MTSException, ISerializable
    {

        public DAException()
            : base()
        {

        }

        public DAException(string message)
            : base(message)
        {

        }

        public DAException(string message, Exception innerException)
            : base(message, innerException)
        {

        }

        protected DAException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {

        }
    }
}
