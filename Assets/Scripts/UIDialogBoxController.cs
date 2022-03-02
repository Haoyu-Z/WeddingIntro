using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIDialogBoxController : MonoBehaviour
{
    public UIDialogTextPopper textPopper;

    public UIDialogPanelScaler panelScaler;

    public delegate void UIDialogPanelFinishEvent();

    private enum DialogBoxState
    {
        Hidden,
        ScaleIn,
        TextPopping,
        TextShown,
        ScaleOut,
    }

    private DialogBoxState state;

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.J))
        {
            TriggerNextState();
        }
    }

    private void TriggerNextState()
    {
        if(textPopper==null||panelScaler==null)
        {
            return;
        }

        switch(state)
        {
            case DialogBoxState.Hidden:
                textPopper.ResetText();
                panelScaler.TriggerScaleIn(new UIDialogPanelFinishEvent(()=>
                {
                    state = DialogBoxState.TextPopping;
                    textPopper.TriggerText(new UIDialogPanelFinishEvent(() =>
                    {
                        state = DialogBoxState.TextShown;
                    }));
                }));
                state = DialogBoxState.ScaleIn;
                break;
            case DialogBoxState.ScaleIn:
                panelScaler.ForceFinishScaleIn(false);
                textPopper.TriggerText(new UIDialogPanelFinishEvent(() =>
                {
                    state = DialogBoxState.TextShown;
                }));
                state = DialogBoxState.TextPopping;
                break;
            case DialogBoxState.TextPopping:
                textPopper.ForceFinish(false);
                state = DialogBoxState.TextShown;
                break;
            case DialogBoxState.TextShown:
                panelScaler.TriggerScaleOut(new UIDialogPanelFinishEvent(() =>
                {
                    state = DialogBoxState.Hidden;
                    textPopper.ResetText();
                }));
                state = DialogBoxState.ScaleOut;
                break;
            case DialogBoxState.ScaleOut:
                break;
        }
    }
}
