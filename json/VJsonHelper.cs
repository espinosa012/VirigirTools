using System.Text.Json;
using Godot;


public static class VJsonHelper
{
    public static Dictionary<string, Variant> LoadJson(string path)
    {
        var file = Godot.FileAccess.Open(path, Godot.FileAccess.ModeFlags.Read);
        var noiseDict = JsonSerializer.Deserialize<Dictionary<string, string>>(file.GetAsText());
        return noiseDict.ToDictionary<KeyValuePair<string, string>, string, Variant>
            (kvp => kvp.Key, kvp => kvp.Value);
    }
}