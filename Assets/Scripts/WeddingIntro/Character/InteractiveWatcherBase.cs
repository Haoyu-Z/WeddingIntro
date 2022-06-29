using UnityEngine;
using WeddingIntro.Utility;

namespace WeddingIntro.Character
{
    public abstract class InteractiveWatcherBase : MonoBehaviour
    {
        [SerializeField]
        private GameObject interactCenter;

        public GameObject InteractCenter => interactCenter == null ? gameObject : interactCenter;

        protected void Start()
        {
            GameStatics.Instance?.PlayerAvatarInteraction?.RegisterInteractiveWatcher(this);
        }

        private void OnDestroy()
        {
            GameStatics.Instance?.PlayerAvatarInteraction?.RemoveInteractiveWatcher(this);
        }

        public abstract void InvokeInteract();
    }
}
