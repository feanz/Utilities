using System;
using System.Collections.Generic;
using System.Linq;
using Utilities.Extensions;

namespace Utilities.Maths.ExtensionMethods
{
    /// <summary>
    ///   Extension methods that add basic math functions
    /// </summary>
    public static class MathExtensions
    {
        /// <summary>
        ///   Determines if a value is between two values
        /// </summary>
        /// <typeparam name="T"> Data type </typeparam>
        /// <param name="value"> value to check </param>
        /// <param name="Low"> Low bound (inclusive) </param>
        /// <param name="high"> High bound (inclusive) </param>
        /// <returns> True if it is between the low and high values </returns>
        public static bool Between<T>(this T value, T Low, T high) where T : IComparable
        {
            var Comparer = new GenericComparer<T>();
            return Comparer.Compare(high, value) >= 0 && Comparer.Compare(value, Low) >= 0;
        }

        /// <summary>
        ///   Clamps a value between two values
        /// </summary>
        /// <param name="value"> value sent in </param>
        /// <param name="max"> Max value it can be (inclusive) </param>
        /// <param name="min"> Min value it can be (inclusive) </param>
        /// <returns> The value set between Min and Max </returns>
        public static T Clamp<T>(this T value, T max, T min) where T : IComparable
        {
            var Comparer = new GenericComparer<T>();
            if (Comparer.Compare(max, value) < 0)
                return max;
            return Comparer.Compare(value, min) < 0 ? min : value;
        }

        /// <summary>
        ///   Calculates the factorial for a number
        /// </summary>
        /// <param name="input"> Input value (N!) </param>
        /// <returns> The factorial specified </returns>
        public static int Factorial(this int input)
        {
            var value1 = 1;
            for (var x = 2; x <= input; ++x)
                value1 = value1*x;
            return value1;
        }

        /// <summary>
        ///   Returns the maximum value between the two
        /// </summary>
        /// <param name="inputA"> Input A </param>
        /// <param name="inputB"> Input B </param>
        /// <returns> The maximum value </returns>
        public static T Max<T>(this T inputA, T inputB) where T : IComparable
        {
            var Comparer = new GenericComparer<T>();
            return Comparer.Compare(inputA, inputB) < 0 ? inputB : inputA;
        }

        /// <summary>
        ///   Gets the median from the list
        /// </summary>
        /// <typeparam name="T"> The data type of the list </typeparam>
        /// <param name="values"> The list of values </param>
        /// <returns> The median value </returns>
        public static T Median<T>(this List<T> values)
        {
            if (values.IsNull())
                return default(T);
            if (values.Count() == 0)
                return default(T);
            values.Sort();
            return values[(values.Count/2)];
        }

        /// <summary>
        ///   Returns the minimum value between the two
        /// </summary>
        /// <param name="inputA"> Input A </param>
        /// <param name="inputB"> Input B </param>
        /// <returns> The minimum value </returns>
        public static T Min<T>(this T inputA, T inputB) where T : IComparable
        {
            var Comparer = new GenericComparer<T>();
            return Comparer.Compare(inputA, inputB) > 0 ? inputB : inputA;
        }

        /// <summary>
        ///   Gets the mode (item that occurs the most) from the list
        /// </summary>
        /// <typeparam name="T"> The data type of the list </typeparam>
        /// <param name="values"> The list of values </param>
        /// <returns> The mode value </returns>
        public static T Mode<T>(this IEnumerable<T> values)
        {
            if (values.IsNull())
                return default(T);
            if (values.IsNotNull() && values.Count() == 0)
                return default(T);

            if (values.IsNotNull())
            {
                var query = values.GroupBy(n => n)
                    .OrderByDescending(g => g.Count())
                    .Select(g => g.Key).FirstOrDefault();
                return query;
            }
            return default(T);
        }

        /// <summary>
        ///   Raises value to the power of Power
        /// </summary>
        /// <param name="value"> value to raise </param>
        /// <param name="Power"> Power </param>
        /// <returns> The resulting value </returns>
        public static double Pow(this double value, double Power)
        {
            return Math.Pow(value, Power);
        }

        /// <summary>
        ///   Rounds the value to the number of digits
        /// </summary>
        /// <param name="value"> value to round </param>
        /// <param name="Digits"> Digits to round to </param>
        /// <returns> </returns>
        public static double Round(this double value, int Digits = 2)
        {
            return Math.Round(value, Digits);
        }

        /// <summary>
        ///   Returns the square root of a value
        /// </summary>
        /// <param name="value"> value to take the square root of </param>
        /// <returns> The square root </returns>
        public static double Sqrt(this double value)
        {
            return Math.Sqrt(value);
        }

        /// <summary>
        ///   Gets the standard deviation
        /// </summary>
        /// <param name="values"> List of values </param>
        /// <returns> The standard deviation </returns>
        public static double StandardDeviation(this IEnumerable<double> values)
        {
            return values.Variance().Sqrt();
        }

        /// <summary>
        ///   Calculates the variance of a list of values
        /// </summary>
        /// <param name="values"> List of values </param>
        /// <returns> The variance </returns>
        public static double Variance(this IEnumerable<double> values)
        {
            if (values.IsNull() || values.Count() == 0)
                return 0;
            if (values.IsNotNull())
            {
                var meanValue = values.Average();
                var sum = values.Sum(value => (value - meanValue).Pow(2));
                return sum/values.Count();
            }
            return 0;
        }

        /// <summary>
        ///   Calculates the variance of a list of values
        /// </summary>
        /// <param name="values"> List of values </param>
        /// <returns> The variance </returns>
        public static double Variance(this IEnumerable<int> values)
        {
            if (values.IsNull() || values.Count() == 0)
                return 0;
            if (values.IsNotNull())
            {
                var meanValue = values.Average();
                double sum = values.Sum(value => (value - meanValue).Pow(2));
                return sum/values.Count();
            }
            return 0;
        }

        /// <summary>
        ///   Calculates the variance of a list of values
        /// </summary>
        /// <param name="values"> List of values </param>
        /// <returns> The variance </returns>
        public static double Variance(this IEnumerable<float> values)
        {
            if (values.IsNull() || values.Count() == 0)
                return 0;
            double meanValue = values.Average();
            double sum = values.Cast<int>().Sum(value => (value - meanValue).Pow(2));
            return sum/values.Count();
        }
    }
}