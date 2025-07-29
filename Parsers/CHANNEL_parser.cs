using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using test_task.Models;

namespace test_task.Parsers
{
    internal class CHANNEL_parser : BaseParser
    {
        public string Preamble => "CHANNEL=";

        public override bool TryParse(string input, out StringAnalysisResult result)
        {
            result = new StringAnalysisResult();
            if (!input.StartsWith(Preamble)) return false;

            int lengthStart = Preamble.Length;

            string channel_number = input.Substring(lengthStart, input.Length - 2 - Preamble.Length);
            string suffix = input.Substring(input.Length - 2);
            /*if (suffix != "\r\n")
            {
                result.IsValid = false;
                result.ReturnString = "Ожидалось завершение \\r\\n";
                return true;
            }*/

            result.IsValid = true;
            result.Preamble = Preamble;
            result.ReturnString = Preamble.Substring(0, Preamble.Length - 1) + " " + channel_number;
            return true;
        }
    }
}
