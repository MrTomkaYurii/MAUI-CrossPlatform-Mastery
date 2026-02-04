namespace MauiMastery.ShellRouting.Routing;

public class ShellQueryResolver
{
    public static Dictionary<string, string> ParseQueryString(string route)
    {
        var result = new Dictionary<string, string>();
        var queryIndex = route.IndexOf('?');
        if (queryIndex < 0 || queryIndex == route.Length - 1) return result;

        var queryString = route[(queryIndex + 1)..];
        var pairs = queryString.Split('&', StringSplitOptions.RemoveEmptyEntries);
        foreach (var pair in pairs)
        {
            var parts = pair.Split('=', 2);
            var key = Uri.UnescapeDataString(parts[0]);
            var value = parts.Length > 1 ? Uri.UnescapeDataString(parts[1]) : string.Empty;
            result[key] = value;
        }
        return result;
    }
}