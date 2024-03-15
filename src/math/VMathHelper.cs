using MathNet.Numerics;

namespace VirigirTools.math;

public static class VMathHelper
{
    // Constants
    public const float Pi = (float)Constants.Pi;

    // Trigonometry
    public static float Cos(float angleInRadians) => (float)Trig.Cos(angleInRadians);
    public static float Sin(float angleInRadians) => (float)Trig.Sin(angleInRadians);

    // Matrix
    public static VMatrix GetClearMatrix(int sizeX, int sizeY, float initVal) => new(sizeX, sizeY, initVal);


    // Random
    public static float GetRandomFloatInRange(float min, float max) =>
        min + (float)(new Random()).NextDouble() * (max - min);

    // TODO: Usar MathDotNet para generar números aleatorios
    public static int GetRandomIntInRange(int min, int max) =>
        min + Convert.ToInt32(((new Random()).NextDouble() * max));

    // TODO: usar un generador de números aleatorios externo a Godot, algo más estándar

    
    // Tiers
    public static int GetNormalizedValueTier(float value, int nTiers) => (int)(value / (1.0f / nTiers));
}