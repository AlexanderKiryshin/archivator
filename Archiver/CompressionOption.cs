using CommandLine;

namespace archiver
{
    [Verb("compress", HelpText = "Архивировать файл")]
    public class CompressionOption:IZipOption
    {
        public string InputFileName
        { get; set; }

        public string OutputFileName
        { get; set; }
    }
}