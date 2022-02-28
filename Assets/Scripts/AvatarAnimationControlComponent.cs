using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AvatarAnimationControlComponent : MonoBehaviour
{
    public bool isMoving;

    public Direction facingDirection;

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
            isMoving = movementComponent.isMoving;
            facingDirection = movementComponent.facingDirection;
        }

        if(animator!= null)
        {
            animator.SetBool("IsMoving", isMoving);
            animator.SetInteger("Direction", (int)facingDirection);
        }
    }
}
