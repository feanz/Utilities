using System;

namespace Utilities.Extensions
{
    public static class Boolean
    {
        /// <summary>
        ///   Returns a yes/no string based on the specified Boolean flag
        /// </summary>
        /// <param name="flag"> Boolean to check. </param>
        /// <param name="trueValue"> The true string value. Defaults to 'Yes' </param>
        /// <param name="falseValue"> The false string value. Defaults to 'No' </param>
        /// <returns> A string that represents the Boolean flag. </returns>
        public static string YesNo(this bool flag, string trueValue = "Yes", string falseValue = "No")
        {
            return (flag ? trueValue : falseValue);
        }
    }
}