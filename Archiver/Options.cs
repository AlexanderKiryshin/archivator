using CommandLine;
public class Options
    {
        [Value(1)]
        public string InputFileName
        { get; set; }

    [Value(0)]
    public int OutputFileName
    { get; set; }
}
