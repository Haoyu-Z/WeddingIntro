using UnityEngine;
using WeddingIntro.Data;
using WeddingIntro.Character;

namespace WeddingIntro.Utility
{
    [System.Serializable]
    public class AudioEntry
    {
        public string EntryName;

        public AudioClip ClipAsset;
    }

    public class AudioManager : MonoBehaviour
    {
        [SerializeField]
        private PieceOfAudio audioData;

        [SerializeField]
        private AudioSource backgroundAudioSource;

        [SerializeField]
        private AudioSource effectAudioSource;

        [SerializeField]
        private SpeechAudio speechAudio;

        private static AudioManager instance;

        public static AudioManager Instance => instance;


        private void Awake()
        {
            Debug.Assert(instance == null);
            instance = this;

            WorldEvent.RegisterEvent(WorldEvent.WorldEventType.Login, new System.Action(() => { PlayBackgroundMusic("Background"); }));
            WorldEvent.RegisterEvent(WorldEvent.WorldEventType.ConfirmComing, () => PlayerSoundEffect("ConfirmJoin"));
            WorldEvent.RegisterEvent(WorldEvent.WorldEventType.RejectComing, () => PlayerSoundEffect("RejectJoin"));
        }

        public void PlayBackgroundMusic(string name)
        {
            AudioClip clip = audioData?.FindClip(name);
            if (clip != null)
            {
                backgroundAudioSource.clip = clip;
                backgroundAudioSource.loop = true;
                backgroundAudioSource.Play();
            }
        }

        public void PlayerSoundEffect(string effectName)
        {
            AudioClip clip = audioData?.FindClip(effectName);
            if (clip != null)
            {
                effectAudioSource.clip = clip;
                effectAudioSource.loop = false;
                effectAudioSource.Play();
            }
        }

        public void StartSpeechVoice(string speechVoiceId)
        {
            speechAudio.StartPlaying(speechVoiceId);
        }

        public void StopSpeechVoice()
        {
            speechAudio.StopPlaying();
        }

        private void Update()
        {
            if (backgroundAudioSource.isPlaying)
            {
                backgroundAudioSource.mute = effectAudioSource.isPlaying;
            }
        }
    }
}
