using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GSMapp
{
    public class GeneralCommands
    {
        public string OperatorName { get; set; } = null;

        public string Operator(string sender)
        {
            string a = sender;
            int startIndex = a.IndexOf("\"", StringComparison.Ordinal)+4;
            int lastIndex = a.LastIndexOf("\"", StringComparison.Ordinal) - startIndex;
            if (sender.Contains("+COPS:"))
            {
                a = a.Substring(startIndex, lastIndex);
                return a;
            }
            return null;
        }
    }
}
