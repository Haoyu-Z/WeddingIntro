using UnityEngine;
using WeddingIntro.Ui;

namespace WeddingIntro.Character
{
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
}
