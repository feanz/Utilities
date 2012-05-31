using System;

namespace Utilities
{
    /// <summary>
    ///   Wrapper class to simplify lines of code around Try/Catch blocks with various customized behaviour.
    /// </summary>
    public class Try
    {
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
        ///   Executes an action inside a try catch and logs any exceptions.
        /// </summary>
        /// <param name="action"> The function to call. </param>
        /// <param name="exceptionHandler"> The action to use for handling the exception </param>
        public static void Catch(Action action, Action<Exception> exceptionHandler = null)
        {
            try
            {
                action();
            }
            catch (Exception ex)
            {
                if (exceptionHandler != null)
                    exceptionHandler(ex);
            }
        }

        /// <summary>
        ///   Executes an action inside a try catch and logs any exceptions.
        /// </summary>
        /// <param name="errorMessage"> Error errorMessage to log. </param>
        /// <param name="action"> The function to call. </param>
        /// <param name="exceptionHandler"> The action to use for handling the exception </param>
        /// <param name="finallyHandler"> The action to use in the finally block </param>
        public static void CatchHandle(string errorMessage, Action action, Action<Exception> exceptionHandler,
                                       Action finallyHandler)
        {
            try
            {
                action();
            }
            catch (Exception ex)
            {
                if (exceptionHandler != null)
                    exceptionHandler(ex);
            }
            finally
            {
                if (finallyHandler != null)
                {
                    finallyHandler();
                }
            }
        }

        /// <summary>
        ///   Executes an action inside a try catch and logs any exceptions.
        /// </summary>
        /// <param name="errorMessage"> Error errorMessage to log. </param>
        /// <param name="action"> The function to call. </param>
        /// <param name="logger"> The logger to use </param>
        public static void CatchLog(string errorMessage, Action action, Action<object, Exception, object[]> logger)
        {
            try
            {
                action();
            }
            catch (Exception ex)
            {
                if (logger != null)
                    logger(errorMessage, ex, null);
            }
        }

        /// <summary>
        ///   Executes an action inside a try catch and logs any exceptions.
        /// </summary>
        /// <param name="errorMessage"> Error errorMessage to log. </param>
        /// <param name="action"> Action to execute. </param>
        public static T CatchLogGet<T>(string errorMessage, Func<T> action)
        {
            return CatchLogGet(errorMessage, false, action, null);
        }

        /// <summary>
        ///   Executes an action inside a try catch and logs any exceptions.
        /// </summary>
        /// <typeparam name="T"> Type of return item. </typeparam>
        /// <param name="errorMessage"> Error errorMessage to log. </param>
        /// <param name="rethrow"> Whether or not to rethrow </param>
        /// <param name="action"> The function to call. </param>
        /// <param name="logger"> The logger to use </param>
        /// <returns> Returned result. </returns>
        public static T CatchLogGet<T>(string errorMessage, bool rethrow, Func<T> action,
                                       Action<object, Exception, object[]> logger)
        {
            T result = default(T);
            try
            {
                result = action();
            }
            catch (Exception ex)
            {
                if (logger != null)
                    logger(errorMessage, ex, null);
                else if (_logger != null)
                    _logger.Error(errorMessage, ex);

                if (rethrow) throw;
            }
            return result;
        }

        /// <summary>
        ///   Get a boolmessage item after running the action.
        /// </summary>
        /// <param name="errorMessage"> Error errorMessage to log. </param>
        /// <param name="action"> The function to call. </param>
        /// <returns> Returned result. </returns>
        public static BoolMessageItem<T> CatchLogGetBoolResult<T>(string errorMessage, Func<T> action)
        {
            return CatchLogGetBoolResult(errorMessage, action, null);
        }

        /// <summary>
        ///   Executes an action inside a try catch and logs any exceptions.
        /// </summary>
        /// <typeparam name="T"> Type of return item. </typeparam>
        /// <param name="errorMessage"> Error errorMessage to log. </param>
        /// <param name="action"> The function to call. </param>
        /// <param name="logger"> Logger object. </param>
        /// <returns> Return item. </returns>
        public static BoolMessageItem<T> CatchLogGetBoolResult<T>(string errorMessage, Func<T> action,
                                                                  Action<object, Exception, object[]> logger)
        {
            T result = default(T);
            bool success = false;
            string message = string.Empty;
            try
            {
                result = action();
                success = true;
            }
            catch (Exception ex)
            {
                if (logger != null)
                    logger(errorMessage, ex, null);
                else if (_logger != null)
                    _logger.Error(errorMessage, ex);

                message = ex.Message;
            }
            return new BoolMessageItem<T>(result, success, message);
        }

        /// <summary>
        ///   Runs the action, logs an error and returns boolMessageEx that wraps up all there values. Bool flag for successful call without excpetion, errorMessage if there was an exception, and the exception itself if it occurred.
        /// </summary>
        /// <param name="message"> Success errorMessage </param>
        /// <param name="errorMessage"> Failure errorMessage </param>
        /// <param name="action"> Action to run </param>
        /// <returns> Error errorMessage. </returns>
        public static BoolMessageEx CatchLogGetBoolResultEx(string message, string errorMessage, Action action)
        {
            string finalMessage = message;
            bool success;
            Exception ex = null;
            try
            {
                action();
                success = true;
            }
            catch (Exception exception)
            {
                success = false;
                finalMessage = errorMessage;
                ex = exception;
                _logger.Error(ex);
            }
            return new BoolMessageEx(success, ex, finalMessage);
        }

        /// <summary>
        ///   Runs the action, logs an error and returns boolMessageEx that wraps up all there values. Bool flag for successful call without excpetion, errorMessage if there was an exception, and the exception itself if it occurred.
        /// </summary>
        /// <param name="message"> Success errorMessage </param>
        /// <param name="errorMessage"> Failure errorMessage </param>
        /// <param name="action"> Action to run </param>
        /// <returns> Error errorMessage. </returns>
        public static BoolMessageEx CatchLogReturnBoolResultEx(string message, string errorMessage,
                                                               Func<BoolMessageEx> action)
        {
            string finalMessage;
            bool success;
            Exception ex;
            try
            {
                BoolMessageEx result = action();
                ex = result.Ex;
                success = result.Success;
                finalMessage = result.Message;
            }
            catch (Exception exception)
            {
                success = false;
                ex = exception;
                finalMessage = errorMessage;
                _logger.Error(exception);
            }
            return new BoolMessageEx(success, ex, finalMessage);
        }

        /// <summary>
        ///   Runs the action, logs an error and returns boolMessageEx that wraps up all there values. Bool flag for successful call without excpetion, errorMessage if there was an exception, and the exception itself if it occurred.
        /// </summary>
        /// <param name="message"> Success errorMessage </param>
        /// <param name="errorMessage"> Failure errorMessage </param>
        /// <param name="action"> Action to run </param>
        /// <returns> Error errorMessage. </returns>
        public static BoolMessageEx GetBoolResultEx(string message, string errorMessage, Func<BoolMessageEx> action)
        {
            BoolMessageEx result = action();
            string finalmessage = message;
            if (!result.Success)
                finalmessage = string.IsNullOrEmpty(errorMessage) ? result.Message : errorMessage;

            return new BoolMessageEx(result.Success, result.Ex, finalmessage);
        }

        /// <summary>
        ///   Handle the highest level application exception.
        /// </summary>
        /// <param name="ex"> Exception to handle. </param>
        public static void HandleException(Exception ex)
        {
            string message = string.Format("{0}, {1} \r\n {2}", ex.Message, ex.Source, ex.StackTrace);
            if (_logger != null)
                _logger.Error(message, ex);
        }

        /// <summary>
        ///   Executes an action inside a try catch and logs any exceptions.
        /// </summary>
        /// <param name="errorMessage"> Error errorMessage to log. </param>
        /// <param name="action"> The function to call. </param>
        /// <param name="logger"> Logger object. </param>
        /// <returns> Return item. </returns>
        public static BoolMessageItem TryCatchLogGetBoolMessageItem(string errorMessage, Func<BoolMessageItem> action,
                                                                    Action<object, Exception, object[]> logger)
        {
            BoolMessageItem result;
            try
            {
                result = action();
            }
            catch (Exception ex)
            {
                if (logger != null)
                    logger(errorMessage, ex, null);
                else if (_logger != null)
                    _logger.Error(errorMessage, ex);

                string fullMessage = errorMessage + Environment.NewLine
                                     + ex.Message + Environment.NewLine
                                     + ex.Source + Environment.NewLine
                                     + ex.StackTrace;
                result = new BoolMessageItem(null, false, fullMessage);
            }
            return result;
        }
    }
}