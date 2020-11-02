using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace MTSEntBlocks.ExceptionBlock
{
    public class MTSException : System.Exception, ISerializable
    {

        public MTSException()
            : base()
        {

        }

        public MTSException(string message)
            : base(message)
        {

        }

        public MTSException(string message, Exception innerException)
            : base(message, innerException)
        {

        }

        protected MTSException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {

        }
    }
}
