namespace BL.Lib;

public class Writer : ALogicBase
{
    public Task WriteFlatfile(string dataOutput, string targetLocation)
    {
        using (StreamWriter streamWriter = File.AppendText(targetLocation))
        {
            ExecuteTaskAdvanced(async () =>
            {
                foreach (string match in dataOutput.Split(Environment.NewLine))
                    await streamWriter.WriteLineAsync(match);
            });
        }

        return Task.CompletedTask;
    }
}
