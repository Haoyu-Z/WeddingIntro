using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum UIKeyCode
{
    UI_Up, 
    UI_Down,
    UI_Left,
    UI_Right,  
    UI_A,
    UI_B,
}

public class UIKeyState
{
    public UIKeyCode keyCode;
    public bool IsPressed;
    public bool IsPressedLastFrame;

    public UIKeyState(UIKeyCode key, bool pressed, bool pressedLastFrame = false)
    {
        keyCode = key;
        IsPressed = pressed;
        IsPressedLastFrame = pressedLastFrame;
    }
}

public class UIKeyManager : MonoBehaviour
{
    private Dictionary<UIKeyCode, UIKeyState> keyStates = new Dictionary<UIKeyCode, UIKeyState>();

    public void SetKeyState(UIKeyCode key, bool state)
    {
        if (!keyStates.ContainsKey(key))
        {
            keyStates.Add(key, new UIKeyState(key, state));
        }
        else
        {
            keyStates[key].IsPressed = state;
        }
    }

    private void LateUpdate()
    {
        foreach (UIKeyCode key in keyStates.Keys)
        {
            UIKeyState state = keyStates[key];
            state.IsPressedLastFrame = state.IsPressed;
        }
    }

    public bool GetUIKey(UIKeyCode key)
    {
        return keyStates.ContainsKey(key) && keyStates[key].IsPressed;
    }

    public bool GetUIKeyDown(UIKeyCode key)
    {
        return GetUIKey(key) && !keyStates[key].IsPressedLastFrame;
    }
}
