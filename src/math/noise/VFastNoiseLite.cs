using System.Text.Json;
using System.Text.RegularExpressions;
using Godot;
using VirigirTools.json;
using VirigirTools.opencv;
using VirigirTools.@string;

namespace VirigirTools.math.noise;

public class VFastNoiseLite : FastNoiseLite
{
    private string _name;
    private RandomNumberGenerator _rng;
    private int _nTiers;

    private string[] _noiseProperties =
    {
        "CellularDistanceFunction", "CellularJitter", "CellularReturnType",

        "DomainWarpEnabled",
        "DomainWarpType", "DomainWarpAmplitude", "DomainWarpFrequency",
        "DomainWarpFractalType", "DomainWarpFractalGain", "DomainWarpFractalLacunarity", "DomainWarpFractalOctaves",

        "FractalType",
        "FractalGain", "FractalLacunarity", "FractalOctaves",
        "FractalPingPongStrength", "FractalWeightedStrength",

        "Frequency", "NoiseType", "Seed"
    };

    // CONSTRUCTOR
    public VFastNoiseLite(string name, int nTiers)
    {
        _name = name;
        _rng = new RandomNumberGenerator(); // inicializamos el rng
        _nTiers = nTiers;
    }

    // NOISE VALUES
    public float GetAbsoluteValueNoise(int x, int y) => Mathf.Abs(GetNoise2D(x, y));

    public float GetNormalizedInverseNoise2D(int x, int y) => 1.0f - GetNormalizedNoise2D(x, y);

    public float GetNormalizedNoise2D(int x, int y) => (GetNoise2D(x, y) + 1f) * 0.5f;

    private int GetValueTier(float value, int nTiers = 0)
    {
        nTiers = (nTiers == 0) ? _nTiers : nTiers;
        return (int)(value / (1.0f / nTiers));
    }

    public int GetValueTierAt(int x, int y) => GetValueTier(GetNormalizedNoise2D(x, y));

    public float GetAbsoluteNoiseValueTierAt(int x, int y, int nTiers = 0) =>
        GetValueTier(Mathf.Abs(GetNoise2D(x, y)), nTiers);


    //  NOISE PARAMS
    public void UpdateNoiseProperty(string prop, Variant value) =>
        Set(VStringHelper.CamelCaseToSnakeCase(prop), value);

    public Variant GetNoiseProperty(string prop) => Get(VStringHelper.CamelCaseToSnakeCase(prop));

    public void RandomizeSeed() => SetSeed(_rng.RandiRange(0, 999999999));

    public void SetSeed(int seed) => Seed = seed;

    //  NOISE JSON
    public void SaveToJson(string filename = "")
    {
        var noiseDict = _noiseProperties.ToDictionary(vts => vts, vts =>
            Get(VStringHelper.CamelCaseToSnakeCase(vts)).ToString());
        var jsonString = JsonSerializer.Serialize(noiseDict);

        if (filename == "")
        {
            filename = _name;
        }

        if (!Regex.IsMatch(filename, @"\.json$", RegexOptions.IgnoreCase))
        {
            filename += ".json";
        }

        File.WriteAllText("resources/noise/" + filename, jsonString); // TODO: hacer con la librería de Godot
    }

    public static VFastNoiseLite NoiseFromJson(string filePath, string name, int nTiers)
    {
        var noise = new VFastNoiseLite(name, nTiers);
        //noise.LoadFromJson(filePath);
        if (!Regex.IsMatch(filePath, @"\.json$", RegexOptions.IgnoreCase))
        {
            filePath += ".json";
        }

        foreach (var kvp in VJsonHelper.LoadJson(filePath))
            noise.Set(VStringHelper.CamelCaseToSnakeCase(kvp.Key), (string)kvp.Value);
        return noise;
    }

    // AUX FUNCTIONS
    public IEnumerable<string> GetNoiseProperties() => _noiseProperties;

    // TODO: podríamos pasarle un 4º parámetro opcional para indicar qué método de VFastNoiseLite queremos usar para obtener
    // los valores, siendo el método por defecto GetNormalizedNoise2D.
    public VMatrix AsMatrix(int rows, int cols, int offsetX, int offsetY)
    {
        var matrix = new VMatrix(rows, cols, 0.0f);
        for (var i = 0; i < rows; i++)
        for (var j = 0; j < cols; j++)
            matrix.SetValue(i, j, GetNormalizedNoise2D(i + offsetX, j + offsetY));
        return matrix;
    }

    public VImage AsImage(int rows, int cols, int offsetX, int offsetY) => // untested
        VOpenCvHelper.MatrixToImage(AsMatrix(rows, cols, offsetX, offsetY));

    public VFastNoiseLite Clone() => (VFastNoiseLite)MemberwiseClone();
}