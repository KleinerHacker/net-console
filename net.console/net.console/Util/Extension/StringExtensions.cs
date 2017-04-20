using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace net.console.Util.Extension
{
    internal static class StringExtensions
    {
        public static string Repeat(this string value, int count)
        {
            var val = "";
            for (var i = 0; i < count; i++)
            {
                val += value;
            }

            return val;
        }
    }
}
