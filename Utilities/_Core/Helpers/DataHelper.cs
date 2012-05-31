using System;
using System.Text;

namespace Utilities
{
    public static class DataHelper
    {
        private static readonly Random random = new Random((int)DateTime.Now.Ticks);//thanks to McAden
        
        public static string RandomString(int size)
        {
            var builder = new StringBuilder();
            for (var i = 0; i < size; i++)
            {
                var ch = Convert.ToChar(Convert.ToInt32(Math.Floor(26 * random.NextDouble() + 65)));
                builder.Append(ch);
            }

            return builder.ToString();
        }
    }
}