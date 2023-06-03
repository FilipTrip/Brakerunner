using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Threading.Tasks;

public class Exit : MonoBehaviour
{
    /// <summary>
    /// Exits this application.
    /// </summary>
    public static void ExitApplication()
    {
        #if UNITY_EDITOR

        UnityEditor.EditorApplication.isPlaying = false;

        #else

        Application.Quit();

        #endif
    }

}
