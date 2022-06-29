using UnityEngine;
using System.Collections.Generic;
using WeddingIntro.Character;
using WeddingIntro.Ui;

namespace WeddingIntro.Avatar
{
    public class AvatarInteraction : MonoBehaviour
    {
        [SerializeField]
        private float interactAngle = 30.0f;

        [SerializeField]
        private float interactDistance = 1.1f;

        [SerializeField]
        private bool logInteraction = false;

        private readonly HashSet<InteractiveWatcherBase> watchers = new HashSet<InteractiveWatcherBase>();

        private AvatarMovementComponent avatarMovementComponent;

        private AvatarInput avatarInput;

        private void Start()
        {
            avatarMovementComponent = GetComponent<AvatarMovementComponent>();

            avatarInput = GetComponent<AvatarInput>();
            Debug.Assert(avatarInput != null);
            avatarInput.AddKeyResponse(InteractionKeyPriority.InteractiveWatcher, new AvatarInput.KeyResponse(CheckInteraction), KeyPressType.KeyDown);
        }

        private void CheckInteraction(GameKeyCode _ = GameKeyCode.KeyA)
        {
            if (avatarMovementComponent == null)
            {
                return;
            }

            foreach (InteractiveWatcherBase watcher in watchers)
            {
                Vector3 relative = watcher.InteractCenter.transform.position - gameObject.transform.position;
                relative.z = 0.0f;

                float angle = Mathf.Atan2(relative.x, relative.y) / Mathf.PI * 180;
                float angleDiff = angle - avatarMovementComponent.FacingDirectionAngle;
                angleDiff -= Mathf.RoundToInt(angleDiff / 360.0f) * 360.0f;

                if (logInteraction)
                {
                    UIDebugText.Instance.AddDebugText($"i am at ({gameObject.transform.position.x},{gameObject.transform.position.y}), {watcher} is at ({watcher.InteractCenter.transform.position.x},{watcher.InteractCenter.transform.position.y}). Magnitude={relative.magnitude}, Angle={angleDiff}.", UIDebugText.DebugTextLevel.Log);
                }

                if (relative.magnitude <= interactDistance && Mathf.Abs(angleDiff) <= interactAngle)
                {
                    watcher.InvokeInteract();
                    return;
                }
            }

            if (logInteraction)
            {
                UIDebugText.Instance.AddDebugText("No interaction triggered.", UIDebugText.DebugTextLevel.Warning);
            }
        }

        public void RegisterInteractiveWatcher(InteractiveWatcherBase watcher)
        {
            watchers.Add(watcher);
        }

        public void RemoveInteractiveWatcher(InteractiveWatcherBase watcher)
        {
            watchers.Remove(watcher);
        }
    }
}
