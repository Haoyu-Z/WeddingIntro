using UnityEngine;
using WeddingIntro.Avatar;
using WeddingIntro.Ui;
using WeddingIntro.Utility;

namespace WeddingIntro.Character
{
    public class InteractiveWatcherAvatarAction : InteractiveWatcherDialog
    {
        private enum AudioPlayEventType
        {
            AtStart,
            AtDestroy,
        }

        [SerializeField]
        private AvatarAnimationControlComponent.ActionType action;

        [SerializeField]
        private Sprite[] foodSprite;

        [SerializeField]
        private GameObject foodObject;

        [SerializeField]
        private string audioKey;

        [SerializeField]
        private AudioPlayEventType audioPlayType;

        public override void InvokeInteract()
        {
            UIDialogBoxController.Instance.StartDialog(dialogEntrance, dialogVoice, PlayAction);
        }

        private void PlayAction(int selection)
        {
            AvatarInput avatarInput = GameStatics.Instance.PlayerAvatarInput;
            Debug.Assert(avatarInput != null);

            avatarInput.AddKeyResponse(DirectionKeyResponsePriority.UIFocus, (_) => { }, KeyPressType.DefaultPressing);
            avatarInput.AddKeyResponse(InteractionKeyPriority.UIFocus, (_) => { }, KeyPressType.DefaultPressing);

            AvatarAnimationControlComponent animationComponent = GameStatics.Instance.PlayerAvatarAnimation;
            Debug.Assert(avatarInput != null);

            animationComponent.PlayAction(action, () =>
            {
                avatarInput.RemoveKeyResponse(DirectionKeyResponsePriority.UIFocus);
                avatarInput.RemoveKeyResponse(InteractionKeyPriority.UIFocus);
            });

            if (audioPlayType == AudioPlayEventType.AtStart)
            {
                AudioManager.Instance.PlayerSoundEffect(audioKey);
            }

            if (selection < foodSprite.Length)
            {
                GameObject food = Instantiate(foodObject);
                ItemMove moveScript = food.GetComponent<ItemMove>();
                Debug.Assert(moveScript != null);

                moveScript.FallTo(avatarInput.transform.position, audioPlayType == AudioPlayEventType.AtDestroy ? audioKey : null);
                food.transform.position = moveScript.CurrentLocation;

                SpriteRenderer renderer = food.GetComponent<SpriteRenderer>();
                renderer.sprite = foodSprite[selection];
            }
        }
    }
}
