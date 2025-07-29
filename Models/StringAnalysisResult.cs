using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace test_task.Models
{
    public class StringAnalysisResult
    {
        public bool IsValid { get; set; }
        public string Preamble { get; set; }
        public string ReturnString { get; set; }
    
        public override string ToString()
        {
            return "Результат: " + ReturnString;
        }
    }
}
