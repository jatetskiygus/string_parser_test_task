using System.Collections.Generic;
using test_task.Utils;

namespace test_task.Parsers
{
    public static class MainParser
    {
        private static readonly List<BaseParser> Parsers = new List<BaseParser>
        {
            new DATA_parser(),
            new CHANNEL_parser(),
            new ERROR_parser()
        // Новый парсер сюда
        };

        public static List<string> GetPreambles()
        {
            List<string> preambles = new List<string>(); ;
            foreach (var parser in Parsers)
            {
                preambles.Add(parser.Preamble);
            }
            return preambles;
        }

        public static bool Parse(string preamble, string payload, out string result)
        {
            if (payload == null || payload == "")
            {
                Logger.LogError("Пустая полезная информация в MainParser");
                result = string.Empty;
                return false;
            }

            if (preamble == null || preamble == "")
            {
                Logger.LogError("Пустая преамбула в MainParser");
                result = string.Empty;
                return false;
            }

            foreach (var parser in Parsers)
            {
                if(parser.Preamble != preamble)
                {
                    continue;
                }

                Logger.LogInfo($"Начат разбор строки: \"{preamble + payload}\" с помощью парсера \"{parser.GetType().Name}\"");
                if (parser.TryParse(payload, out string result_string))
                {
                    result = result_string;
                    return true;
                }
            }
            Logger.LogError($"Не удалось разобрать строку: \"{payload}\". Неизвестная преамбула.");
            result = string.Empty;
            return false;
        }
    }
}
