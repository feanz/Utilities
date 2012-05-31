using System;
using System.Collections.Generic;

namespace Utilities
{
    /// <summary>
    ///   Generic IComparable class
    /// </summary>
    /// <typeparam name="T"> Data type </typeparam>
    public class GenericComparer<T> : IComparer<T> where T : IComparable
    {
        #region IComparer<T> Members

        public int Compare(T x, T y)
        {
            if (!typeof (T).IsValueType
                || (typeof (T).IsGenericType
                    && typeof (T).GetGenericTypeDefinition().IsAssignableFrom(typeof (Nullable<>))))
            {
                if (Equals(x, default(T)))
                    return Equals(y, default(T)) ? 0 : -1;
                if (Equals(y, default(T)))
                    return -1;
            }
            if (x.GetType() != y.GetType())
                return -1;
            if (x is IComparable<T>)
                return ((IComparable<T>) x).CompareTo(y);
            return x.CompareTo(y);
        }

        #endregion
    }
}