using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using test_task.ViewModels;

namespace test_task_tests
{
    [TestClass]
    public class AnalyzeTests
    {
        private MainViewModel viewModel;

        [TestInitialize]
        public void Setup()
        {
            viewModel = new MainViewModel();
        }

        [TestMethod]
        public void IndexOfSubstring_ReturnsCorrectIndex_WhenSubstringExists()
        {
            string content = "payload";
            string substring = "pay";

            int index = MainViewModel.IndexOfSubstring(content, substring);

            Assert.AreEqual(0, index);
        }

        [TestMethod]
        public void IndexOfSubstring_ReturnsMinusOne_WhenSubstringNotFound()
        {
            string content = "preamble";
            string substring = "payload";

            int index = MainViewModel.IndexOfSubstring(content, substring);

            Assert.AreEqual(-1, index);
        }

        [TestMethod]
        public void ExtractMessages_FindsMessagesWithPreambles()
        {
            string testContent = "CHANNEL=000\r\nDATA11\r\n";
            var messages = InvokeExtractMessages(viewModel, testContent);

            Assert.AreEqual(2, messages.Count);
            Assert.AreEqual("DATA", messages[0].preamble);
            Assert.AreEqual("11", messages[0].payload);
            Assert.AreEqual("CHANNEL=", messages[1].preamble);
            Assert.AreEqual("000", messages[1].payload);
        }

        [TestMethod]
        public void Analyze_SetsAnalysisResult_WhenInputTextIsSet()
        {
            // Подготовка входных данных
            viewModel.InputText = "apfkapsofkDATA11\r\n";

            viewModel.AnalyzeCommand.Execute(null);

            Assert.IsFalse(string.IsNullOrEmpty(viewModel.AnalysisResult));
        }

        [TestMethod]
        public void Analyze_SetsError_WhenNoInputOrFile()
        {
            viewModel.InputText = "";
            viewModel.SelectedFilePath = "";

            viewModel.AnalyzeCommand.Execute(null);

            Assert.IsTrue(viewModel.AnalysisResult.Contains("Ошибка обработки"));
        }


        // Вспомогательный метод для вызова приватного ExtractMessages (через рефлексию)
        private List<(string preamble, string payload)> InvokeExtractMessages(MainViewModel vm, string content)
        {
            var method = typeof(MainViewModel).GetMethod("ExtractMessages", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
            return (List<(string preamble, string payload)>)method.Invoke(vm, new object[] { content });
        }
    }
}
