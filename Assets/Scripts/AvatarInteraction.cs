using UnityEngine;
using System.Collections.Generic;

public class AvatarInteraction : MonoBehaviour
{
    [SerializeField]
    private float interactAngle = 30.0f;

    [SerializeField]
    private float interactDistance = 1.1f;

    private readonly HashSet<InteractiveWatcher> watchers = new HashSet<InteractiveWatcher>();

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

        foreach (InteractiveWatcher watcher in watchers)
        {
            Vector3 relative = watcher.gameObject.transform.position - gameObject.transform.position;
            if (relative.magnitude > interactDistance)
            {
                continue;
            }

            float angle = Mathf.Atan2(relative.x, relative.y) / Mathf.PI * 180;
            float angleDiff = angle - avatarMovementComponent.FacingDirectionAngle;
            angleDiff -= Mathf.RoundToInt(angleDiff / 360.0f) * 360.0f;
            if (Mathf.Abs(angleDiff) < interactAngle)
            {
                watcher.InvokeInteract();
                return;
            }
        }

        GameStatics.Instance.UIDebugText.AddDebugText("No interaction triggered.");
    }

    public void RegisterInteractiveWatcher(InteractiveWatcher watcher)
    {
        watchers.Add(watcher);
    }

    public void RemoveInteractiveWatch(InteractiveWatcher watcher)
    {
        watchers.Remove(watcher);
    }
}
