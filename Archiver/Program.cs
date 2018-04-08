using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace archiver
{
    class Program
    {
        static void Main(string[] args)
        {
            OutputConsole.StartMessage();
            Console.ReadKey();
            Console.WriteLine();
          //  Options options = new Options();
            try
            {
            //    CommandLine.Parser.Default.ParseArguments(args, options);
            }
            catch (Exception ex)
            {
                OutputConsole.DisplayError(ex);
                OutputConsole.ExitMessage();
                Console.ReadKey();
                return;
            }
        }
    }
}
