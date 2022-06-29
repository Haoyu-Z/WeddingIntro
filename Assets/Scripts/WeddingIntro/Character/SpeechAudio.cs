using System.Collections.Generic;
using UnityEngine;
using WeddingIntro.Data;

namespace WeddingIntro.Character
{
    [System.Serializable]
    public class SpeechAssetEntry
    {
        public AudioClip Asset;

        public List<float> PossibitiliesOfContinue;
    }

    [System.Serializable]
    public class SpeechEntry
    {
        public string EntryId;

        public List<SpeechAssetEntry> AssetEntries;
    }

    public class SpeechAudio : MonoBehaviour
    {
        [SerializeField]
        private PieceOfSpeechData speechData;

        [SerializeField]
        private AudioSource audioSource;

        [SerializeField]
        private float canContinuePosition;

        [SerializeField]
        private float sentencePauseTime;

        private struct SpeechState
        {
            public bool isPlaying;

            public int continuousCount;

            public bool isContinueTested;

            public int currentSpeechIndex;

            public float currentPauseTime;
        }

        private SpeechEntry speechEntry;

        private SpeechState speechState;

        public void StartPlaying(string speechId)
        {
            SpeechEntry entry = speechData.FindSpeechEntry(speechId);
            if (entry != null)
            {
                speechEntry = entry;
                speechState.isPlaying = true;
            }
        }

        public void StopPlaying()
        {
            speechState = new SpeechState();
        }

        private void Update()
        {
            if (speechState.isPlaying && audioSource != null)
            {
                if (!audioSource.isPlaying)
                {
                    if (speechState.currentPauseTime < sentencePauseTime)
                    {
                        speechState.currentPauseTime += Time.deltaTime;
                    }
                    else
                    {
                        speechState.currentPauseTime = 0.0f;
                        speechState.currentSpeechIndex = Random.Range(0, speechEntry.AssetEntries.Count);
                        audioSource.clip = speechEntry.AssetEntries[speechState.currentSpeechIndex].Asset;
                        audioSource.loop = false;
                        audioSource.time = 0.0f;
                        audioSource.Play();

                        speechState.continuousCount = 0;
                        speechState.isContinueTested = false;
                    }
                }
                else
                {
                    if (audioSource.time / audioSource.clip.length > canContinuePosition && !speechState.isContinueTested)
                    {
                        speechState.isContinueTested = true;

                        List<float> possibilities = speechEntry.AssetEntries[speechState.currentSpeechIndex].PossibitiliesOfContinue;
                        float possibility = speechState.continuousCount < possibilities.Count ? possibilities[speechState.continuousCount] : possibilities[possibilities.Count - 1];
                        float randomValue = Random.value;
                        if (randomValue < possibility)
                        {
                            audioSource.time = 0.0f;
                            speechState.isContinueTested = false;
                            speechState.continuousCount++;
                        }
                    }
                }
            }
        }
    }
}
