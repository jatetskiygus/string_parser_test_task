using test_task.Utils;

namespace test_task.Parsers
{
    internal class DATA_parser : BaseParser
    {
        public DATA_parser() : base("DATA") 
        {
            ReturnString = string.Empty;
        }

        public override bool TryParse(string input)
        {
            if (!input.StartsWith(Preamble))
            {
                Logger.LogError($"Ошибка при разборе строки: преамбула не DATA. СТРОКА: \"{input}\"");
                return false;
            }

            int lengthStart = Preamble.Length;
            string lengthPart = input.Substring(lengthStart, 1);
            if (!int.TryParse(lengthPart, out int payloadLength))
            {
                Logger.LogError($"Ошибка при разборе строки: неверный байт длины. СТРОКА: \"{input}\"");
                return false;
            }

            int expectedLength = Preamble.Length + 1 + payloadLength + 2;
            if (input.Length < expectedLength)
            {
                Logger.LogError($"Ошибка при разборе строки: неверная длина. СТРОКА: \"{input}\"");
                return false;
            }

            string payload = input.Substring(lengthStart + 1, payloadLength);
            ReturnString = Preamble + " " + payload;
            Logger.LogInfo($"Успешно разобрана строка: \"{input}\". Вывод: \"{ReturnString}\"");
            return true;
        }
    }
}
