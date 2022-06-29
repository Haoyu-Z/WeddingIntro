using UnityEngine;
using WeddingIntro.Ui;

namespace WeddingIntro.Character
{
    public class InteractiveWatcherQuest : InteractiveWatcherBase
    {
        [SerializeField]
        private string interactDialog;

        [SerializeField]
        private bool destroySelf;

        public override void InvokeInteract()
        {
            UIDialogBoxController.Instance.StartDialog(interactDialog, null);

            if (destroySelf)
            {
                Destroy(gameObject);
            }
        }
    }
}
