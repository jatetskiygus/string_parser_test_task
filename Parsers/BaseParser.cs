namespace test_task.Parsers
{
    internal abstract  class BaseParser
    {
        public string Preamble { get; protected set; }

        protected BaseParser(string preamble)
        {
            Preamble = preamble;
        }
        
        public abstract bool TryParse(string payload, out string result_string);
    }
}
