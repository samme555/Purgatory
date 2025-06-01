using UnityEngine;

public static class LevelTracker
{
    //?We’ll still keep currentLevel public, but “Increment” is the only safe way to change it.
    public static int currentLevel = 1;

    // A small helper that does the increment AND prints a log with precise context.
    public static void Increment()
    {
        int oldValue = currentLevel;
        currentLevel++;

        // Log: timestamp, old - new, current active scene name, and stack trace if needed
        string sceneName = UnityEngine.SceneManagement.SceneManager.GetActiveScene().name;
        Debug.Log(
            $"[LevelTracker]?INCREMENT?Scene: {sceneName}?{oldValue} ? {currentLevel}?(Time: {Time.time:F3})"
        );
    }

    // (Optional) If you ever need to force?set or reset, wrap that too:
    public static void ForceSet(int newLevel)
    {
        int old = currentLevel;
        currentLevel = newLevel;
        string sceneName = UnityEngine.SceneManagement.SceneManager.GetActiveScene().name;
        Debug.Log(
            $"[LevelTracker]?FORCE_SET?Scene: {sceneName}?{old} ? {currentLevel}?(Time: {Time.time:F3})"
        );
    }
}
