using System;

public abstract class Debug : UnityEngine.Debug
{
    private static bool Debugs => GplayManager.Debugs;

    private static void FilterDebugs(Action method)
    {

#if UNITY_EDITOR

        if (Debugs) method?.Invoke();

#endif

    }


    public new static void Log(object message) => FilterDebugs(() => UnityEngine.Debug.Log(message));
    public new static void Log(object message, UnityEngine.Object context) => FilterDebugs(() => UnityEngine.Debug.Log(message, context));

    public new static void LogWarning(object message) => FilterDebugs(() => UnityEngine.Debug.LogWarning(message));
    public new static void LogWarning(object message, UnityEngine.Object context) => FilterDebugs(() => UnityEngine.Debug.LogWarning(message, context));

    public new static void LogError(object message) => FilterDebugs(() => UnityEngine.Debug.LogError(message));
    public new static void LogError(object message, UnityEngine.Object context) => FilterDebugs(() => UnityEngine.Debug.LogError(message, context));

    public new static void LogAssertion(object message) => FilterDebugs(() => UnityEngine.Debug.LogAssertion(message));
    public new static void LogAssertion(object message, UnityEngine.Object context) => FilterDebugs(() => UnityEngine.Debug.LogAssertion(message, context));
}