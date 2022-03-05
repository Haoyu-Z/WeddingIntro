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

    private AvatarState playerAvatarState;

    public AvatarState PlayerAvatarState { get { return playerAvatarState; } }

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
