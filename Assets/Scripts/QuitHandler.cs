using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

#if UNITY_EDITOR
using UnityEditor;
#endif

public class QuitHandler : MonoBehaviour
{
    void OnQuit(InputValue val)
    {
        bool pressed = val.Get<float>() != 0;
        
        if (pressed)
        {
#if UNITY_EDITOR
            EditorApplication.isPlaying = false;
#elif UNITY_STANDALONE
            Application.Quit();
#else

#endif
        }
    }
}
