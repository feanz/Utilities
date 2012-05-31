namespace Utilities.Maths
{
    /// <summary>
    ///   Conversion helper
    /// </summary>
    public static class Conversion
    {
        /// <summary>
        ///   Celsius to Fahrenheit
        /// </summary>
        /// <param name="value"> Celsius value </param>
        /// <returns> Equivalent Fahrenheit temp </returns>
        public static double CelsiusToFahrenheit(double value)
        {
            return ((value*9)/5) + 32;
        }

        /// <summary>
        ///   Celsius to Kelvin
        /// </summary>
        /// <param name="value"> Celsius value </param>
        /// <returns> Equivalent Kelvin temp </returns>
        public static double CelsiusToKelvin(double value)
        {
            return value + 273.15;
        }

        /// <summary>
        ///   Fahrenheit to Celsius
        /// </summary>
        /// <param name="value"> Fahrenheit value </param>
        /// <returns> Equivalent Celsius value </returns>
        public static double FahrenheitToCelsius(double value)
        {
            return ((value - 32)*5)/9;
        }

        /// <summary>
        ///   Fahrenheit to Kelvin
        /// </summary>
        /// <param name="value"> Fahrenheit value </param>
        /// <returns> Equivalent Kelvin value </returns>
        public static double FahrenheitToKelvin(double value)
        {
            return ((value + 459.67)*5)/9;
        }

        /// <summary>
        ///   Kelvin to Celsius
        /// </summary>
        /// <param name="value"> Kelvin value </param>
        /// <returns> Equivalent Celsius value </returns>
        public static double KelvinToCelsius(double value)
        {
            return value - 273.15;
        }

        /// <summary>
        ///   Kelvin to Fahrenheit
        /// </summary>
        /// <param name="value"> Kelvin value </param>
        /// <returns> Equivalent Fahrenheit value </returns>
        public static double KelvinToFahrenheit(double value)
        {
            return ((value*9)/5) - 459.67;
        }
    }
}