using Godot;

namespace VirigirTools.logger;

public class VLogger
{
    
    // TODO: usar herramienta logger opensource externa

    public static void Info(string message)
    {
        string timeStamp = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
        string logMessage = $"[{timeStamp}] [INFO] {message}";
        GD.Print(logMessage);
    }
    
    public static void Error(string message)
    {
        string timeStamp = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
        string logMessage = $"[{timeStamp}] [ERROR] {message}";
        GD.Print(logMessage);
    }
    
    
}