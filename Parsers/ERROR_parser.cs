using test_task.Utils;

namespace test_task.Parsers
{
    internal class ERROR_parser : BaseParser
    {
        public ERROR_parser() : base("ERROR") { }

        public override bool TryParse(string payload, out string result_string)
        {
            result_string = string.Empty;

            if (payload == null || payload == string.Empty)
            {
                Logger.LogError($"Пустая полезная информация");
                result_string = string.Empty;
                return false;
            }

            int payload_length = payload.Length;
            if (payload_length != 6)
            {
                Logger.LogError($"Неверная длина полезной информации. Ожидалось: 6, по факту:{payload_length} СТРОКА: \"{Preamble + payload}\"");
                
                return false;
            }

            result_string = Preamble + " " + payload;
            Logger.LogInfo($"Успешно разобрана строка: \"{Preamble + payload}\". Вывод: \"{result_string}\"");
            return true;
        }
    }
}
