using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [Tooltip("Measured in absolute game unit.")]
    public float ScreenBorder = 0.375f;

    public Vector2 PlayerRenderBorderX = new Vector2(-11.0f, 12.0f);

    public Vector2 PlayerRenderBorderY = new Vector3(-13.0f, 14.0f);

    private Vector2 cameraBorderX;

    private Vector2 cameraBorderY;

    private GameObject playerAvatar;

#pragma warning disable CS0108 // Member hides inherited member; missing new keyword
    private Camera camera;
#pragma warning restore CS0108 // Member hides inherited member; missing new keyword

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

        AvatarMovementComponent playerMovement = GameStatics.Instance?.PlayerAvatarMovement;
        if (playerMovement != null)
        {
            cameraBorderX = new Vector2(PlayerRenderBorderX.x + camera.orthographicSize * camera.aspect, PlayerRenderBorderX.y - camera.orthographicSize * camera.aspect);
            cameraBorderY = new Vector2(PlayerRenderBorderY.x + camera.orthographicSize, PlayerRenderBorderY.y - camera.orthographicSize);
        }
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
