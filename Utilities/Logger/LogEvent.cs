using System;

namespace Utilities.Logger
{
    /// <summary>
    ///   A record in the log.
    /// </summary>
    public class LogEvent
    {
        /// <summary>
        ///   Additional arguments passed by caller.
        /// </summary>
        public object[] Args;

        /// <summary>
        ///   Name of the computer.
        /// </summary>
        public string Computer;

        /// <summary>
        ///   Create time.
        /// </summary>
        public DateTime CreateTime;

        /// <summary>
        ///   Exception passed.
        /// </summary>
        public Exception Error;

        /// <summary>
        ///   The exception.
        /// </summary>
        public Exception Ex;

        /// <summary>
        ///   This is the final errorMessage that is printed.
        /// </summary>
        public string FinalMessage;

        /// <summary>
        ///   The log level.
        /// </summary>
        public LogLevel Level;

        /// <summary>
        ///   The data type of the caller that is logging the event.
        /// </summary>
        public Type LogType;

        /// <summary>
        ///   Message that is logged.
        /// </summary>
        public object Message;

        /// <summary>
        ///   The name of the currently executing thread that created this log entry.
        /// </summary>
        public string ThreadName;

        /// <summary>
        ///   Name of the user.
        /// </summary>
        public string UserName;

        /// <summary>
        ///   Enable default constructor.
        /// </summary>
        public LogEvent()
        {
        }

        /// <summary>
        ///   Initialize log event using loglevel, errorMessage and exception
        /// </summary>
        /// <param name="level"> Event log level. </param>
        /// <param name="errorMessage"> Log errorMessage. </param>
        /// <param name="ex"> Exception to log. </param>
        public LogEvent(LogLevel level, string message, Exception ex)
        {
            Level = level;
            Message = message;
            Ex = ex;
        }
    }
}