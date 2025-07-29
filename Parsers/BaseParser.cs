using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using test_task.Models;

namespace test_task.Parsers
{
    internal abstract  class BaseParser
    {
        string Preamble { get; } // Например "DATA"
        public abstract bool TryParse(string input, out StringAnalysisResult result);
    }
}
