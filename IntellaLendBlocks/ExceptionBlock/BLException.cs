using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace MTSEntBlocks.ExceptionBlock
{
    public class BLException : MTSException, ISerializable
    {

        public BLException()
            : base()
        {

        }

        public BLException(string message)
            : base(message)
        {

        }

        public BLException(string message, Exception innerException)
            : base(message, innerException)
        {

        }

        protected BLException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {

        }
    }
}
