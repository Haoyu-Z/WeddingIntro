using UnityEngine;

public struct PlayerInfo
{
    public string Name;

    public string Gender;

    public PlayerInfo(string name, string gender)
    {
        Debug.Assert(name != null && gender != null);
        Name = name;
        Gender = gender;
    }

    public static readonly string NoNameHint = "üo√˚ œ£ø";

    public static readonly PlayerInfo Tarnished = new PlayerInfo("UnNamed", "NoGender");
}

public class GameStatics : MonoBehaviour
{
    public static GameStatics Instance;

    public PlayerInfo PlayerInformation { get; set; } = PlayerInfo.Tarnished;

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
