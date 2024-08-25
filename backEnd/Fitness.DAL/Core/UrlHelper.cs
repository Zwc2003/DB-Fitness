using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Fitness.DAL.Core
{
    internal class UrlHelper
    {
        public static bool IsUrl(string input)
        {
            string urlPattern = @"^https?:\/\/[-A-Z0-9+&@#\/%?=~_|!:,.;]*[-A-Z0-9+&@#\/%=~_|]$";
            return Regex.IsMatch(input, urlPattern, RegexOptions.IgnoreCase);
        }
    }
}
