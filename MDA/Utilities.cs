using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MDA
{
    public class Utilities
    {
        public string GetSubstringByString(string a, string b, string c)
        {
            return c.Substring((c.IndexOf(a) + a.Length), (c.IndexOf(b) - c.IndexOf(a) - a.Length));
        }
    }
}