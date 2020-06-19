using System;
using System.Collections.Generic;
using System.Text;

namespace SecretNest.RemoteAgency
{
    /// <summary>
    /// Represents an redirected exception thrown from user code.
    /// </summary>
    public class ExceptionRedirectedEventArgs
    {
        /// <summary>
        /// Gets the exception thrown.
        /// </summary>
        public Exception RedirectedException { get; }

        /// <summary>
        /// Initializes an instance of ExceptionRedirectedEventArgs.
        /// </summary>
        /// <param name="exception">Exception thrown.</param>
        public ExceptionRedirectedEventArgs(Exception exception)
        {
            RedirectedException = exception;
        }
    }
}
