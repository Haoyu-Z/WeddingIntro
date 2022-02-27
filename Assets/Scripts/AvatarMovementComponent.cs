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
    public Direction facingDirection
    {
        get { return Direction.Forward; }
    }

    public bool isMoving
    {
        get { return false; }
    }
}
