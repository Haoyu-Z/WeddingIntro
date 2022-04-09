using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractiveWatcherMessageBoard : InteractiveWatcherBase
{
    public override void InvokeInteract()
    {
        UIMessageBoardController.Instance.ShowMessageBoard();
    }
}
