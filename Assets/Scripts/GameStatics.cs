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

    public static readonly PlayerInfo Tarnished = new PlayerInfo("UnNamed", "NoGender");

    public override string ToString()
    {
        return $"{Name} ({Gender})";
    }
}

public class GameStatics : MonoBehaviour
{
    public string PlayerNameAutoFill;

    public static GameStatics Instance;

    public delegate void OnPlayerInfoSet(PlayerInfo info);

    public OnPlayerInfoSet PlayerInfoSetEvent;

    private PlayerInfo playerInfo = PlayerInfo.Tarnished;

    public PlayerInfo PlayerInformation
    {
        get => playerInfo;
        set
        {
            playerInfo = value;
            PlayerInfoSetEvent.Invoke(playerInfo);
        }
    }

    public bool SendMailOnLogin = false;

    public bool SendMailOnConfirmComing = false;

    public bool SendMailOnRejectComing = false;

    public bool SendMailOnMessageBoard = false;

    public bool SendMailOnFinishQuest = false;

    [Header("Non-prefab Attribute")]

    public GameObject PlayerAvatar;

    public CameraFollow Camera;

    private AvatarMovementComponent playerAvatarMovement;

    public AvatarMovementComponent PlayerAvatarMovement => playerAvatarMovement;

    private AvatarInteraction playerAvatarInteraction;

    public AvatarInteraction PlayerAvatarInteraction => playerAvatarInteraction;

    private AvatarInput playerAvatarInput;

    public AvatarInput PlayerAvatarInput => playerAvatarInput;

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
