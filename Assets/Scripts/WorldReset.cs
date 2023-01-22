using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Events;

public class WorldReset : MonoBehaviour
{
    public UnityEvent OnResetWorld = new UnityEvent();


    void OnWorldReset(InputValue val)
    {
        bool pressed = val.Get<float>() > 0;
        Debug.LogWarning("Should Reset: " + pressed);
        if (pressed)
        {
            OnResetWorld.Invoke();
        }
    }
}
