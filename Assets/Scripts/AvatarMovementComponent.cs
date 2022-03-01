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
    public static readonly Vector2 PlayerRenderHaftRect = new Vector2(0.08f, 0.16f);

    public float MoveSpeed = 1;

    private Direction facingDirection = Direction.Backward;

    private bool isMoving = false;

    public Direction FacingDirection
    {
        get { return facingDirection; }
    }

    public bool IsMoving
    {
        get { return isMoving; }
    }

    private void Update()
    {
        Vector3 inputVector = new Vector3(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"), 0);
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

        gameObject.transform.Translate(MoveSpeed * Time.deltaTime * inputVector.normalized);
    }
}
