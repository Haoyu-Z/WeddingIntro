using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractiveWatcherDialog : InteractiveWatcherBase
{
    [SerializeField]
    private string dialogEntrance;

    [SerializeField]
    private string dialogVoice;

    public override void InvokeInteract()
    {
        UIDialogBoxController.Instance.StartDialog(dialogEntrance, dialogVoice);
    }
}
