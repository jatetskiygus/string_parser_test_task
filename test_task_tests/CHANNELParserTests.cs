using Microsoft.VisualStudio.TestTools.UnitTesting;
using test_task.Parsers;

namespace test_task.Tests
{
    [TestClass]
    public class CHANNEL_parserTests
    {
        [TestMethod]
        public void TryParse_ReturnsTrue_AndCorrectResultString()
        {
            var parser = new CHANNEL_parser();
            string payload = "payload_data";
            bool result = parser.TryParse(payload, out string resultString);

            Assert.IsTrue(result);
            Assert.AreEqual("CHANNEL payload_data", resultString);
        }

        [TestMethod]
        public void TryParse_EmptyPayload_ReturnsTrue_AndCorrectResultString()
        {
            var parser = new CHANNEL_parser();
            string payload = "";
            bool result = parser.TryParse(payload, out string resultString);

            Assert.IsFalse(result);
            Assert.AreEqual("", resultString);
        }

        [TestMethod]
        public void TryParse_ReturnsFalse_WhenPayloadIsNull()
        {
            var parser = new CHANNEL_parser();

            bool result = parser.TryParse(null, out string resultString);

            Assert.IsFalse(result);
            Assert.AreEqual(string.Empty, resultString);
        }
    }
}
