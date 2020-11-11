using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Power4Lib
{

    [Serializable]
    public class LocationOutsideGridException : Exception
    {
        public LocationOutsideGridException() { }
        public LocationOutsideGridException(string message) : base(message) { }
        public LocationOutsideGridException(string message, Exception inner) : base(message, inner) { }
        protected LocationOutsideGridException(
          System.Runtime.Serialization.SerializationInfo info,
          System.Runtime.Serialization.StreamingContext context) : base(info, context) { }
    }


    [Serializable]
    public class FullColumnException : Exception
    {
        public int ColumnLocation { get; set; }

        public FullColumnException(int columnNumber)
        {
            ColumnLocation = columnNumber;
        }
        public FullColumnException() { }
        public FullColumnException(string message) : base(message) { }
        public FullColumnException(string message, Exception inner) : base(message, inner) { }
        protected FullColumnException(
          System.Runtime.Serialization.SerializationInfo info,
          System.Runtime.Serialization.StreamingContext context) : base(info, context) { }
    }
}
