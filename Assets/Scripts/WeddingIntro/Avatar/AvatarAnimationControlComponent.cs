using UnityEngine;

namespace WeddingIntro.Avatar
{
    public class AvatarAnimationControlComponent : MonoBehaviour
    {
        private bool isMoving;

        private Direction facingDirection;

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
                isMoving = movementComponent.IsMoving;
                facingDirection = movementComponent.FacingDirection;
            }

            if (animator != null)
            {
                animator.SetBool("IsMoving", isMoving);
                animator.SetInteger("Direction", (int)facingDirection);
            }
        }
    }
}
