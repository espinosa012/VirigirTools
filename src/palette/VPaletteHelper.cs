using System.Drawing;
using System.Globalization;
using Emgu.CV;
using Emgu.CV.CvEnum;
using Emgu.CV.Structure;
using Godot;
using Color = Godot.Color;
// Eliminar referencias y usar biblioteca genérica para color (quizá en la biblioteca de Imágenes hay)


public static class VPaletteHelper
{
    // Public methods 
    public static void CreateGradientPalette(string middleColor, int nColors, int tileSizeX, int tileSizeY,
        string filename, string savePath,
        string initialColor = "#FFFFFF", string finalColor = "#000000") =>
        CreatePalettePng(GetGradientPaletteString(nColors, initialColor, finalColor, middleColor), tileSizeX, tileSizeY,
            ProjectSettings.GlobalizePath(savePath + filename.Replace(".png", "") + ".png"));

    public static Bgr IntToBgr(int value) => new Bgr(value, value, value);

    public static Bgr NormValueToBgr(float value) => new Bgr((int)(255 * value), (int)(255 * value),
        (int)(255 * value));


    // Private methods
    private static List<string> GetGradientPaletteString(int nColors, string initialColor, string finalColor,
        string middleColor) // verde
    {
        var paletteArray = new List<string>();
        for (var i = 0; i < nColors; i++)
        {
            var t = (float)i / (nColors - 1);
            Color currentColor;

            if (t <= 0.5f)
            {
                var partialT = t * 2;
                currentColor = LerpColor(HexToColor(initialColor), HexToColor(middleColor), partialT);
            }
            else
            {
                var partialT = (t - 0.5f) * 2;
                currentColor = LerpColor(HexToColor(middleColor), HexToColor(finalColor), partialT);
            }

            paletteArray.Add(ColorToHex(currentColor));
        }

        return paletteArray;
    }


    private static void CreatePalettePng(IReadOnlyList<string> paletteArray, int tileSizeX, int tileSizeY,
        string savePath)
    {
        var paletteMat = new Mat(new Size(tileSizeY * paletteArray.Count, tileSizeX),
            DepthType.Cv8U, 3);
        paletteMat.SetTo(new Bgr(255, 255, 255).MCvScalar);
        var paletteImage = paletteMat.ToImage<Bgr, byte>();
        for (var i = 0; i < paletteArray.Count; i++)
            paletteImage.Draw(new Rectangle(i * tileSizeX, 0, tileSizeY, tileSizeX),
                HexToBgr(paletteArray[i]), -1);
        paletteImage.Save(savePath);
    }

    private static string ColorToHex(Color color) =>
        $"#{(int)(color.R * 255):X2}{(int)(color.G * 255):X2}{(int)(color.B * 255):X2}";

    private static Color HexToColor(string hex)
    {
        if (hex.StartsWith("#")) hex = hex[1..]; //hex.Substring(1);
        if (hex.Length != 6)
            throw new ArgumentException("ERROR: Invalid hex color code.");

        return new Color((int.Parse(hex[..2], NumberStyles.HexNumber)) / 255f,
            int.Parse(hex.Substring(2, 2), NumberStyles.HexNumber) / 255f,
            int.Parse(hex.Substring(4, 2), NumberStyles.HexNumber) / 255f);
    }

    private static Bgr HexToBgr(string hex) => ColorToBgr(HexToColor(hex));

    private static Bgr ColorToBgr(Color color) => new Bgr(255 * color.B, 255 * color.G, 255 * color.R);

    private static Color LerpColor(Color a, Color b, float t) =>
        new(a.R + (b.R - a.R) * t, a.G + (b.G - a.G) * t, a.B + (b.B - a.B) * t);
}