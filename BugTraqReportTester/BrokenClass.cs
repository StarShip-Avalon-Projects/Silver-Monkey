using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BugTraqReportTester
{
    public class BrokenClass
    {
        public BrokenClass()
        {
        }

        public void MakeError()
        {
            throw new Exception("ERROR: I'm Broken");
        }
    }
}