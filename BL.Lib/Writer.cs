namespace BL.Lib;

public class Writer : ALogicBase
{
    public Task WriteFlatfile(string dataOutput, string targetLocation)
    {
        using (StreamWriter streamWriter = File.AppendText(targetLocation))
        {
            Task fileWritingTask = Task.Run(async () =>
            {
                foreach (string match in dataOutput.Split(Environment.NewLine))
                    await streamWriter.WriteLineAsync(match);
            });

            while (!fileWritingTask.IsCompletedSuccessfully)
            {
                OnTaskReporting(EventArgs.Empty);

                if (fileWritingTask.IsFaulted)
                {
                    OnTaskFail(EventArgs.Empty);
                    return Task.CompletedTask;
                }
            }
        }

        OnTaskSuccess(EventArgs.Empty);
        return Task.CompletedTask;
    }
}
