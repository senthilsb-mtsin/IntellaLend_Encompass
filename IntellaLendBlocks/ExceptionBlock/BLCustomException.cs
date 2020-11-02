using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace MTSEntBlocks.ExceptionBlock
{
    public class BLCustomException : MTSException, ISerializable
    {

        public BLCustomException()
            : base()
        {

        }

        public BLCustomException(string message)
            : base(message)
        {

        }

        public BLCustomException(string message, Exception innerException)
            : base(message, innerException)
        {

        }

        protected BLCustomException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {

        }
    }
}
