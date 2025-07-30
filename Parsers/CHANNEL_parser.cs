using test_task.Utils;

namespace test_task.Parsers
{
    internal class CHANNEL_parser : BaseParser
    {
        public CHANNEL_parser() : base("CHANNEL=")
        {
            ReturnString = string.Empty;
        }

        public override bool TryParse(string input)
        {
            if (!ValidPreamble(input))
            {
                Logger.LogError($"Ошибка при разборе строки: преамбула не CHANNEL=. СТРОКА: \"{input}\"");
                return false;
            }

            int lengthStart = Preamble.Length;

            string channel_number = input.Substring(lengthStart, input.Length - 2 - Preamble.Length);

            ReturnString = Preamble.Substring(0, Preamble.Length - 1) + " " + channel_number;
            return true;
        }
    }
}
