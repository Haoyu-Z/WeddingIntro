using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [SerializeField, Tooltip("Measured in absolute game unit.")]
    private float screenBorder = 0.375f;

    public float ScreenBorder { get { return screenBorder; } }

    [SerializeField]
    private Vector2 uiBorderX = new Vector2(0.0f, 0.0f);

    [SerializeField]
    private Vector2 uiBorderY = new Vector2(0.0f, 0.0f);

    [SerializeField]
    private Vector2 playerRenderBorderX = new Vector2(-11.0f, 12.0f);

    public Vector2 PlayerRenderBorderX { get { return playerRenderBorderX; } }

    [SerializeField]
    private Vector2 playerRenderBorderY = new Vector3(-13.0f, 14.0f);

    public Vector2 PlayerRenderBorderY { get { return playerRenderBorderY; } }

    private Vector2 cameraBorderX;

    private Vector2 cameraBorderY;

    private GameObject playerAvatar;

#pragma warning disable CS0108 // Member hides inherited member; missing new keyword
    private Camera camera;
#pragma warning restore CS0108 // Member hides inherited member; missing new keyword

    private void ResetCameraBorder()
    {
        AvatarMovementComponent playerMovement = GameStatics.Instance?.PlayerAvatarMovement;
        if (playerMovement != null)
        {
            cameraBorderX = new Vector2(PlayerRenderBorderX.x + camera.orthographicSize * camera.aspect - uiBorderX.x, PlayerRenderBorderX.y - camera.orthographicSize * camera.aspect + uiBorderX.y);
            cameraBorderY = new Vector2(PlayerRenderBorderY.x + camera.orthographicSize - uiBorderY.x, PlayerRenderBorderY.y - camera.orthographicSize + uiBorderY.y);
        }
    }

    private void Start()
    {
        camera = GetComponent<Camera>();
        Debug.Assert(camera != null);

        playerAvatar = GameStatics.Instance?.PlayerAvatar;
        if (playerAvatar != null)
        {
            Vector3 viewPosition = playerAvatar.transform.position;
            viewPosition.y += AvatarMovementComponent.PlayerRenderHaftRect.y;
            viewPosition.z = gameObject.transform.position.z;

            gameObject.transform.position = viewPosition;
        }

        ResetCameraBorder();
    }

    void LateUpdate()
    {
        if (playerAvatar == null || camera == null)
        {
            return;
        }

        Vector3 playerPosition = playerAvatar.transform.position;
        Vector3 cameraPosition = new Vector3(playerPosition.x, playerPosition.y + AvatarMovementComponent.PlayerRenderHaftRect.y, gameObject.transform.position.z);
        cameraPosition.x = Mathf.Clamp(cameraPosition.x, cameraBorderX.x, cameraBorderX.y);
        cameraPosition.y = Mathf.Clamp(cameraPosition.y, cameraBorderY.x, cameraBorderY.y);
        gameObject.transform.position = cameraPosition;
    }
}
