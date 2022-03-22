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

    private AvatarInput playerAvatarInput;

    public AvatarInput PlayerAvatarInput { get { return playerAvatarInput; } }

    private void Awake()
    {
        Debug.Assert(Instance == null);
        Instance = this;

        if (PlayerAvatar != null)
        {
            playerAvatarMovement = PlayerAvatar.GetComponent<AvatarMovementComponent>();
            playerAvatarInteraction = PlayerAvatar.GetComponent<AvatarInteraction>();
            playerAvatarInput = PlayerAvatar.GetComponent<AvatarInput>();
        }
    }

    private void OnDestroy()
    {
        if (Instance == this)
        {
            Instance = null;
        }
    }
}
