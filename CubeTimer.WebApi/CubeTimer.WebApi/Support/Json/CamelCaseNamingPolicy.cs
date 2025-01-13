using System.Text.Json;

namespace CubeTimer.WebApi.Support.Json;

public class CamelCaseNamingPolicy : JsonNamingPolicy
{
    public override string ConvertName(string name)
    {
        if (name.Contains("."))
        {
            var parts = name.Split('.');
            return string.Join(".", parts.Select(p => char.ToLower(p[0]) + p.Substring(1)));
        }

        return char.ToLower(name[0]) + name.Substring(1);
    }
}