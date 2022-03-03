using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIDialogBoxController : MonoBehaviour
{
    public static UIDialogBoxController Instance;

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

    private DialogEntry currentDialogEntry;

    private void Awake()
    {
        Debug.Assert(Instance == null);
        Instance = this;
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.K))
        {
            TriggerNextState();
        }

        if(Input.GetKeyDown(KeyCode.UpArrow))
        {
            ChangeDialogSelection(true);
        }

        if(Input.GetKeyDown(KeyCode.DownArrow))
        {
            ChangeDialogSelection(false);
        }
    }

    public void StartDialog(string dialogId)
    {
        if (state != DialogBoxState.Hidden)
        {
            return;
        }

        DialogEntry dialogEntry = DialogData.Instance.LookupDialog(dialogId);
        if (dialogEntry == null)
        {
            return;
        }

        currentDialogEntry = dialogEntry;
        TriggerNextState();
    }

    private void ChangeDialogSelection(bool inverse = false)
    {
        if (textPopper.TextType == UIDialogTextPopper.DialogTextType.TextWithSelector && state == DialogBoxState.TextShown && textPopper != null)
        {
            textPopper.NextSelection(inverse);
        }
    }

    private void TriggerNextState()
    {
        if (textPopper == null || panelScaler == null)
        {
            return;
        }

        switch (state)
        {
            case DialogBoxState.Hidden:
                textPopper.ResetText(currentDialogEntry);
                panelScaler.TriggerScaleIn(new UIDialogPanelFinishEvent(StartPopTextEventImpl));
                state = DialogBoxState.ScaleIn;
                break;
            case DialogBoxState.ScaleIn:
                panelScaler.ForceFinishScaleIn(false);
                StartPopTextEventImpl();
                break;
            case DialogBoxState.TextPopping:
                textPopper.ForceFinish(false);
                state = DialogBoxState.TextShown;
                break;
            case DialogBoxState.TextShown:
                string nextDialogId = "";
                if (textPopper.TextType == UIDialogTextPopper.DialogTextType.PlainText)
                {
                    nextDialogId = currentDialogEntry.NextDialogId;
                }
                else
                {
                    Debug.Assert(textPopper.SelectedIndex >= 0 && textPopper.SelectedIndex < currentDialogEntry.SelectionsNextDialogId.Length);
                    nextDialogId = currentDialogEntry.SelectionsNextDialogId[textPopper.SelectedIndex];
                }

                DialogEntry dialogEntry = DialogData.Instance.LookupDialog(nextDialogId);
                if (dialogEntry != null)
                {
                    currentDialogEntry = dialogEntry;
                    textPopper.ResetText(currentDialogEntry);
                    StartPopTextEventImpl();
                }
                else
                {
                    StartScaleOutEventImpl();
                }
                break;
            case DialogBoxState.ScaleOut:
                break;
        }
    }

    private void StartPopTextEventImpl()
    {
        state = DialogBoxState.TextPopping;
        textPopper.TriggerText(new UIDialogPanelFinishEvent(() =>
        {
            state = DialogBoxState.TextShown;
        }));
    }

    private void StartScaleOutEventImpl()
    {
        panelScaler.TriggerScaleOut(new UIDialogPanelFinishEvent(() =>
        {
            state = DialogBoxState.Hidden;
            textPopper.ResetText(null);
        }));
        state = DialogBoxState.ScaleOut;
    }
}
