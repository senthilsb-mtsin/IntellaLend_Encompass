using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace MTSEntBlocks.ExceptionBlock
{
    public class UIException : MTSException, ISerializable
    {
        public UIException()
            : base()
        {
            
        }

        public UIException(string message)
            : base(message)
        {
            
        }

        public UIException(string message, System.Exception inner)
            : base(message, inner)
        {
            
        }

        protected UIException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
            
        }
    }
}
