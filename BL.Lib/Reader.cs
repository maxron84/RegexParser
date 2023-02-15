namespace BL.Lib;

public class Reader : ALogicBase
{
    public string GetTextinputData(string url)
    {
        string output = string.Empty;

        if (string.IsNullOrEmpty(url))
            return output;

        if (!url.StartsWith("http"))
        {
            _ = ExecuteActionAdvanced(async () => output = await File.ReadAllTextAsync(url));
            return output;
        }

        using (HttpClient client = new HttpClient())
        {
            HttpResponseMessage response = Task.Run(async () => await client.GetAsync(url)).Result;

            if (!response.IsSuccessStatusCode)
                throw new NotImplementedException(); // Fire event

            _ = ExecuteActionAdvanced(async () => output = await response.Content.ReadAsStringAsync());
        }

        return output;
    }
}
