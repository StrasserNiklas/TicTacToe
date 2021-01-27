using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Server.Extensions
{
    public static class Extensions
    {
        public static string ConvertToString(this int[] input)
        {
            return string.Join(':', input);
        }

        public static int[] ConvertToIntArray(this string input)
        {
            var deserialized = new List<int>();

            var split = input.Split(':');

            foreach (var item in split)
            {
                deserialized.Add(Convert.ToInt32(item));
            }

            return deserialized.ToArray();
        }
    }
}
