using System.Text;
using System.Text.RegularExpressions;

namespace BL.Lib;

public class Parser : ALogicBase
{
    public IEnumerable<string> GetEachMatch(string dataInput, string pattern, string group, string delimiter, bool isCaseSensitive)
    {
        MatchCollection matches;
        RegexOptions regexOptions = isCaseSensitive ? RegexOptions.None : RegexOptions.IgnoreCase;
        
        try
        {
            matches = GetMatches(dataInput, pattern, regexOptions);
        }
        catch (RegexParseException)
        {
            throw;
        }

        if (matches.Any())
        {
            if (!int.TryParse(group, out int target))
            {
                StringBuilder sbResults = new();

                for (int i = 0; i < matches.Count; i++)
                {
                    for (int j = 0; j < matches[i].Groups.Count; j++)
                    {
                        sbResults.Append(matches[i].Groups[j].Value);

                        if (j == matches[i].Groups.Count - 1)
                            break;

                        sbResults.Append(delimiter);
                    }

                    yield return sbResults.ToString();

                    sbResults.Clear();
                }
            }
            else
            {
                foreach (Match match in matches)
                    yield return match.Groups[target].Value;
            }
        }
    }

    private MatchCollection GetMatches(string data, string pattern, RegexOptions regexOptions)
    {
        try
        {
            return Regex.Matches(data, pattern, regexOptions);
        }
        catch (RegexParseException)
        {
            throw;
        }
    }
}
