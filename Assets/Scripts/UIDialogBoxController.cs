using UnityEngine;

public class UIDialogBoxController : MonoBehaviour
{
    private static UIDialogBoxController instance;

    public static UIDialogBoxController Instance { get { return instance; } }

    [SerializeField]
    private UIDialogTextPopper textPopper;

    [SerializeField]
    private UIDialogPanelScaler panelScaler;

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

    private AvatarInput avatarInput;

    private void Awake()
    {
        Debug.Assert(instance == null);
        instance = this;
    }

    private void Start()
    {
        avatarInput = GameStatics.Instance.PlayerAvatarInput;
        Debug.Assert(avatarInput != null);
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

        avatarInput.AddKeyResponse(InteractionKeyPriority.DialogConfirm, new AvatarInput.KeyResponse(TriggerNextState), KeyPressType.KeyDown);
        avatarInput.AddKeyResponse(DirectionKeyResponsePriority.ChangeDialogSelection, new AvatarInput.KeyResponse(ChangeDialogSelectionWrapper), KeyPressType.KeyDown);

        currentDialogEntry = dialogEntry;
        TriggerNextState();
    }

    private void ChangeDialogSelectionWrapper(GameKeyCode gameKeyCode)
    {
        if(gameKeyCode == GameKeyCode.DirectionUp)
        {
            ChangeDialogSelection(true);
        }
        else if(gameKeyCode == GameKeyCode.DirectionDown)
        {
            ChangeDialogSelection(false);
        }
    }

    private void ChangeDialogSelection(bool inverse = false)
    {
        if (textPopper.TextType == UIDialogTextPopper.DialogTextType.TextWithSelector && state == DialogBoxState.TextShown && textPopper != null)
        {
            textPopper.NextSelection(inverse);
        }
    }

    private void TriggerNextState(GameKeyCode _ = GameKeyCode.KeyA)
    {
        Debug.Assert(textPopper != null && panelScaler != null);

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
                Debug.Assert(currentDialogEntry != null);

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

                DialogEntry nextDialogEntry = DialogData.Instance.LookupDialog(nextDialogId);
                if (nextDialogEntry != null)
                {
                    currentDialogEntry = nextDialogEntry;
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

        avatarInput.RemoveKeyResponse(InteractionKeyPriority.DialogConfirm);
        avatarInput.RemoveKeyResponse(DirectionKeyResponsePriority.ChangeDialogSelection);
    }
}
