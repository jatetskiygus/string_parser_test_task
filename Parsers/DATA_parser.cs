using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using test_task.Models;

namespace test_task.Parsers
{
    internal class DATA_parser : BaseParser
    {
        public string Preamble => "DATA";

        public override bool TryParse(string input, out StringAnalysisResult result)
        {
            result = new StringAnalysisResult();
            if (!input.StartsWith(Preamble)) return false;

            int lengthStart = Preamble.Length;
            string lengthPart = input.Substring(lengthStart, 1);
            if (!int.TryParse(lengthPart, out int payloadLength))
            {
                result.IsValid = false;
                result.ReturnString = "Неверный байт длины";
                return true;
            }

            int expectedLength = Preamble.Length + 1 + payloadLength + 2;
            if (input.Length < expectedLength)
            {
                result.IsValid = false;
                result.ReturnString = "Недостаточная длина строки";
                return true;
            }

            string payload = input.Substring(lengthStart + 1, payloadLength);
            string suffix = input.Substring(lengthStart + 1 + payloadLength);
            /*if (suffix != "\r\n")
            {
                result.IsValid = false;
                result.ReturnString = "Ожидалось завершение \\r\\n";
                return true;
            }*/

            result.IsValid = true;
            result.Preamble = Preamble;
            result.ReturnString = Preamble + " " + payload;
            return true;
        }
    }
}
