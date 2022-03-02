using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ButtonType
{
    A,
    B,
}

public class UIButtonHintControl : MonoBehaviour
{
    public ButtonType Type;

    private void Start()
    {
        Animator animator = GetComponent<Animator>();
        animator.SetInteger("ButtonType", (int)Type);
    }
}
