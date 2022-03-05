using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Direction
{
    Forward = 0,
    Rightward = 1,
    Backward = 2,
    Leftward = 3,
}

public class AvatarMovementComponent : MonoBehaviour
{
    public static readonly Vector2 PlayerRenderHaftRect = new Vector2(0.5f, 1.0f);

    private static readonly float[] directionAngles = new float[] { 0.0f, 90.0f, 180.0f, -90.0f };

    public float MoveSpeed = 3.2f;

    public float SpeedFade = 0.8f;

    public Vector2 playerMovementRangeX;

    public Vector2 playerMovementRangeY;

    private Direction facingDirection = Direction.Backward;

    private bool isMoving = false;

    private Rigidbody2D rigidBody2D;

    public Direction FacingDirection
    {
        get { return facingDirection; }
    }

    public bool IsMoving
    {
        get { return isMoving; }
    }

    public float FacingDirectionAngle
    {
        get { return directionAngles[(int)facingDirection]; }
    }

    private Vector3 movementInputVector
    {
        get { return new Vector3(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"), 0); }
    }

    private void Start()
    {
        rigidBody2D = GetComponent<Rigidbody2D>();

        float ScreenBorder = 0.0f;
        CameraFollow cameraFollow = GameStatics.Instance?.Camera;
        Debug.Assert(cameraFollow != null);

        ScreenBorder = cameraFollow.ScreenBorder;
        playerMovementRangeX = new Vector2(
            cameraFollow.PlayerRenderBorderX.x + ScreenBorder + PlayerRenderHaftRect.x,
            cameraFollow.PlayerRenderBorderX.y - ScreenBorder - PlayerRenderHaftRect.x
            );
        playerMovementRangeY = new Vector2(
            cameraFollow.PlayerRenderBorderY.x + ScreenBorder,
            cameraFollow.PlayerRenderBorderY.y - ScreenBorder - PlayerRenderHaftRect.y - PlayerRenderHaftRect.y
            );
    }

    private void FixedUpdate()
    {
        if (rigidBody2D == null)
        {
            return;
        }

        Vector3 inputVector = movementInputVector;
        if (inputVector.x != 0 || inputVector.y != 0)
        {
            rigidBody2D.velocity = (new Vector2(inputVector.x, inputVector.y)).normalized * MoveSpeed;
        }
        else
        {
            rigidBody2D.velocity = Vector2.Lerp(rigidBody2D.velocity, Vector2.zero, SpeedFade);
        }
    }

    private void Update()
    {
        Vector3 inputVector = movementInputVector;
        isMoving = inputVector.x != 0 || inputVector.y != 0;
        if (isMoving)
        {
            if (Mathf.Abs(inputVector.x) >= Mathf.Abs(inputVector.y))
            {
                facingDirection = inputVector.x > 0 ? Direction.Rightward : Direction.Leftward;
            }
            else
            {
                facingDirection = inputVector.y > 0 ? Direction.Forward : Direction.Backward;
            }
        }

        CameraFollow cameraFollow = GameStatics.Instance?.Camera;
        if (cameraFollow != null)
        {
            Vector3 position = gameObject.transform.position;
            position.x = Mathf.Clamp(position.x, playerMovementRangeX.x, playerMovementRangeX.y);
            position.y = Mathf.Clamp(position.y, playerMovementRangeY.x, playerMovementRangeY.y);
            gameObject.transform.position = position;
        }
    }
}
