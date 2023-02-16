namespace BL.Lib;

public class Reader : ALogicBase
{
    public string GetTextinputData(string url)
    {
        string output = string.Empty;

        if (string.IsNullOrEmpty(url))
            throw new ArgumentException("Path cannot be null or empty!");

        if (!url.StartsWith("http"))
        {
            _ = ExecuteTaskAdvanced(async () => output = await File.ReadAllTextAsync(url));
            return output;
        }

        using (HttpClient client = new HttpClient())
        {
            HttpResponseMessage response = new();
            _ = ExecuteTaskAdvanced(async () => response = await client.GetAsync(url));

            try
            {
                response.EnsureSuccessStatusCode();
            }
            catch (HttpRequestException)
            {
                throw;
            }

            _ = ExecuteTaskAdvanced(async () => output = await response.Content.ReadAsStringAsync());
        }

        return output;
    }
}
