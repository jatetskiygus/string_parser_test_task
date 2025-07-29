using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using test_task.Models;

namespace test_task.Parsers
{
    public static class MainParser
    {
        private static readonly List<BaseParser> Parsers = new List<BaseParser>
    {
        new DATA_parser(),
        new CHANNEL_parser()
        // Добавляешь новый — просто допиши сюда
    };

        public static StringAnalysisResult Parse(string input)
        {
            foreach (var parser in Parsers)
            {
                if (parser.TryParse(input, out var result))
                {
                    return result;
                }
            }

            return new StringAnalysisResult
            {
                IsValid = false,
                ReturnString = "Неизвестная преамбула или формат"
            };
        }
    }
}
