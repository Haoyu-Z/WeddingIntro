using UnityEngine;

namespace WeddingIntro.Avatar
{
    public class AvatarAnimationControlComponent : MonoBehaviour
    {
        public enum ActionType
        {
            None = 0,
            Drink = 1,
            Eat = 2,
        }

        [System.Serializable]
        private struct ActionTimeEntry
        {
            public ActionType Type;

            public float Duration;
        }

        [SerializeField]
        private ActionTimeEntry[] actionTimeTable;

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

        public void PlayAction(ActionType actionType, System.Action finishCallback)
        {
            float actionTime = 0.0f;
            foreach(ActionTimeEntry entry in actionTimeTable)
            {
                if(entry.Type == actionType)
                {
                    actionTime = entry.Duration;
                    break;
                }
            }

            if (actionTime > 0.0f)
            {
                animator.SetInteger("ActionType", (int)actionType);
                StartCoroutine(ActionTimer(actionTime, finishCallback));
            }
        }

        private System.Collections.IEnumerator ActionTimer(float time, System.Action finishCallback)
        {
            yield return new WaitForSeconds(time);

            animator.SetInteger("ActionType", (int)ActionType.None);
            movementComponent.FacingDirection = Direction.Backward;
            animator.SetInteger("Direction", (int)Direction.Backward);

            finishCallback.Invoke();
        }
    }
}
