using CommandLine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace archiver
{
   public interface IZipOption
    {
        [Value(0, Required = true)]
        string InputFileName
        { get; set; }

        [Value(1,Required =true)]
        string OutputFileName
        { get; set; }
    }
}
