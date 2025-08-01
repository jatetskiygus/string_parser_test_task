using Microsoft.VisualStudio.TestTools.UnitTesting;
using test_task.Parsers;

namespace test_task.Tests
{
    [TestClass]
    public class DATAparserTests
    {
        [TestMethod]
        public void TryParse_ReturnsTrue_WhenPayloadLengthMatches()
        {
            var parser = new DATA_parser();
            string payload = "5jgnasp";
            bool result = parser.TryParse(payload, out string resultString);

            Assert.IsTrue(result);
            Assert.AreEqual("DATA jgnasp", resultString);
        }

        [TestMethod]
        public void TryParse_ReturnsFalse_WhenPayloadLengthMismatch()
        {
            var parser = new DATA_parser();
            string payload = "3osdjgpsdifjg"; 

            bool result = parser.TryParse(payload, out string resultString);

            Assert.IsFalse(result);
            Assert.AreEqual(string.Empty, resultString);
        }

        [TestMethod]
        public void TryParse_ReturnsFalse_WhenPayloadIsNull()
        {
            var parser = new DATA_parser();

            bool result = parser.TryParse(null, out string resultString);

            Assert.IsFalse(result);
            Assert.AreEqual(string.Empty, resultString);
        }

        [TestMethod]
        public void TryParse_ReturnsFalse_WhenPayloadIsEmpty()
        {
            var parser = new DATA_parser();

            bool result = parser.TryParse(string.Empty, out string resultString);

            Assert.IsFalse(result);
            Assert.AreEqual(string.Empty, resultString);
        }
    }
}
