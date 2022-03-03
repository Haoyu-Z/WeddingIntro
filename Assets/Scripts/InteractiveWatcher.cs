using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum InteractiveType
{
    Dialog,
}

public class InteractiveWatcher : MonoBehaviour
{
    public InteractiveType interactiveType;

    public string dialogEntrance;

    private void Start()
    {
        GameStatics.Instance?.PlayerAvatarInteraction?.RegisterInteractiveWatcher(this);
    }

    private void OnDestroy()
    {
        GameStatics.Instance?.PlayerAvatarInteraction?.RegisterInteractiveWatcher(this);
    }

    public void InvokeInteract()
    {
        if(interactiveType == InteractiveType.Dialog)
        {
            UIDialogBoxController.Instance.StartDialog(dialogEntrance);
        }
    }
}
