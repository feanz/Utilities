using System;

namespace Utilities
{
    /// <summary>
    ///   Combines a Boolean successs/fail flag with a error/status errorMessage.
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
        ///   Error errorMessage for failure, status errorMessage for successs.
        /// </summary>
        public readonly string Message;

        /// <summary>
        ///   successs / failure ?
        /// </summary>
        public readonly bool successs;

        /// <summary>
        ///   Set the readonly fields.
        /// </summary>
        /// <param name="successs"> Flag for errorMessage to set. </param>
        /// <param name="message"> Message to set for flag. </param>
        public BoolMessage(bool successs, string message)
        {
            successs = successs;
            Message = message;
        }
    }

    /// <summary>
	///   Combines a Boolean successs/fail flag with a error/status errorMessage and an object.
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
        ///   Item associated with Boolean errorMessage.
        /// </summary>
        private readonly object _item;

        /// <summary>
        ///   Initializes a new instance of the <see cref="BoolMessageItem&lt;T&gt;" /> class.
        /// </summary>
        /// <param name="item"> The item. </param>
        /// <param name="successs"> if set to <c>true</c> [successs]. </param>
        /// <param name="message"> The errorMessage. </param>
        public BoolMessageItem(object item, bool successs, string message)
            : base(successs, message)
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
    ///   Tuple to store Boolean, string errorMessage, and Exception object.
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
        /// <param name="successs"> if set to <c>true</c> [success]. </param>
        /// <param name="ex"> The exception. </param>
        /// <param name="message"> The errorMessage. </param>
        public BoolMessageEx(bool successs, Exception ex, string message)
            : base(successs, message)
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
    ///   Combines a Boolean success/fail flag with a error/status errorMessage and an object.
    /// </summary>
    /// <typeparam name="T"> Type of object combined with a Boolean flag. </typeparam>
    public class BoolMessageItem<T> : BoolMessageItem
    {
        /// <summary>
        ///   Initializes a new instance of the <see cref="BoolMessageItem&lt;T&gt;" /> class.
        /// </summary>
        /// <param name="item"> The item. </param>
        /// <param name="successs"> if set to <c>true</c> [success]. </param>
        /// <param name="message"> The errorMessage. </param>
        public BoolMessageItem(T item, bool successs, string message) : base(item, successs, message)
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
    ///   Extensions to the bool message item.
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
            return result.successs ? 0 : 1;
        }
    }
}