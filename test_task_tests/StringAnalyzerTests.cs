using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using test_task.Utils;

namespace test_task.Tests
{
    [TestClass]
    public class StringAnalyzerTests
    {
        [TestMethod]
        public void AnalyzeDataPreambleWithResult()
        {
            string input = "DATA11\r\n";

            string result = StringAnalyzer.Analyze(input);

            Assert.IsFalse(string.IsNullOrEmpty(result), "Ожидался непустой результат парсинга");
            StringAssert.Contains(result, "DATA 1"); // Можно подкорректировать под конкретный ожидаемый вывод
        }

        [TestMethod]
        public void Analyze_WithNoPreamble_ReturnsEmptyString()
        {
            string input = "[sdopfkps[odkfposdkfpo]";

            string result = StringAnalyzer.Analyze(input);

            Assert.AreEqual(string.Empty, result);
        }

        [TestMethod]
        public void Analyze_WithEmptyInput_ReturnsEmptyString()
        {
            string input = string.Empty;

            string result = StringAnalyzer.Analyze(input);

            Assert.AreEqual(string.Empty, result);
        }

        [TestMethod]
        public void Analyze_WithMultipleMessages_ReturnsConcatenatedParsedResults()
        {
            string input = $"poiasdjfpsiodDATA11\r\nCHANNEL=11111\r\nspdofpsd";

            string result = StringAnalyzer.Analyze(input);

            Assert.IsFalse(string.IsNullOrEmpty(result));
            StringAssert.Contains(result, "DATA 1");
            StringAssert.Contains(result, "CHANNEL 11111");
        }

        [TestMethod]
        public void Analyze_WithEmptyPayload_IgnoresMessage()
        {
            string input = $"CHANNEL=\r\n";

            string result = StringAnalyzer.Analyze(input);

            Assert.AreEqual(string.Empty, result);
        }

        [TestMethod]
        public void IndexOfSubstring_FindsSubstring()
        {
            int result = StringAnalyzer.IndexOfSubstring("Hello world", "world", 0);
            Assert.AreEqual(6, result);
        }

        [TestMethod]
        public void IndexOfSubstring_ReturnsMinusOne_WhenNotFound()
        {
            int result = StringAnalyzer.IndexOfSubstring("Hello", "planet", 0);
            Assert.AreEqual(-1, result);
        }

        [TestMethod]
        public void IndexOfSubstring_ReturnsMinusOne_OnNullOrEmptyInput()
        {
            Assert.AreEqual(-1, StringAnalyzer.IndexOfSubstring(null, "load", 0));
            Assert.AreEqual(-1, StringAnalyzer.IndexOfSubstring("", "pay", 0));
            Assert.AreEqual(-1, StringAnalyzer.IndexOfSubstring("payload", null, 0));
            Assert.AreEqual(-1, StringAnalyzer.IndexOfSubstring("payload", "", 0));
        }

        [TestMethod]
        public void IndexOfSubstring_ReturnsMinusOne_WhenStartIndexOutOfRange()
        {
            Assert.AreEqual(-1, StringAnalyzer.IndexOfSubstring("payload", "pay", -1));
            Assert.AreEqual(-1, StringAnalyzer.IndexOfSubstring("payload", "load", 100));
        }

        [TestMethod]
        public void ExtractMessages_FindsMessages()
        {
            string input = "DATA11\r\nCHANNEL=AISFJ\r\n";
            List<(string preamble, string payload)> result = StringAnalyzer.ExtractMessages(input);

            Assert.AreEqual(2, result.Count);
            Assert.AreEqual("DATA", result[0].preamble);
            Assert.AreEqual("11", result[0].payload);
            Assert.AreEqual("CHANNEL=", result[1].preamble);
            Assert.AreEqual("AISFJ", result[1].payload);
        }

        [TestMethod]
        public void ExtractMessages_ReturnsEmptyList_WhenNoMessages()
        {
            string input = "DATA1CHANNEL=\rERROR37\n";
            List<(string preamble, string payload)> result = StringAnalyzer.ExtractMessages(input);
            Assert.AreEqual(0, result.Count);
        }

        [TestMethod]
        public void ExtractMessages_IgnoresEmptyPayload()
        {
            string input = "DATA\r\nCHANNEL=1092421820948\r\n";
            List<(string preamble, string payload)> result = StringAnalyzer.ExtractMessages(input);
            Assert.AreEqual(1, result.Count);
            Assert.AreEqual("CHANNEL=", result[0].preamble);
            Assert.AreEqual("1092421820948", result[0].payload);
        }
    }
}
