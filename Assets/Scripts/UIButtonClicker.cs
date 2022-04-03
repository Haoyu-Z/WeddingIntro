using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIButtonClicker : MonoBehaviour
{
    [SerializeField]
    private UIKeyCode key;

    public void OnPressed()
    {
        Debug.Assert(GameStatics.Instance != null && GameStatics.Instance.UIKeyManager != null);
        GameStatics.Instance.UIDebugText.AddDebugText($"{key} pressed.");
        GameStatics.Instance.UIKeyManager.SetKeyState(key, true);
    }

    public void OnRelease()
    {
        Debug.Assert(GameStatics.Instance != null && GameStatics.Instance.UIKeyManager != null);
        GameStatics.Instance.UIDebugText.AddDebugText($"{key} released.");
        GameStatics.Instance.UIKeyManager.SetKeyState(key, false);
    }
}
