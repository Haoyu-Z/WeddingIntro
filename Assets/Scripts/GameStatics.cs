using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameStatics : MonoBehaviour
{
    public static GameStatics Instance;

    public GameObject PlayerAvatar;

    public CameraFollow Camera;

    private AvatarMovementComponent playerAvatarMovement;

    public AvatarMovementComponent PlayerAvatarMovement { get { return playerAvatarMovement; } }

    private AvatarInteraction playerAvatarInteraction;

    public AvatarInteraction PlayerAvatarInteraction { get { return playerAvatarInteraction; } }

    private void Awake()
    {
        Debug.Assert(Instance == null);
        Instance = this;

        if (PlayerAvatar != null)
        {
            playerAvatarMovement = PlayerAvatar.GetComponent<AvatarMovementComponent>();
            playerAvatarInteraction = PlayerAvatar.GetComponent<AvatarInteraction>();
        }
    }
}
