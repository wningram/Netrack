using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using System.Drawing;

namespace NetrackServer {
    public class Common {

        [Serializable]
        public class DeserializationFailedException : Exception {
            public DeserializationFailedException() { }
            public DeserializationFailedException(string message) : base(message) { }
            public DeserializationFailedException(string message, Exception inner) : base(message, inner) { }
            protected DeserializationFailedException(
              System.Runtime.Serialization.SerializationInfo info,
              System.Runtime.Serialization.StreamingContext context) : base(info, context) { }
        }

        /// <summary>
        /// Converts a string in the following format to a <see cref="System.Drawing.Point"/>: ({x:int}, {y:int})
        /// </summary>
        /// <param name="data">The string to convert to a Point.</param>
        /// <returns>A <see cref="Point"/> representation of <paramref name="data"/>.</returns>
        public static Point DeserializePoint(string data) {
            int x = 0, y = 0;
            string[] splitData = data.Split(',');
            bool ysuccess = false, xsuccess = false;

            if (splitData.Length == 2) {
                string left = splitData[0],
                    right = splitData[1];
                // Get X
                xsuccess = int.TryParse(left.Trim('('), out x);
                // Get Y
                ysuccess = int.TryParse(right.Trim(')'), out y);
            }

            if (xsuccess && ysuccess) {
                return new Point(x, y);
            } else {
                throw new DeserializationFailedException($"Could not convert point: {data}");
            }
        }
    }
}
