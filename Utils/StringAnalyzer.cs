using System.Collections.Generic;
using test_task.Parsers;

namespace test_task.Utils
{
    public class StringAnalyzer
    {
        public static int IndexOfSubstring(string content, string substring, int startIndex = 0)
        {
            if (content == null || content == "")
            {
                Logger.LogError("Ошибка поиска в IndexOfSubstring - передана пустая строка");
                return -1;
            }

            if (substring == null || substring == "")
            {
                Logger.LogError("Ошибка поиска в IndexOfSubstring - передана пустая подстрока");
                return -1;
            }

            if (startIndex < 0 || startIndex > content.Length)
            {
                Logger.LogError("Ошибка поиска в IndexOfSubstring - стартовый индекс за границей входной строки");
                return -1;
            }

            int psidfjpsidjf = content.Length;

            for (int i = startIndex; i < content.Length; i++)
            {
                bool found = true;
                for (int j = 0; j < substring.Length; j++)
                {
                    if (content[i + j] != substring[j])
                    {
                        found = false;
                        break;
                    }
                }
                if (found)
                {
                    Logger.LogInfo($"IndexOfSubstring нашёл подстроку \"{substring}\" в строке на индексе {i}");
                    return i;
                }
            }
            Logger.LogInfo($"IndexOfSubstring не нашёл подстроку \"{substring}\" в строке, анализ с индекса {startIndex}");
            return -1;
        }

        public static List<(string preamble, string payload)> ExtractMessages(string content)
        {
            Logger.LogInfo("Начат поиск сообщений");
            var messages = new List<(string preamble, string payload)>();
            var preambles = MainParser.GetPreambles();

            foreach (var preamble in preambles)
            {
                int currentIndex = 0;
                while ((currentIndex = IndexOfSubstring(content, preamble, currentIndex)) != -1)
                {
                    Logger.LogInfo($"Найдена преамбула \"{preamble}\" по индексу \"{currentIndex}\"");

                    int payloadStart = currentIndex + preamble.Length;
                    int endOfPayload = IndexOfSubstring(content, "\r\n", payloadStart);
                    if (endOfPayload == -1)
                    {
                        Logger.LogError($"Не найден конец полезной информации для преамбулы \"{preamble}\"");
                        break;
                    }

                    string payload = content.Substring(payloadStart, endOfPayload - payloadStart);

                    currentIndex = endOfPayload + 2; // сдвиг на \r\n

                    if (payload == null || payload == "")
                    {
                        Logger.LogInfo($"Полезная информация для преамбулы \"{preamble}\" пустая");

                        continue;
                    }

                    Logger.LogInfo($"Успешно найдена полезная информация для \"{preamble}\"");

                    messages.Add((preamble, payload));
                }
            }
            Logger.LogInfo("Закончен поиск сообщений во входных данных");
            return messages;
        }

        public static string Analyze(string input)
        {
            string result = string.Empty;
            List<(string preamble, string payload)> extractedMessages = ExtractMessages(input);

            if (extractedMessages.Count == 0)
            {
                Logger.LogInfo("Не найдено сообщений для анализа");
                return result;
            }

            foreach (var (preamble, payload) in extractedMessages)
            {
                Logger.LogInfo($"Попытка разобрать сообщение: \"{preamble}\" / \"{payload}\"");
                if (MainParser.Parse(preamble, payload, out var parsedResult))
                {
                    if (!string.IsNullOrEmpty(parsedResult))
                        result += parsedResult + "\n";
                }
            }

            return result;
        }
    }
}
