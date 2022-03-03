using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [Tooltip("Measured in absolute game unit.")]
    public float ScreenBorder;

    private GameObject playerAvatar;

#pragma warning disable CS0108 // Member hides inherited member; missing new keyword
    private Camera camera;
#pragma warning restore CS0108 // Member hides inherited member; missing new keyword

    private void Start()
    {
        camera = GetComponent<Camera>();

        playerAvatar = GameStatics.Instance?.PlayerAvatar;
        if (playerAvatar != null)
        {
            Vector3 viewPosition = playerAvatar.transform.position;
            viewPosition.y += AvatarMovementComponent.PlayerRenderHaftRect.y;
            viewPosition.z = gameObject.transform.position.z;

            gameObject.transform.position = viewPosition;
        }
    }

    void LateUpdate()
    {
        if (playerAvatar == null || camera == null)
        {
            return;
        }

        Vector3 playerPosition = playerAvatar.transform.position;
        Vector3 cameraPosition = gameObject.transform.position;
        Vector2 playerHalfRect = AvatarMovementComponent.PlayerRenderHaftRect;

        float newCameraX = Mathf.Clamp(
            cameraPosition.x,
            playerPosition.x + playerHalfRect.x + ScreenBorder - camera.orthographicSize * camera.aspect,
            playerPosition.x - playerHalfRect.x - ScreenBorder + camera.orthographicSize * camera.aspect);
        float newCameraY = Mathf.Clamp(
            cameraPosition.y,
            playerPosition.y + playerHalfRect.y + playerHalfRect.y + ScreenBorder - camera.orthographicSize,
            playerPosition.y - ScreenBorder + camera.orthographicSize);
        gameObject.transform.position = new Vector3(newCameraX, newCameraY, cameraPosition.z);
    }
}
