using UnityEngine;

namespace WeddingIntro.Utility
{
    public class ItemMove : MonoBehaviour
    {
        [SerializeField]
        private float fallSpeed;

        [SerializeField, Tooltip("This corresponds to 'Eat' animation.")]
        private float fallDuration;

        [SerializeField]
        private float FallRelativeHeight = 1.5f;

        private float fallStartTime;

        private Vector3 fallToLocation;

        public Vector3 CurrentLocation => fallToLocation + new Vector3(0.0f, (fallDuration - (Time.time - fallStartTime)) * fallSpeed, 0.0f);

        private string destroyAudioKey;

        public void FallTo(Vector3 target, string destroyAudio = null)
        {
            fallStartTime = Time.time;
            fallToLocation = target + new Vector3(0.0f, FallRelativeHeight, 0.0f);
            destroyAudioKey = destroyAudio;
        }

        private void Update()
        {
            if (Time.time - fallStartTime >= fallDuration)
            {
                Destroy(gameObject);
                AudioManager.Instance.PlayerSoundEffect(destroyAudioKey);
                return;
            }

            transform.position = CurrentLocation;
        }
    }
}
