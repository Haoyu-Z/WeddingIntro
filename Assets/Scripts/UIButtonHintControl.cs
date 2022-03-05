using UnityEngine;

public enum ButtonType
{
    ControllerButtonA,
    ControllerButtonB,
}

public class UIButtonHintControl : MonoBehaviour
{
    [SerializeField]
    private ButtonType Type;

    private void Start()
    {
        Animator animator = GetComponent<Animator>();
        animator.SetInteger("ButtonType", (int)Type);
    }
}
