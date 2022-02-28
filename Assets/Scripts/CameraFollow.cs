using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Vector2 RangeHalfRect = new Vector2((float)1.5, (float)0.8);

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

        Vector2 RangeRectOfCam = RangeHalfRect * camera.orthographicSize;
        Vector2 PlayerHalfRect = AvatarMovementComponent.PlayerRenderHaftRect;

        Vector3 PlayerToCamera = gameObject.transform.position - PlayerAvatar.transform.position;
        PlayerToCamera.x = Mathf.Clamp(PlayerToCamera.x, -RangeRectOfCam.x + PlayerHalfRect.x, RangeRectOfCam.x - PlayerHalfRect.x);
        PlayerToCamera.y = Mathf.Clamp(PlayerToCamera.y, -RangeRectOfCam.y + PlayerHalfRect.y, RangeRectOfCam.y - PlayerHalfRect.y);

        gameObject.transform.position = PlayerAvatar.transform.position + PlayerToCamera;
    }
}
