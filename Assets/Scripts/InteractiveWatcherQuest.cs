using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractiveWatcherQuest : InteractiveWatcherBase
{
    [SerializeField]
    private string interactDialog;

    [SerializeField]
    private bool destroySelf;

    public override void InvokeInteract()
    {
        UIDialogBoxController.Instance.StartDialog(interactDialog, null);

        if(destroySelf)
        {
            Destroy(gameObject);
        }
    }
}
