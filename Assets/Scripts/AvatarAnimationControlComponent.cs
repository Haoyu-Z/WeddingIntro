using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AvatarAnimationControlComponent : MonoBehaviour
{
    public bool IsMoving;

    public Direction FacingDirection;

    private Animator animator;

    private AvatarMovementComponent movementComponent;

    private void Start()
    {
        animator = GetComponent<Animator>();
        movementComponent = GetComponent<AvatarMovementComponent>();
    }

    private void Update()
    {
        if (movementComponent != null)
        {
            IsMoving = movementComponent.IsMoving;
            FacingDirection = movementComponent.FacingDirection;
        }

        if(animator!= null)
        {
            animator.SetBool("IsMoving", IsMoving);
            animator.SetInteger("Direction", (int)FacingDirection);
        }
    }
}
