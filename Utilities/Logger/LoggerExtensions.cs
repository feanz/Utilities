using System;
using System.Collections.Concurrent;

namespace Utilities.Logger
{
	/// <summary>
	/// Extensions to help make logging awesome
	/// </summary>
	public static class LoggerExtensions
	{
		/// <summary>
		/// Concurrent dictionary that ensures only one instance of a logger for a type.
		/// </summary>
		private static readonly Lazy<ConcurrentDictionary<string, ILog>> _dictionary = new Lazy<ConcurrentDictionary<string, ILog>>(() => new ConcurrentDictionary<string, ILog>());

		/// <summary>
		/// Gets the logger for <see cref="T"/>.
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="type">The type to get the logger for.</param>
		/// <returns>Instance of a logger for the object.</returns>
		public static ILog Log<T>(this T type)
		{
			var objectName = typeof(T).FullName;
			return Log(objectName);
		}

		/// <summary>
		/// Gets the logger for the specified object name.
		/// </summary>
		/// <param name="objectName">Either use the fully qualified object name or the short. If used with Log&lt;T&gt;() you must use the fully qualified object name"/></param>
		/// <returns>Instance of a logger for the object.</returns>
		public static ILog Log(this string objectName)
		{
			return _dictionary.Value.GetOrAdd(objectName, Logger.Log.GetLoggerFor(objectName));
		}
	}
}