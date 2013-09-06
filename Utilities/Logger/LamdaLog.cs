using System;

namespace Utilities.Logger
{
	/// <summary>
	///     Logger using lamda method to call external logging code. This decouples code from using a specific ILogging
	///     interface via lamdas.
	/// </summary>
	public class LamdaLog : ILog
	{
		private static Action<object> _debugLogger;
		private static Action<object, Exception> _errorLogger;
		private static Action<object, Exception> _fatalLogger;
		private static Action<object> _infoLogger;
		private static Action<object> _warnLogger;

		/// <summary>
		///     Initialize the different level lamda loggers.
		/// </summary>
		/// <param name="infoLogger"> Information logger. </param>
		/// <param name="warnLogger">Warn Logger</param>
		/// <param name="debugLogger"> Debug logger. </param>
		/// <param name="errorLogger"> Error logger. </param>
		/// <param name="fatalLogger">Fatal Logger</param>
		public void Init(Action<object> infoLogger,
			Action<object> warnLogger,
			Action<object> debugLogger,
			Action<object, Exception> errorLogger,
			Action<object, Exception> fatalLogger)
		{
			if (infoLogger != null) _infoLogger = infoLogger;
			if (warnLogger != null) _warnLogger = warnLogger;
			if (debugLogger != null) _debugLogger = debugLogger;
			if (errorLogger != null) _errorLogger = errorLogger;
			if (fatalLogger != null) _fatalLogger = fatalLogger;
		}

		public void Debug(string message, params object[] formatting)
		{
			_debugLogger.Invoke(string.Format(message, formatting));
		}

		public void Debug(Func<string> message)
		{
			_debugLogger.Invoke(message.Invoke());
		}

		public void Error(string message, params object[] formatting)
		{
			_errorLogger.Invoke(string.Format(message, formatting), null);
		}

		public void Error(Func<string> message)
		{
			_errorLogger.Invoke(message, null);
		}

		public void Error(string message, Exception exception)
		{
			_errorLogger.Invoke(message, exception);
		}

		public void Fatal(string message, params object[] formatting)
		{
			_fatalLogger.Invoke(message, null);
		}

		public void Fatal(Func<string> message)
		{
			_fatalLogger.Invoke(message.Invoke(), null);
		}

		public void Fatal(string message, Exception exception)
		{
			_fatalLogger.Invoke(message, exception);
		}

		public void Info(string message, params object[] formatting)
		{
			_infoLogger.Invoke(string.Format(message, formatting));
		}

		public void Info(Func<string> message)
		{
			_infoLogger.Invoke(message.Invoke());
		}

		public void InitializeFor(string loggerName)
		{
			// Default logger to console.
			_infoLogger = (message) => Console.WriteLine(BuildMessage("info", message));
			_errorLogger = (message, ex) => Console.WriteLine(BuildMessage("error", message, ex));
			_fatalLogger = (message, ex) => Console.WriteLine(BuildMessage("fatal", message, ex));
			_debugLogger = (message) => Console.WriteLine(BuildMessage("debug", message));
			_warnLogger = (message) => Console.WriteLine(BuildMessage("warn", message));
		}

		public void Warn(string message, params object[] formatting)
		{
			_warnLogger.Invoke(string.Format(message, formatting));
		}

		public void Warn(Func<string> message)
		{
			_warnLogger.Invoke(message.Invoke());
		}

		private static string BuildMessage(string level, object message, Exception ex = null)
		{
			var finalMessage = level.ToUpper() + " : " + message;

			if (ex != null)
			{
				finalMessage += Environment.NewLine
				                + ex.Message + Environment.NewLine
				                + ex.StackTrace + Environment.NewLine;
			}
			return finalMessage;
		}
	}
}