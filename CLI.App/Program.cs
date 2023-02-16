using System.Text;
using BL.Lib;

namespace CLI.App;

class Program
{
    private Reader _reader = new();
    private Parser _parser = new();
    private Writer _writer = new();
    private string _url = string.Empty;
    private string _data = string.Empty;
    private string _pattern = string.Empty;
    private string _group = string.Empty;
    private string _delimiter = string.Empty;
    private bool _isCaseSensitive = false;
    private StringBuilder _sbProcessedData = new();
    private string[] _argsBearer = new[] { string.Empty };

    public static void Main(string[] args)
    {
        Program program = new();
        _ = program.Config(args);
        _ = program.Startup();
    }

    private Task Config(string[] args)
    {
        _ = SubscribeToAllEvents();

        return Task.CompletedTask;
    }

    private Task Startup()
    {
        _ = SetDataFromUrl();
        _ = SetParserInstructions();
        _ = PrintParsedData();
        _ = SaveParsedDataToFlatfileConditional();

        Console.WriteLine($"# Press r to restart, or any other key to exit...");
        if (Console.ReadKey().KeyChar is 'r' or 'R')
        {
            Console.WriteLine();
            _ = Startup();
        }

        return Task.CompletedTask;
    }

    private Task SetDataFromUrl()
    {
        Console.WriteLine("# Paste Website-URL or Path to local Textfile:");
        _url = Console.ReadLine() ?? string.Empty;
        _data = _reader.GetTextinputData(_url);

        return Task.CompletedTask;
    }

    private Task SetParserInstructions()
    {
        Console.WriteLine("# Paste Regex-Pattern:");
        _pattern = @"title=""([^\""]*(class|ship|vessel)[^\""]*)"">";
        _pattern = Console.ReadLine() ?? string.Empty;
        Console.WriteLine();

        Console.WriteLine("# For each Match, type the Index of one of the parsed Groups, or leave blank to look up all results as a Resultset first:");
        _group = Console.ReadLine() ?? string.Empty;
        if (string.IsNullOrEmpty(_group))
        {
            Console.WriteLine("# Define delimiter for each column within the Resultset:");
            _delimiter = Console.ReadLine() ?? string.Empty;
        }
        Console.WriteLine();

        Console.WriteLine("# Case-Sensitive? y/n");
        _isCaseSensitive = Console.ReadKey().KeyChar is 'y' or 'Y' ? true : false;

        return Task.CompletedTask;
    }

    private Task PrintParsedData()
    {
        int counter = 0;
        string header = "Source: " + _url + $"{Environment.NewLine}---";
        _sbProcessedData.AppendLine(header);
        Console.WriteLine(header);

        foreach (string result in _parser.GetEachMatch(_data, _pattern, _group, _delimiter, _isCaseSensitive))
        {
            _sbProcessedData.AppendLine(result);
            Console.WriteLine(result);
            counter++;
        }

        string footer = $"---{Environment.NewLine}Total Results: " + counter;
        _sbProcessedData.Append(footer);
        Console.WriteLine(footer);

        return Task.CompletedTask;
    }

    private Task SaveParsedDataToFlatfileConditional()
    {
        Console.WriteLine($"# Save parsed Data to File? y/n{Environment.NewLine}");
        if (Console.ReadKey().KeyChar is 'y' or 'Y')
        {
            string filename = string.Join("-", _url.Split(Path.GetInvalidFileNameChars()));
            string targetlocation = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, $@"Results\{filename}_{DateTime.Now:yyyy-MM-dd_HH-mm-ss}.txt");
            _writer.WriteFlatfile(_sbProcessedData.ToString(), targetlocation);
        }

        Console.WriteLine();

        return Task.CompletedTask;
    }

    private Task SubscribeToAllEvents()
    {
        _reader.TaskReporting += TaskReporting_EventHandler;
        _parser.TaskReporting += TaskReporting_EventHandler;
        _writer.TaskReporting += TaskReporting_EventHandler;
        _reader.TaskFail += TaskFail_EventHandler;
        _parser.TaskFail += TaskFail_EventHandler;
        _writer.TaskFail += TaskFail_EventHandler;
        _reader.TaskSuccess += TaskSuccess_EventHandler;
        _parser.TaskSuccess += TaskSuccess_EventHandler;
        _writer.TaskSuccess += TaskSuccess_EventHandler;

        return Task.CompletedTask;
    }

    private void TaskReporting_EventHandler(object? sender, EventArgs e) => Console.WriteLine("# Processing...");

    private void TaskFail_EventHandler(object? sender, EventArgs e)
    {
        string message = string.Empty;

        try
        {
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
        }
        catch (InvalidCastException ex)
        {
            Console.WriteLine("# ERROR: " + ex.Message);
        }

        Console.WriteLine(message + Environment.NewLine);
        _ = Startup();
    }

    private void TaskSuccess_EventHandler(object? sender, EventArgs e) => Console.WriteLine("# Done!" + Environment.NewLine);
}
