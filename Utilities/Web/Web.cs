using System.Web;
using Utilities.Extensions;

namespace Utilities.Web
{
    public class Web
    {
        /// <summary>
        ///   Cookies the specified param.
        /// </summary>
        /// <typeparam name="T"> </typeparam>
        /// <param name="param"> The param. </param>
        /// <returns> </returns>
        public static T Cookie<T>(string param)
        {
            var result = default(T);

            var cookie = HttpContext.Current.Request.Cookies[param];

            if (cookie != null)
            {
                var paramValue = cookie.Value;
                result = paramValue.As<T>();
            }

            return result;
        }

        /// <summary>
        ///   Queries the string.
        /// </summary>
        /// <typeparam name="T"> </typeparam>
        /// <param name="param"> The param. </param>
        /// <returns> </returns>
        public static T QueryString<T>(string param)
        {
            var result = default(T);

            if (HttpContext.Current.Request.QueryString[param] != null)
            {
                object paramValue = HttpContext.Current.Request[param];
                result = paramValue.As<T>();
            }

            return result;
        }

        /// <summary>
        ///   Sessions the value.
        /// </summary>
        /// <typeparam name="T"> </typeparam>
        /// <param name="param"> The param. </param>
        /// <returns> </returns>
        public static T SessionValue<T>(string param)
        {
            var result = default(T);

            if (HttpContext.Current.Session[param] != null)
            {
                var paramValue = HttpContext.Current.Session[param];
                result = paramValue.As<T>();
            }

            return result;
        }
    }
}