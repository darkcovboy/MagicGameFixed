using UnityEditor;
using UnityEngine;

public static class Tools
{
    [MenuItem("Tools/Clear progress")]
    public static void ClearProgress()
    {
        PlayerPrefs.DeleteAll();
        PlayerPrefs.Save();
    }
}
