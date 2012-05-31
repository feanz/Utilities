using System;

namespace Utilities
{
    /// <summary>
    ///   Simple interface for logging information. This extends the common Log4net interface by 1. Taking additional argument as an object array 2. Exposing a simple Log method that takes in the loglevel.
    /// </summary>
    public interface ILog
    {
        /// <summary>
        ///   Get the name of the logger.
        /// </summary>
        string Name { get; }

        /// <summary>
        ///   Get / set the loglevel.
        /// </summary>
        LogLevel Level { get; set; }

        /// <summary>
        ///   Gets a value indicating whether this instance is debug enabled.
        /// </summary>
        /// <value> <c>true</c> if this instance is debug enabled; otherwise, <c>false</c> . </value>
        bool IsDebugEnabled { get; }

        /// <summary>
        ///   Gets a value indicating whether this instance is error enabled.
        /// </summary>
        /// <value> <c>true</c> if this instance is error enabled; otherwise, <c>false</c> . </value>
        bool IsErrorEnabled { get; }

        /// <summary>
        ///   Gets a value indicating whether this instance is fatal enabled.
        /// </summary>
        /// <value> <c>true</c> if this instance is fatal enabled; otherwise, <c>false</c> . </value>
        bool IsFatalEnabled { get; }


        /// <summary>
        ///   Gets a value indicating whether this instance is info enabled.
        /// </summary>
        /// <value> <c>true</c> if this instance is info enabled; otherwise, <c>false</c> . </value>
        bool IsInfoEnabled { get; }

        /// <summary>
        ///   Gets a value indicating whether this instance is warn enabled.
        /// </summary>
        /// <value> <c>true</c> if this instance is warn enabled; otherwise, <c>false</c> . </value>
        bool IsWarnEnabled { get; }

        /// <summary>
        ///   Builds a log event from the parameters supplied.
        /// </summary>
        /// <param name="level"> Level of errorMessage. </param>
        /// <param name="errorMessage"> Message to log. </param>
        /// <param name="ex"> Exception to log. </param>
        /// <param name="args"> Arguments to use. </param>
        /// <returns> Created log event. </returns>
        LogEvent BuildLogEvent(LogLevel level, object message, Exception ex, object[] args);

        /// <summary>
        ///   Logs a Debug errorMessage.
        /// </summary>
        /// <param name="errorMessage"> The errorMessage. </param>
        void Debug(object message);

        /// <summary>
        ///   Logs a Debug errorMessage with exception.
        /// </summary>
        /// <param name="errorMessage"> The errorMessage. </param>
        /// <param name="exception"> The exception. </param>
        void Debug(object message, Exception exception);

        /// <summary>
        ///   Logs a debug errorMessage with the exception and arguments.
        /// </summary>
        /// <param name="errorMessage"> The errorMessage. </param>
        /// <param name="exception"> The exception. </param>
        /// <param name="args"> The args. </param>
        void Debug(object message, Exception exception, object[] args);

        /// <summary>
        ///   Logs a Error errorMessage.
        /// </summary>
        /// <param name="errorMessage"> The errorMessage. </param>
        void Error(object message);

        /// <summary>
        ///   Logs a Error errorMessage with exception.
        /// </summary>
        /// <param name="errorMessage"> The errorMessage. </param>
        /// <param name="exception"> The exception. </param>
        void Error(object message, Exception exception);

        /// <summary>
        ///   Logs an error errorMessage with the exception additional arguments.
        /// </summary>
        /// <param name="errorMessage"> The errorMessage. </param>
        /// <param name="exception"> The exception. </param>
        /// <param name="args"> The args. </param>
        void Error(object message, Exception exception, object[] args);

        /// <summary>
        ///   Logs a Fatal errorMessage.
        /// </summary>
        /// <param name="errorMessage"> The errorMessage. </param>
        void Fatal(object message);

        /// <summary>
        ///   Logs a Fatal errorMessage with exception.
        /// </summary>
        /// <param name="errorMessage"> The errorMessage. </param>
        /// <param name="exception"> The exception. </param>
        void Fatal(object message, Exception exception);

        /// <summary>
        ///   Logs a fatal errorMessage with exception and arguments.
        /// </summary>
        /// <param name="errorMessage"> The errorMessage. </param>
        /// <param name="exception"> The exception. </param>
        /// <param name="args"> The args. </param>
        void Fatal(object message, Exception exception, object[] args);

        /// <summary>
        ///   Flushes the buffers.
        /// </summary>
        void Flush();

        /// <summary>
        ///   Logs a Info errorMessage.
        /// </summary>
        /// <param name="errorMessage"> The errorMessage. </param>
        void Info(object message);

        /// <summary>
        ///   Logs a Info errorMessage with exception.
        /// </summary>
        /// <param name="errorMessage"> The errorMessage. </param>
        /// <param name="exception"> The exception. </param>
        void Info(object message, Exception exception);

        /// <summary>
        ///   Logs a info errorMessage with the arguments.
        /// </summary>
        /// <param name="errorMessage"> The errorMessage. </param>
        /// <param name="exception"> The exception. </param>
        /// <param name="args"> The args. </param>
        void Info(object message, Exception exception, object[] args);

        /// <summary>
        ///   Is the level enabled.
        /// </summary>
        /// <param name="level"> Logging level to check for. </param>
        /// <returns> True if specified logging level is enabled. </returns>
        bool IsEnabled(LogLevel level);

        /// <summary>
        ///   Logs the specified level.
        /// </summary>
        void Log(LogEvent logEvent);

        /// <summary>
        ///   Logs a Message.
        /// </summary>
        /// <param name="level"> Logging level of errorMessage. </param>
        /// <param name="errorMessage"> The errorMessage. </param>
        void Log(LogLevel level, object message);

        /// <summary>
        ///   Logs a Message with exception.
        /// </summary>
        /// <param name="level"> Logging level of errorMessage. </param>
        /// <param name="errorMessage"> The errorMessage. </param>
        /// <param name="exception"> The exception. </param>
        void Log(LogLevel level, object message, Exception exception);

        /// <summary>
        ///   Messages should always get logged.
        /// </summary>
        /// <param name="level"> Logging level of errorMessage. </param>
        /// <param name="errorMessage"> The errorMessage. </param>
        /// <param name="ex"> The exception. </param>
        /// <param name="args"> The args. </param>
        void Log(LogLevel level, object message, Exception ex, object[] args);

        /// <summary>
        ///   Logs a Message.
        /// </summary>
        /// <param name="errorMessage"> The errorMessage. </param>
        void Message(object message);

        /// <summary>
        ///   Logs a Message with exception.
        /// </summary>
        /// <param name="errorMessage"> The errorMessage. </param>
        /// <param name="exception"> The exception. </param>
        void Message(object message, Exception exception);

        /// <summary>
        ///   Messages should always get logged.
        /// </summary>
        /// <param name="errorMessage"> The errorMessage. </param>
        /// <param name="exception"> The exception. </param>
        /// <param name="args"> The args. </param>
        void Message(object message, Exception exception, object[] args);


        /// <summary>
        ///   Shutdown the logger.
        /// </summary>
        void ShutDown();

        /// <summary>
        ///   Logs a warning errorMessage.
        /// </summary>
        /// <param name="errorMessage"> The errorMessage. </param>
        void Warn(object message);

        /// <summary>
        ///   Logs a warning errorMessage with exception.
        /// </summary>
        /// <param name="errorMessage"> The errorMessage. </param>
        /// <param name="exception"> The exception. </param>
        void Warn(object message, Exception exception);

        /// <summary>
        ///   Logs a warning errorMessage with exception and additional arguments.
        /// </summary>
        /// <param name="errorMessage"> The errorMessage. </param>
        /// <param name="exception"> The exception. </param>
        /// <param name="args"> Additional arguments. </param>
        void Warn(object message, Exception exception, object[] args);
    }
}