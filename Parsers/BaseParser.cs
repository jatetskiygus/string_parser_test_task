
namespace test_task.Parsers
{
    internal abstract  class BaseParser
    {
        public string Preamble { get; protected set; }
        public string ReturnString { get; protected set; }

        protected BaseParser(string preamble)
        {
            Preamble = preamble;
        }

        protected bool ValidPreamble(string input)
        {
            if(!input.StartsWith(Preamble))
            {
                return false;
            }
            return true;
        }

        public string GetResult()
        {
            return ReturnString;
        }

        public abstract bool TryParse(string input);
    }
}
