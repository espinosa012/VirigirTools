using System.Text.RegularExpressions;

namespace VirigirTools.@string;

public static class VStringHelper
{
    
    public static string CamelCaseToSnakeCase(string str) 
        => Regex.Replace(str, @"([A-Z])", "_$1").TrimStart('_').ToLower();
    
}