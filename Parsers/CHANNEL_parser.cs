using test_task.Utils;

namespace test_task.Parsers
{
    internal class CHANNEL_parser : BaseParser
    {
        public CHANNEL_parser() : base("CHANNEL="){}

        public override bool TryParse(string payload, out string result_string)
        {
            result_string = string.Empty;

            if (payload == null || payload == string.Empty)
            {
                Logger.LogError($"Пустая полезная информация");
                return false;
            }

            for (int i = 0; i < Preamble.Length - 1; i++)
            {
                result_string += Preamble[i];
            }
            result_string += (" " + payload);
            Logger.LogInfo($"Успешно разобрана строка: \"{Preamble + payload}\". Вывод: \"{result_string}\"");
            return true;
        }
    }
}
