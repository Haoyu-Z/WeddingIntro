using UnityEngine;

public abstract class InteractiveWatcherBase : MonoBehaviour
{
    private void Start()
    {
        GameStatics.Instance?.PlayerAvatarInteraction?.RegisterInteractiveWatcher(this);
    }

    private void OnDestroy()
    {
        GameStatics.Instance?.PlayerAvatarInteraction?.RemoveInteractiveWatcher(this);
    }

    public abstract void InvokeInteract();
}