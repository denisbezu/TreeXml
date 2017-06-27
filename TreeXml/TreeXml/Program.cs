using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TreeXmlLibrary;

namespace TreeXml
{
    class Program
    {
        static void Main(string[] args)
        {
            Saver saver = new Saver();
            saver.LoadXml();
            ConsoleApi consoleApi = new ConsoleApi();
            
            consoleApi.StartInput();
        }
        

    }

}
