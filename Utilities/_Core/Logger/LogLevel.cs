namespace Utilities
{
    /// <summary>
    ///   Level for the logging.
    /// </summary>
    public enum LogLevel
    {
        /// <summary>
        ///   Used to always log a errorMessage regardless of loglevel
        /// </summary>
        Message,


        /// <summary>
        ///   Debug level
        /// </summary>
        Debug,


        /// <summary>
        ///   Info level
        /// </summary>
        Info,


        /// <summary>
        ///   Warn level
        /// </summary>
        Warn,


        /// <summary>
        ///   Error level
        /// </summary>
        Error,


        /// <summary>
        ///   Fatal level
        /// </summary>
        Fatal,
    };
}