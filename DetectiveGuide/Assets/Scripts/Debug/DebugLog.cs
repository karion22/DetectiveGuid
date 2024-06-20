using UnityEngine;

public static class DebugLog
{
    public static void Print(this Object inContext, string inMessage)
    {
        Debug.Log(Utility.BuildString("[{0} LOG] {1}", inContext.name, inMessage));
    }

    public static void Print(this Object inContext, string inFormat, params object[] inValues)
    {
        Debug.LogFormat(LogType.Log, LogOption.None, inContext, inFormat, inValues);
    }

    public static void Warning(this Object inContext, string inMessage)
    {
        Debug.Log(Utility.BuildString("[{0} WARNING] {1}", inContext.name, inMessage));
    }

    public static void Warning(this Object inContext, string inFormat, params object[] inValues)
    {
        Debug.LogFormat(LogType.Warning, LogOption.None, inContext, inFormat, inValues);
    }

    public static void Asset(this Object inContext, string inMessage)
    {
        Debug.LogFormat(Utility.BuildString("[{0} ASSERT] {1}", inContext.name, inMessage));
    }

    public static void Assert(this Object inContext, string inFormat, params object[] inValues)
    {
        Debug.LogFormat(LogType.Assert, LogOption.None, inContext, inFormat, inValues);
    }

    public static void Error(string inMessage)
    {
        Debug.LogError(inMessage);
    }

    public static void Error(this Object inContext, string inMessage)
    {
        Debug.LogError(Utility.BuildString("[{0} ERROR] {1}", inContext.name, inMessage));
    }

    public static void Error(this Object inContext, string inFormat, params object[] inValues)
    {
        Debug.LogFormat(LogType.Error, LogOption.None, inContext, inFormat, inValues);
    }
}
