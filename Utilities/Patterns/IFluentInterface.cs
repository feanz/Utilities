using System;
using System.ComponentModel;

namespace Utilities.Patterns
{
    /// <summary>
    ///   Helps in fluent interface design to hide ToString, Equals, and GetHashCode
    /// </summary>
    public interface IFluentInterface
    {
        #region Functions

        [EditorBrowsable(EditorBrowsableState.Never)]
        bool Equals(object obj);

        [EditorBrowsable(EditorBrowsableState.Never)]
        int GetHashCode();

        [EditorBrowsable(EditorBrowsableState.Never)]
        Type GetType();

        [EditorBrowsable(EditorBrowsableState.Never)]
        string ToString();

        #endregion
    }
}