using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace test_task.Parsers
{
    public static class MainParser
    {
        private static readonly List<BaseParser> Parsers = new List<BaseParser>
        {
            new DATA_parser(),
            new CHANNEL_parser()
        // Новый парсер сюда
        };

        public static string Parse(string input)
        {
            foreach (var parser in Parsers)
            {
                if (parser.TryParse(input))
                {
                    return "Результат: " + parser.GetResult();
                }
            }
            return string.Empty;
        }
    }
}
