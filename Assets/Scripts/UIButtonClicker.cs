using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIButtonClicker : MonoBehaviour
{
    [SerializeField]
    private UIKeyCode key;

    public void OnPressed()
    {
        UIDebugText.Instance.AddDebugText($"{key} pressed.", UIDebugText.DebugTextLevel.Log);
        UIKeyManager.Instance.SetKeyState(key, true);
    }

    public void OnRelease()
    {
        UIDebugText.Instance.AddDebugText($"{key} released.", UIDebugText.DebugTextLevel.Log);
        UIKeyManager.Instance.SetKeyState(key, false);
    }
}
