using UnityEngine;
using UnityEngine.UI;

namespace WeddingIntro.Ui
{
    public class UIButtonWithEnabledState : MonoBehaviour
    {
        [SerializeField]
        private Sprite enabledSprite;

        [SerializeField]
        private Sprite diabledSprite;

        private Image imageComponent;

        private void Awake()
        {
            imageComponent = GetComponent<Image>();
            Debug.Assert(imageComponent != null);
        }

        public void ChangeState(bool enabled)
        {
            imageComponent.sprite = enabled ? enabledSprite : diabledSprite;
        }
    }
}
