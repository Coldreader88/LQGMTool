using System;

#if !NETCF_1_0 && !NETCF_2_0
using System.Runtime.Serialization;
#endif

namespace ICSharpCode.SharpZipLib.Hfs
{

    /// <summary>
    /// Represents exception conditions specific to Hfs archive handling
    /// </summary>
#if !NETCF_1_0 && !NETCF_2_0
    [Serializable]
#endif
    public class HfsException : SharpZipBaseException
    {
#if !NETCF_1_0 && !NETCF_2_0
        /// <summary>
        /// Deserialization constructor 
        /// </summary>
        /// <param name="info"><see cref="SerializationInfo"/> for this constructor</param>
        /// <param name="context"><see cref="StreamingContext"/> for this constructor</param>
        protected HfsException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }
#endif

        /// <summary>
        /// Initializes a new instance of the HfsException class.
        /// </summary>
        public HfsException()
        {
        }

        /// <summary>
        /// Initializes a new instance of the HfsException class with a specified error message.
        /// </summary>
        /// <param name="message">The error message that explains the reason for the exception.</param>
        public HfsException(string message)
            : base(message)
        {
        }

        /// <summary>
        /// Initialise a new instance of HfsException.
        /// </summary>
        /// <param name="message">A message describing the error.</param>
        /// <param name="exception">The exception that is the cause of the current exception.</param>
        public HfsException(string message, Exception exception)
            : base(message, exception)
        {
        }
    }
}
