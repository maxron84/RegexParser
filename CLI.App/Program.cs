using System.Text;
using BL.Lib;

namespace CLI.App;

class Program
{
    private Reader _reader = new();
    private Parser _parser = new();
    private Writer _writer = new();
    private StringBuilder _sbProcessedData = new();
    private string _url = string.Empty;
    private string _data = string.Empty;
    private string _pattern = string.Empty;
    private string _group = string.Empty;
    private string _delimiter = string.Empty;
    private int _counter = 0;
    private string[]? _argsBearer;

    public static void Main(string[] args) => _ = new Program().Startup(args);

    private Task Startup(string[] args)
    {
        _argsBearer = args;

        _ = SubscribeToAllEvents(_reader, _parser, _writer);

        _ = FetchDataFromUrl(_url, _data, _reader);
        _ = GetParserInstructions(_pattern, _group, _delimiter);
        _ = PrintParsedData(_parser, _sbProcessedData, _url, _data, _pattern, _group, _delimiter, _counter);
        _ = SaveParsedDataToFlatfileOrNot(_sbProcessedData, _url, _writer);

        _ = ExitApplicationAfterConfirm();

        return Task.CompletedTask;
    }

    private Task FetchDataFromUrl(string url, string data, Reader reader)
    {
        Console.WriteLine("# Paste Website-URL or Path to local Textfile:");
        url = Console.ReadLine() ?? string.Empty;
        data = reader.GetTextinputData(url);

        return Task.CompletedTask;
    }

    private Task GetParserInstructions(string pattern, string group, string delimiter)
    {
        Console.WriteLine("# Paste Regex-Pattern:");
        pattern = Console.ReadLine() ?? string.Empty;
        Console.WriteLine();

        Console.WriteLine("# For each Match, type the Index of one of the parsed Groups, or leave blank to look up all results as a Resultset first:");
        group = Console.ReadLine() ?? string.Empty;
        if (string.IsNullOrEmpty(group))
        {
            Console.WriteLine("# Define delimiter for each column within the Resultset:");
            delimiter = Console.ReadLine() ?? string.Empty;
        }
        Console.WriteLine();

        return Task.CompletedTask;
    }

    private Task PrintParsedData(Parser parser, StringBuilder sbProcessedData, string url, string data, string pattern, string group, string delimiter, int counter)
    {
        Console.WriteLine(sbProcessedData.AppendLine("Source: " + url + $"{Environment.NewLine}---"));
        foreach (string result in parser.GetEachMatch(data, pattern, group, delimiter))
        {
            sbProcessedData.AppendLine(result);
            Console.WriteLine(result);
            counter++;
        }

        Console.WriteLine(sbProcessedData.Append($"---{Environment.NewLine}Total Results: " + counter));
        Console.WriteLine();

        return Task.CompletedTask;
    }

    private Task SaveParsedDataToFlatfileOrNot(StringBuilder sbProcessedData, string url, Writer writer)
    {
        Console.WriteLine($"# Save parsed Data to File? y/n{Environment.NewLine}");
        if (Console.ReadKey().KeyChar is 'y' or 'Y')
        {
            string filename = string.Join("-", url.Split(Path.GetInvalidFileNameChars()));
            string targetlocation = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, $@"Results\{filename}_{DateTime.Now:yyyy-MM-dd_HH-mm-ss}.txt");
            writer.WriteFlatfile(sbProcessedData.ToString(), targetlocation);
        }

        return Task.CompletedTask;
    }

    private Task ExitApplicationAfterConfirm()
    {
        Console.WriteLine($"# Press any key to exit...");
        Console.ReadKey();

        return Task.CompletedTask;
    }

    private Task SubscribeToAllEvents(Reader reader, Parser parser, Writer writer)
    {
        reader.TaskReporting += TaskReporting_EventHandler;
        parser.TaskReporting += TaskReporting_EventHandler;
        writer.TaskReporting += TaskReporting_EventHandler;
        reader.TaskFail += TaskFail_EventHandler;
        parser.TaskFail += TaskFail_EventHandler;
        writer.TaskFail += TaskFail_EventHandler;
        reader.TaskSuccess += TaskSuccess_EventHandler;
        parser.TaskSuccess += TaskSuccess_EventHandler;
        writer.TaskSuccess += TaskSuccess_EventHandler;

        return Task.CompletedTask;
    }

    private void TaskReporting_EventHandler(object? sender, EventArgs e)
    {
        Console.WriteLine("# Processing...");
        Thread.Sleep(500);
    }

    private void TaskFail_EventHandler(object? sender, EventArgs e)
    {
        string message = string.Empty;

        switch (sender)
        {
            case Reader:
                message = "# ERROR: Failed to read from source!";
                break;
            case Parser:
                message = "# ERROR: Invalid input!";
                break;
            case Writer:
                message = "# ERROR: Failure while writing to file!";
                break;
            default:
                throw new InvalidCastException();
        }

        Console.WriteLine(message + Environment.NewLine);
        Main(_argsBearer ?? new[] { string.Empty });
    }

    private void TaskSuccess_EventHandler(object? sender, EventArgs e) => Console.WriteLine("# Done!" + Environment.NewLine);
}
