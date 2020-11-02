using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace MTSEntBlocks.ExceptionBlock
{
    public class PassThruException : MTSException, ISerializable
    {

        public PassThruException()
            : base()
        {

        }

        public PassThruException(string message)
            : base(message)
        {

        }

        public PassThruException(string message, Exception innerException)
            : base(message, innerException)
        {

        }

        protected PassThruException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {

        }
    }
}
