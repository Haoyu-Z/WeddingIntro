using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public float ScreenBorder;

    public GameObject PlayerAvatar;

#pragma warning disable CS0108 // Member hides inherited member; missing new keyword
    private Camera camera;
#pragma warning restore CS0108 // Member hides inherited member; missing new keyword

    private void Start()
    {
        camera = GetComponent<Camera>();
    }

    void LateUpdate()
    {
        if (PlayerAvatar == null || camera == null)
        {
            return;
        }

        Vector3 playerPosition = PlayerAvatar.transform.position;
        Vector3 cameraPosition = gameObject.transform.position;
        Vector2 playerHalfRect = AvatarMovementComponent.PlayerRenderHaftRect;

        float newCameraX = Mathf.Clamp(
            cameraPosition.x,
            playerPosition.x + playerHalfRect.x + ScreenBorder - camera.orthographicSize * camera.aspect,
            playerPosition.x - playerHalfRect.x - ScreenBorder + camera.orthographicSize * camera.aspect);
        float newCameraY = Mathf.Clamp(
            cameraPosition.y,
            playerPosition.y + playerHalfRect.y + ScreenBorder - camera.orthographicSize,
            playerPosition.y - playerHalfRect.y - ScreenBorder + camera.orthographicSize);
        gameObject.transform.position = new Vector3(newCameraX, newCameraY, cameraPosition.z);
    }
}
