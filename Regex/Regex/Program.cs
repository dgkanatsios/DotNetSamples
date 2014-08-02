using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace RegexTest
{
    class Program
    {
        static void Main(string[] args)
        {
            List<string> results = new List<string>();
            string input = "o=ntua,ou=Dep4,ou=units,c=gr;o=ntua;ou=cheng,ou=lab3,c=gr";
            string regex = @"ou=(?<ou>.+?)(;|,|$| )";
            RegexOptions options = RegexOptions.IgnoreCase | RegexOptions.Singleline;
            
            MatchCollection matches = Regex.Matches(input, regex, options);

            foreach (Match match in matches)
            {
                Console.WriteLine(match.Groups["ou"].Value);
            }
            Console.ReadLine();
        }
    }
}
