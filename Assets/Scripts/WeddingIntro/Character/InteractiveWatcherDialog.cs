using UnityEngine;
using WeddingIntro.Ui;

namespace WeddingIntro.Character
{
    public class InteractiveWatcherDialog : InteractiveWatcherBase
    {
        [SerializeField]
        protected string dialogEntrance;

        [SerializeField]
        protected string dialogVoice;

        public override void InvokeInteract()
        {
            UIDialogBoxController.Instance.StartDialog(dialogEntrance, dialogVoice);
        }
    }
}
