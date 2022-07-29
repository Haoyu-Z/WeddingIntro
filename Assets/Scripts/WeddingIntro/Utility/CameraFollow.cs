using UnityEngine;
using WeddingIntro.Avatar;

namespace WeddingIntro.Utility
{
    public class CameraFollow : MonoBehaviour
    {
        [SerializeField, Tooltip("Measured in world-space unit.")]
        private float screenBorder = 0.375f;

        public float ScreenBorder { get { return screenBorder; } }

        [SerializeField, Tooltip("Measured in scale ratio. This correspones to UI.GbaFace.ScreenSpace.")]
        private Vector2 uiBorderX = new Vector2(0.0f, 0.0f);

        [SerializeField, Tooltip("Measured in scale ratio. This correspones to UI.GbaFace.ScreenSpace.")]
        private Vector2 uiBorderY = new Vector2(0.0f, 0.0f);

        [SerializeField, Tooltip("Measured in world-space unit, as absolute value.")]
        private Vector2 playerRenderBorderX = new Vector2(-11.0f, 12.0f);

        public Vector2 PlayerRenderBorderX { get { return playerRenderBorderX; } }

        [SerializeField, Tooltip("Measured in world-space unit, as absolute value.")]
        private Vector2 playerRenderBorderY = new Vector3(-13.0f, 14.0f);

        public Vector2 PlayerRenderBorderY { get { return playerRenderBorderY; } }

        [SerializeField]
        private Vector2 springArmOffset;

        private Vector2 cameraBorderX;

        private Vector2 cameraBorderY;

        private GameObject playerAvatar;

        public static readonly Vector2 PlayerRenderHaftRect = new Vector2(0.5f, 1.0f);

        public Vector2 PlayerMovementRangeX => new Vector2(
                PlayerRenderBorderX.x + ScreenBorder + PlayerRenderHaftRect.x,
                PlayerRenderBorderX.y - ScreenBorder - PlayerRenderHaftRect.x);

        public Vector2 PlayerMovementRangeY => new Vector2(
                PlayerRenderBorderY.x + ScreenBorder,
                PlayerRenderBorderY.y - ScreenBorder - PlayerRenderHaftRect.y - PlayerRenderHaftRect.y);

#pragma warning disable CS0108 // Member hides inherited member; missing new keyword
        [SerializeField]
        private Camera camera;
#pragma warning restore CS0108 // Member hides inherited member; missing new keyword

        private void ResetCameraBorder()
        {
            AvatarMovementComponent playerMovement = GameStatics.Instance?.PlayerAvatarMovement;
            if (playerMovement != null)
            {
                // ViewBorder range <= PlayerRenderBorder range

                cameraBorderX = new Vector2(
                    PlayerRenderBorderX.x + (1.0f - uiBorderX.x * 2.0f) * camera.orthographicSize * camera.aspect,
                    PlayerRenderBorderX.y - (1.0f - uiBorderX.y * 2.0f) * camera.orthographicSize * camera.aspect);
                cameraBorderY = new Vector2(
                    PlayerRenderBorderY.x + (1.0f - uiBorderY.x * 2.0f) * camera.orthographicSize,
                    PlayerRenderBorderY.y - (1.0f - uiBorderY.y * 2.0f) * camera.orthographicSize);
            }
        }

        private void Start()
        {
            Debug.Assert(camera != null);

            playerAvatar = GameStatics.Instance?.PlayerAvatar;
            if (playerAvatar != null)
            {
                Vector3 viewPosition = playerAvatar.transform.position;
                viewPosition.y += PlayerRenderHaftRect.y;
                viewPosition.z = gameObject.transform.position.z;

                gameObject.transform.position = viewPosition;
            }

            ResetCameraBorder();
        }

        private void LateUpdate()
        {
            if (playerAvatar == null || camera == null)
            {
                return;
            }

            ResetCameraBorder();
            Vector3 playerPosition = playerAvatar.transform.position;
            Vector3 cameraPosition = new Vector3(playerPosition.x + springArmOffset.x, playerPosition.y + PlayerRenderHaftRect.y + springArmOffset.y, gameObject.transform.position.z);
            cameraPosition.x = Mathf.Clamp(cameraPosition.x, cameraBorderX.x, cameraBorderX.y);
            cameraPosition.y = Mathf.Clamp(cameraPosition.y, cameraBorderY.x, cameraBorderY.y);
            gameObject.transform.position = cameraPosition;
        }

#if UNITY_EDITOR
        private Vector2 ViewBorderX => new Vector2(
                transform.position.x - (1.0f - uiBorderX.x * 2.0f) * camera.orthographicSize * camera.aspect,
                transform.position.x + (1.0f - uiBorderX.y * 2.0f) * camera.orthographicSize * camera.aspect
            );

        private Vector2 ViewBorderY => new Vector2(
            transform.position.y - (1.0f - uiBorderY.x * 2.0f) * camera.orthographicSize,
            transform.position.y + (1.0f - uiBorderY.y * 2.0f) * camera.orthographicSize
            );

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.green;
            Vector3 playerRenderBorderCenter = new Vector3(
                (PlayerRenderBorderX.x + PlayerRenderBorderX.y) * 0.5f,
                (PlayerRenderBorderY.x + PlayerRenderBorderY.y) * 0.5f);
            Vector3 playerRenderBorderSize = new Vector3(
                PlayerRenderBorderX.y - PlayerRenderBorderX.x,
                PlayerRenderBorderY.y - PlayerRenderBorderY.x);
            Gizmos.DrawWireCube(playerRenderBorderCenter, playerRenderBorderSize);

            Gizmos.color = Color.green;
            Vector2 playerMovementRangeX = PlayerMovementRangeX;
            Vector2 playerMovementRangeY = PlayerMovementRangeY;
            Vector3 playerMovementRangeCenter = new Vector3(
                (playerMovementRangeX.x + playerMovementRangeX.y) * 0.5f,
                (playerMovementRangeY.x + playerMovementRangeY.y) * 0.5f);
            Vector3 playerMovementRangeSize = new Vector3(
                playerMovementRangeX.y - playerMovementRangeX.x,
                playerMovementRangeY.y - playerMovementRangeY.x);
            Gizmos.DrawWireCube(playerMovementRangeCenter, playerMovementRangeSize);

            Gizmos.color = Color.red;
            Vector2 viewBorderX = ViewBorderX;
            Vector2 viewBorderY = ViewBorderY;
            Vector3 viewBorderCenter = new Vector3(
                (viewBorderX.x + viewBorderX.y) * 0.5f,
                (viewBorderY.x + viewBorderY.y) * 0.5f);
            Vector3 viewBorderSize = new Vector3(
                viewBorderX.y - viewBorderX.x,
                viewBorderY.y - viewBorderY.x);
            Gizmos.DrawWireCube(viewBorderCenter, viewBorderSize);
        }
#endif
    }
}
