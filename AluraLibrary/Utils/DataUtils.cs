using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace AluraLibrary.Utils;

public static class DataUtils
{
    public static string RemoveSpecial(string text)
    {
        //Get until a special character appear
        string result = Regex.Replace(text, @"\W", "");
        return result;
    }
}
