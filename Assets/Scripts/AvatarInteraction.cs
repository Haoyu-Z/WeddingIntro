using UnityEngine;
using System.Collections.Generic;

public class AvatarInteraction : MonoBehaviour
{
    private readonly HashSet<InteractiveWatcher> watchers = new HashSet<InteractiveWatcher>();

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.J))
        {
            CheckInteraction();
        }
    }

    private void CheckInteraction()
    {
        if (GameStatics.Instance?.PlayerAvatarMovement == null)
        {
            return;
        }

        AvatarMovementComponent movement = GameStatics.Instance.PlayerAvatarMovement;

        foreach (InteractiveWatcher watcher in watchers)
        {
            Vector3 relative = watcher.gameObject.transform.position - gameObject.transform.position;
            if (relative.magnitude > 1.0f)
            {
                continue;
            }

            float angle = Mathf.Atan2(relative.x, relative.y) / Mathf.PI * 180;
            float angleDiff = angle - movement.FacingDirectionAngle;
            angleDiff -= Mathf.RoundToInt(angleDiff / 360.0f) * 360.0f;
            if (Mathf.Abs(angleDiff) < 30.0f)
            {
                watcher.InvokeInteract();
                return;
            }
        }

        Debug.Log("No interaction triggered.");
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
