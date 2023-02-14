namespace BL.Lib;

public class Reader : ALogicBase
{
    public string GetHtmlData(string url)
    {
        string output = string.Empty;

        if (string.IsNullOrEmpty(url))
            return string.Empty;

        if (!url.StartsWith("http"))
        {
            Task fileReadingTask = Task.Run(async () => output = await File.ReadAllTextAsync(url));

            while (!fileReadingTask.IsCompletedSuccessfully)
            {
                OnTaskReporting(EventArgs.Empty);

                if (fileReadingTask.IsFaulted)
                {
                    OnTaskFail(EventArgs.Empty);
                    return string.Empty;
                }
            }

            OnTaskSuccess(EventArgs.Empty);
            return output;
        }

        using (HttpClient client = new HttpClient())
        {
            HttpResponseMessage response = Task.Run(async () => await client.GetAsync(url)).Result;

            if (!response.IsSuccessStatusCode)
                throw new NotImplementedException(); // Fire event

            Task readHtmlAsStringTask = Task.Run(async () => output = await response.Content.ReadAsStringAsync());

            while (!readHtmlAsStringTask.IsCompletedSuccessfully)
            {
                OnTaskReporting(EventArgs.Empty);

                if (readHtmlAsStringTask.IsFaulted)
                {
                    OnTaskFail(EventArgs.Empty);
                    return string.Empty;
                }
            }
        }

        OnTaskSuccess(EventArgs.Empty);
        return output;
    }
}
