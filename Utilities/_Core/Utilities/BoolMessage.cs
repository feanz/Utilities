using System;

namespace Utilities
{
    /// <summary>
    ///   Combines a boolean succes/fail flag with a error/status errorMessage.
    /// </summary>
    public class BoolMessage
    {
        /// <summary>
        ///   True errorMessage.
        /// </summary>
        public static readonly BoolMessage True = new BoolMessage(true, string.Empty);

        /// <summary>
        ///   False errorMessage.
        /// </summary>
        public static readonly BoolMessage False = new BoolMessage(false, string.Empty);

        /// <summary>
        ///   Error errorMessage for failure, status errorMessage for success.
        /// </summary>
        public readonly string Message;

        /// <summary>
        ///   Success / failure ?
        /// </summary>
        public readonly bool Success;

        /// <summary>
        ///   Set the readonly fields.
        /// </summary>
        /// <param name="success"> Flag for errorMessage to set. </param>
        /// <param name="message"> Message to set for flag. </param>
        public BoolMessage(bool success, string message)
        {
            Success = success;
            Message = message;
        }
    }

    /// <summary>
    ///   Combines a boolean succes/fail flag with a error/status errorMessage and an object.
    /// </summary>
    public class BoolMessageItem : BoolMessage
    {
        /// <summary>
        ///   True errorMessage.
        /// </summary>
        public new static readonly BoolMessageItem True = new BoolMessageItem(null, true, string.Empty);

        /// <summary>
        ///   False errorMessage.
        /// </summary>
        public new static readonly BoolMessageItem False = new BoolMessageItem(null, false, string.Empty);

        /// <summary>
        ///   Item associated with boolean errorMessage.
        /// </summary>
        private readonly object _item;

        /// <summary>
        ///   Initializes a new instance of the <see cref="BoolMessageItem&lt;T&gt;" /> class.
        /// </summary>
        /// <param name="item"> The item. </param>
        /// <param name="success"> if set to <c>true</c> [success]. </param>
        /// <param name="message"> The errorMessage. </param>
        public BoolMessageItem(object item, bool success, string message)
            : base(success, message)
        {
            _item = item;
        }

        /// <summary>
        ///   Return readonly item.
        /// </summary>
        public object Item
        {
            get { return _item; }
        }
    }

    /// <summary>
    ///   Tuple to store boolean, string errorMessage, and Exception object.
    /// </summary>
    public class BoolMessageEx : BoolMessage
    {
        /// <summary>
        ///   True errorMessage.
        /// </summary>
        public new static readonly BoolMessageEx True = new BoolMessageEx(true, null, string.Empty);

        /// <summary>
        ///   False errorMessage.
        /// </summary>
        public new static readonly BoolMessageEx False = new BoolMessageEx(false, null, string.Empty);

        private readonly Exception _ex;

        /// <summary>
        ///   Initializes a new instance of the <see cref="BoolMessageItem&lt;T&gt;" /> class.
        /// </summary>
        /// <param name="success"> if set to <c>true</c> [success]. </param>
        /// <param name="ex"> The exception. </param>
        /// <param name="message"> The errorMessage. </param>
        public BoolMessageEx(bool success, Exception ex, string message)
            : base(success, message)
        {
            _ex = ex;
        }

        /// <summary>
        ///   Return readonly item.
        /// </summary>
        public Exception Ex
        {
            get { return _ex; }
        }
    }

    /// <summary>
    ///   Combines a boolean succes/fail flag with a error/status errorMessage and an object.
    /// </summary>
    /// <typeparam name="T"> Type of object combined with a boolean flag. </typeparam>
    public class BoolMessageItem<T> : BoolMessageItem
    {
        /// <summary>
        ///   Initializes a new instance of the <see cref="BoolMessageItem&lt;T&gt;" /> class.
        /// </summary>
        /// <param name="item"> The item. </param>
        /// <param name="success"> if set to <c>true</c> [success]. </param>
        /// <param name="message"> The errorMessage. </param>
        public BoolMessageItem(T item, bool success, string message) : base(item, success, message)
        {
        }

        /// <summary>
        ///   Return item as correct type.
        /// </summary>
        public new T Item
        {
            get { return (T) base.Item; }
        }
    }

    /// <summary>
    ///   Extensions to the boolmessage item.
    /// </summary>
    public static class BoolMessageExtensions
    {
        /// <summary>
        ///   Convert the result to an exit code.
        /// </summary>
        /// <param name="result"> Result to convert to an exit code. </param>
        /// <returns> 0 for a successful exit, 1 otherwise. </returns>
        public static int AsExitCode(this BoolMessage result)
        {
            return result.Success ? 0 : 1;
        }
    }
}