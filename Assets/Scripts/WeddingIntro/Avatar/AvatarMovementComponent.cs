using UnityEngine;
using WeddingIntro.Utility;

namespace WeddingIntro.Avatar
{
    public enum Direction
    {
        Forward = 0,
        Rightward = 1,
        Backward = 2,
        Leftward = 3,
    }

    public class AvatarMovementComponent : MonoBehaviour
    { 
        private static readonly float[] directionAngles = new float[] { 0.0f, 90.0f, 180.0f, -90.0f };

        [SerializeField]
        private float moveSpeed = 3.2f;

        [SerializeField]
        private float speedFade = 0.8f;

        private Vector2 playerMovementRangeX;

        private Vector2 playerMovementRangeY;

        private Direction facingDirection = Direction.Backward;

        private bool isMoving = false;

        private Rigidbody2D rigidBody2D;

        public Direction FacingDirection
        {
            get { return facingDirection; }
            set { facingDirection = value; }
        }

        public bool IsMoving
        {
            get { return isMoving; }
        }

        public float FacingDirectionAngle
        {
            get { return directionAngles[(int)facingDirection]; }
        }

        private Vector3 pendingMovementInputVector;

        private Vector3 movementInputVector;

        private void AddMovementInputVector(GameKeyCode gameKeyCode)
        {
            if (gameKeyCode == GameKeyCode.DirectionRight)
            {
                pendingMovementInputVector += Vector3.right;
            }
            else if (gameKeyCode == GameKeyCode.DirectionLeft)
            {
                pendingMovementInputVector += Vector3.left;
            }
            else if (gameKeyCode == GameKeyCode.DirectionUp)
            {
                pendingMovementInputVector += Vector3.up;
            }
            else if (gameKeyCode == GameKeyCode.DirectionDown)
            {
                pendingMovementInputVector += Vector3.down;
            }
        }

        private void Start()
        {
            rigidBody2D = GetComponent<Rigidbody2D>();

            AvatarInput avatarInput = GetComponent<AvatarInput>();
            Debug.Assert(avatarInput != null);
            avatarInput.AddKeyResponse(DirectionKeyResponsePriority.Movement, new AvatarInput.KeyResponse(AddMovementInputVector), KeyPressType.DefaultPressing);
        }

        private void ResetPlayerMovementRange()
        {
            CameraFollow cameraFollow = GameStatics.Instance?.Camera;
            Debug.Assert(cameraFollow != null);

            playerMovementRangeX = cameraFollow.PlayerMovementRangeX;
            playerMovementRangeY = cameraFollow.PlayerMovementRangeY;
        }

        private void FixedUpdate()
        {
            if (rigidBody2D == null)
            {
                return;
            }

            if (movementInputVector.x != 0 || movementInputVector.y != 0)
            {
                rigidBody2D.velocity = (new Vector2(movementInputVector.x, movementInputVector.y)).normalized * moveSpeed;
            }
            else
            {
                rigidBody2D.velocity = Vector2.Lerp(rigidBody2D.velocity, Vector2.zero, speedFade);
            }
        }

        private void Update()
        {
            isMoving = pendingMovementInputVector.x != 0 || pendingMovementInputVector.y != 0;
            if (isMoving)
            {
                if (Mathf.Abs(pendingMovementInputVector.x) >= Mathf.Abs(pendingMovementInputVector.y))
                {
                    facingDirection = pendingMovementInputVector.x > 0 ? Direction.Rightward : Direction.Leftward;
                }
                else
                {
                    facingDirection = pendingMovementInputVector.y > 0 ? Direction.Forward : Direction.Backward;
                }
            }

            CameraFollow cameraFollow = GameStatics.Instance?.Camera;
            if (cameraFollow != null)
            {
                ResetPlayerMovementRange();
                Vector3 position = gameObject.transform.position;
                position.x = Mathf.Clamp(position.x, playerMovementRangeX.x, playerMovementRangeX.y);
                position.y = Mathf.Clamp(position.y, playerMovementRangeY.x, playerMovementRangeY.y);
                gameObject.transform.position = position;
            }

            movementInputVector = pendingMovementInputVector;
            pendingMovementInputVector = Vector3.zero;
        }

#if UNITY_EDITOR
        private void OnDrawGizmos()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawSphere(transform.position, 0.1f);
        }
#endif
    }
}
