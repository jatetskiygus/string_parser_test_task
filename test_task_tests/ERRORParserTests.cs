using Microsoft.VisualStudio.TestTools.UnitTesting;
using test_task.Parsers;

namespace test_task.Tests
{
    [TestClass]
    public class ERROR_parserTests
    {
        [TestMethod]
        public void TryParse_ReturnsTrue_WhenPayloadLengthIsSix()
        {
            var parser = new ERROR_parser();
            string payload = "000000"; 

            bool result = parser.TryParse(payload, out string resultString);

            Assert.IsTrue(result);
            Assert.AreEqual("ERROR 000000", resultString);
        }

        [TestMethod]
        public void TryParse_ReturnsFalse_WhenPayloadLengthIsNotSix()
        {
            var parser = new ERROR_parser();
            string payload = "pifgn"; 

            bool result = parser.TryParse(payload, out string resultString);

            Assert.IsFalse(result);
            Assert.AreEqual(string.Empty, resultString);
        }

        [TestMethod]
        public void TryParse_ReturnsFalse_WhenPayloadIsNull()
        {
            var parser = new ERROR_parser();

            bool result = parser.TryParse(null, out string resultString);

            Assert.IsFalse(result);
            Assert.AreEqual(string.Empty, resultString);
        }

        [TestMethod]
        public void TryParse_ReturnsFalse_WhenPayloadIsEmpty()
        {
            var parser = new ERROR_parser();

            bool result = parser.TryParse(string.Empty, out string resultString);

            Assert.IsFalse(result);
            Assert.AreEqual(string.Empty, resultString);
        }
    }
}
