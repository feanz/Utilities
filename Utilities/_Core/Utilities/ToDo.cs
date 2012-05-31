using System;
using System.Diagnostics;

namespace Utilities
{
    /// <summary>
    ///   Class to represent "TODO" blocks which need some action. Either "optimzation", "implementation", "bugfix", "workaround".
    /// </summary>
    public class ToDo
    {
        #region Priority enum

        /// <summary>
        ///   Priority for ToDo items.
        /// </summary>
        public enum Priority
        {
            /// <summary>
            ///   Low prioroty for ToDo
            /// </summary>
            Low,

            /// <summary>
            ///   Normal priority
            /// </summary>
            Normal,

            /// <summary>
            ///   High priority
            /// </summary>
            High,

            /// <summary>
            ///   Critical priority.
            /// </summary>
            Critical
        };

        #endregion

        private static LamdaLogger _logger = new LamdaLogger();

        /// <summary>
        ///   Initialize logging lamda.
        /// </summary>
        public static LamdaLogger Logger
        {
            get { return _logger; }
            set { _logger = value; }
        }

        /// <summary>
        ///   Whether or not to log the ToDo items.
        /// </summary>
        public static bool IsLoggingEnabled { get; set; }

        /// <summary>
        ///   Logs the specified action as an area for a bugfix
        /// </summary>
        /// <param name="priority"> The priority. </param>
        /// <param name="author"> The author. </param>
        /// <param name="description"> The description. </param>
        /// <param name="action"> The action. </param>
        public static void BugFix(Priority priority, string author, string description, Action action)
        {
            Do("BugFix Needed", priority, author, description, action);
        }

        /// <summary>
        ///   Logs the specified action as a code review
        /// </summary>
        /// <param name="priority"> The priority. </param>
        /// <param name="author"> The author. </param>
        /// <param name="description"> The description. </param>
        /// <param name="action"> The action. </param>
        public static void CodeReview(Priority priority, string author, string description, Action action)
        {
            Do("CodeReview Needed", priority, author, description, action);
        }

        /// <summary>
        ///   Implementation the specified action.
        /// </summary>
        /// <param name="priority"> The priority. </param>
        /// <param name="author"> The author. </param>
        /// <param name="description"> The description. </param>
        /// <param name="action"> The action. </param>
        public static void Implement(Priority priority, string author, string description, Action action)
        {
            Do("Implementation Needed", priority, author, description, action);
        }

        /// <summary>
        ///   Logs the specified action for optimization.
        /// </summary>
        /// <param name="priority"> The priority. </param>
        /// <param name="author"> The author. </param>
        /// <param name="description"> The description. </param>
        /// <param name="action"> The action. </param>
        public static void Optimize(Priority priority, string author, string description, Action action)
        {
            Do("Optimization Needed", priority, author, description, action);
        }

        /// <summary>
        ///   Logs the specified action as an area for refactoring.
        /// </summary>
        /// <param name="priority"> The priority. </param>
        /// <param name="author"> The author. </param>
        /// <param name="description"> The description. </param>
        /// <param name="action"> The action. </param>
        public static void Refactor(Priority priority, string author, string description, Action action)
        {
            Do("Refactor Needed", priority, author, description, action);
        }

        /// <summary>
        ///   Logs the specified action as a workaround
        /// </summary>
        /// <param name="priority"> The priority. </param>
        /// <param name="author"> The author. </param>
        /// <param name="description"> The description. </param>
        /// <param name="action"> The action. </param>
        public static void WorkAround(Priority priority, string author, string description, Action action)
        {
            Do("Workaround performed", priority, author, description, action);
        }

        /// <summary>
        ///   Does the specified action while logging contextual information.
        /// </summary>
        /// <param name="tag"> The tag. </param>
        /// <param name="priority"> The priority. </param>
        /// <param name="author"> The author. </param>
        /// <param name="description"> The description. </param>
        /// <param name="action"> The action. </param>
        private static void Do(string tag, Priority priority, string author, string description, Action action)
        {
            if (IsLoggingEnabled)
            {
                var stackTrace = new StackTrace();
                string methodName = stackTrace.GetFrame(2).GetMethod().Name;
                string className = stackTrace.GetFrame(2).GetMethod().DeclaringType.FullName;
                int lineNumber = stackTrace.GetFrame(2).GetFileLineNumber();
                string fileName = stackTrace.GetFrame(2).GetFileName();
                string format = "{0} - Priority: {1}, Author: {2}, Description: {3}" + Environment.NewLine
                                + "At {4}.{5}, File: {6}, Line{7}" + Environment.NewLine;
                string message = string.Format(format, tag, priority.ToString(), author, description, className,
                                               methodName, fileName, lineNumber);
                _logger.Info(message);
            }
            // Now Run the action.
            action();
        }
    }
}