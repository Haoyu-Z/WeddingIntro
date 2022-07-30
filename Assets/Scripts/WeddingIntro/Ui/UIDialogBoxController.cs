using UnityEngine;
using WeddingIntro.Avatar;
using WeddingIntro.Data;
using WeddingIntro.Utility;

namespace WeddingIntro.Ui
{
    public class UIDialogBoxController : MonoBehaviour
    {
        private static UIDialogBoxController instance;

        public static UIDialogBoxController Instance => instance;

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

        private DialogEntry CurrentDialogEntry
        {
            get { return currentDialogEntry; }
            set
            {
                currentDialogEntry = value;
                if (value.TriggerWorldEvent != WorldEvent.WorldEventType.None)
                {
                    WorldEvent.TriggerEvent(value.TriggerWorldEvent);
                }
                if (value.StandaloneAudioEffectKey != null && value.StandaloneAudioEffectKey.Length > 0)
                {
                    AudioManager.Instance.PlayerSoundEffect(value.StandaloneAudioEffectKey);
                }
            }
        }

        private AvatarInput avatarInput;

        private string currentSpeechVoiceId;

        private System.Action<int> dialogBoxEndCallback;

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

        public void StartDialog(string dialogId, string speechVoiceId, System.Action<int> dialogEndCallback = null)
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

            CurrentDialogEntry = dialogEntry;
            currentSpeechVoiceId = speechVoiceId;

            dialogBoxEndCallback = dialogEndCallback;

            TriggerNextState();
        }

        private void ChangeDialogSelectionWrapper(GameKeyCode gameKeyCode)
        {
            if (gameKeyCode == GameKeyCode.DirectionUp)
            {
                ChangeDialogSelection(true);
            }
            else if (gameKeyCode == GameKeyCode.DirectionDown)
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
                    textPopper.ResetText(CurrentDialogEntry);
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
                    AudioManager.Instance.StopSpeechVoice();
                    break;
                case DialogBoxState.TextShown:
                    Debug.Assert(CurrentDialogEntry != null);

                    string nextDialogId = "";
                    int dialogEndCallbackSelection = 0;
                    if (textPopper.TextType == UIDialogTextPopper.DialogTextType.PlainText)
                    {
                        nextDialogId = CurrentDialogEntry.NextDialogId;
                    }
                    else
                    {
                        Debug.Assert(textPopper.SelectedIndex >= 0 && textPopper.SelectedIndex < CurrentDialogEntry.NextSelections.Length);
                        DialogNextSelection nextSelection = CurrentDialogEntry.NextSelections[textPopper.SelectedIndex];
                        dialogEndCallbackSelection = textPopper.SelectedIndex;

                        nextDialogId = nextSelection.NextDialogId;
                        foreach (ConditionedNextDialog conditionedNext in nextSelection.ConditionedDialogs)
                        {
                            if (conditionedNext.ConditionObject is IDialogCondition condition && condition.TestCondition())
                            {
                                nextDialogId = conditionedNext.NextDialogId;
                                break;
                            }
                        }
                    }

                    DialogEntry nextDialogEntry = DialogData.Instance.LookupDialog(nextDialogId);
                    if (nextDialogEntry != null)
                    {
                        CurrentDialogEntry = nextDialogEntry;
                        textPopper.ResetText(CurrentDialogEntry);
                        StartPopTextEventImpl();
                    }
                    else
                    {
                        StartScaleOutEventImpl(dialogEndCallbackSelection);
                    }
                    break;
                case DialogBoxState.ScaleOut:
                    break;
            }
        }

        private void StartPopTextEventImpl()
        {
            state = DialogBoxState.TextPopping;
            AudioManager.Instance.StartSpeechVoice(currentSpeechVoiceId);
            textPopper.TriggerText(new UIDialogPanelFinishEvent(() =>
            {
                state = DialogBoxState.TextShown;
                AudioManager.Instance.StopSpeechVoice();
            }));
        }

        private void StartScaleOutEventImpl(int callbackSelectionValue)
        {
            panelScaler.TriggerScaleOut(new UIDialogPanelFinishEvent(() =>
            {
                state = DialogBoxState.Hidden;
                textPopper.ResetText(null);

                dialogBoxEndCallback?.Invoke(callbackSelectionValue);
                dialogBoxEndCallback = null;
            }));
            state = DialogBoxState.ScaleOut;

            avatarInput.RemoveKeyResponse(InteractionKeyPriority.DialogConfirm);
            avatarInput.RemoveKeyResponse(DirectionKeyResponsePriority.ChangeDialogSelection);
        }
    }
}
