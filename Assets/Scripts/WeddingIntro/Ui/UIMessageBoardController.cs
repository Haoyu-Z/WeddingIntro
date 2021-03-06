using UnityEngine;
using UnityEngine.UI;
using WeddingIntro.Avatar;
using WeddingIntro.Utility;

namespace WeddingIntro.Ui
{
    public class UIMessageBoardController : MonoBehaviour
    {
        private static UIMessageBoardController instance;

        public static UIMessageBoardController Instance => instance;

        [SerializeField]
        private UIDialogPanelScaler panelScaler;

        [SerializeField]
        private InputField messageInputField;

        [SerializeField]
        private Button sendButton;

        [SerializeField]
        private Button cancelButton;

        private enum MessageBoardState
        {
            Hidden,
            ScaleIn,
            Shown,
            ScaleOut,
        }

        private MessageBoardState state;

        private void Awake()
        {
            Debug.Assert(instance == null);
            instance = this;

            state = MessageBoardState.Hidden;
            sendButton.onClick.AddListener(SendButtonClicked);
            cancelButton.onClick.AddListener(CancelButtonClicked);
        }

        public void ShowMessageBoard()
        {
            if (state != MessageBoardState.Hidden)
            {
                return;
            }

            Debug.Assert(GameStatics.Instance != null);
            AvatarInput avatarInput = GameStatics.Instance.PlayerAvatarInput;
            Debug.Assert(avatarInput != null);
            avatarInput.AddKeyResponse(DirectionKeyResponsePriority.MessageBoardFocus, (_) => { }, KeyPressType.DefaultPressing);
            avatarInput.AddKeyResponse(InteractionKeyPriority.MessageBoardFocus, (_) => { }, KeyPressType.DefaultPressing);

            messageInputField.text = "";
            state = MessageBoardState.ScaleIn;
            panelScaler.TriggerScaleIn(() => { state = MessageBoardState.Shown; });
        }

        private void SendButtonClicked()
        {
            if (state != MessageBoardState.Shown)
            {
                return;
            }

            AudioManager.Instance.PlayerSoundEffect("MessageSent");
            string messageText = messageInputField.text;
            if (messageText.Trim().Length > 0 && GameStatics.Instance.SendMailOnMessageBoard)
            {
                Mailer.Instance.SendMail(Mailer.MailType.MessageBoard, new Mailer.IJsonSerializable[] { GameStatics.Instance.PlayerInformation, new Mailer.SingleValueSerializableField<string>("MessageBoardText", messageText), });
            }

            CommonScaleOut();
        }

        private void CancelButtonClicked()
        {
            if (state != MessageBoardState.Shown)
            {
                return;
            }

            CommonScaleOut();
        }

        private void CommonScaleOut()
        {
            state = MessageBoardState.ScaleOut;
            panelScaler.TriggerScaleOut(() =>
            {
                state = MessageBoardState.Hidden;

                Debug.Assert(GameStatics.Instance != null);
                AvatarInput avatarInput = GameStatics.Instance.PlayerAvatarInput;
                Debug.Assert(avatarInput != null);
                avatarInput.RemoveKeyResponse(DirectionKeyResponsePriority.MessageBoardFocus);
                avatarInput.RemoveKeyResponse(InteractionKeyPriority.MessageBoardFocus);
            });
        }
    }
}
