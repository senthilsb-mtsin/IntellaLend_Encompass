using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace MTSEntBlocks.ExceptionBlock
{
    public class DACustomException : MTSException, ISerializable
    {

        public DACustomException()
            : base()
        {

        }

        public DACustomException(string message)
            : base(message)
        {

        }

        public DACustomException(string message, Exception innerException)
            : base(message, innerException)
        {

        }

        protected DACustomException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {

        }
    }
}
