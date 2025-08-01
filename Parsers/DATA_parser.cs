﻿using test_task.Utils;

namespace test_task.Parsers
{
    internal class DATA_parser : BaseParser
    {
        public DATA_parser() : base("DATA"){}

        public override bool TryParse(string payload, out string result_string)
        {
            result_string = string.Empty;

            if (payload == null || payload == string.Empty)
            {
                Logger.LogError($"Пустая полезная информация");
                return false;
            }

            int awaited_payload_length = payload[0] - '0';
            int payload_length = payload.Length - 1;
            if (awaited_payload_length != payload_length)
            {
                Logger.LogError($"Неверный байт длины. Ожидалось: {awaited_payload_length}, по факту:{payload_length} СТРОКА: \"{Preamble + payload}\"");
                return false;
            }

            result_string = Preamble + " " + payload.Substring(1);
            Logger.LogInfo($"Успешно разобрана строка: \"{Preamble + payload}\". Вывод: \"{result_string}\"");
            return true;
        }
    }
}
