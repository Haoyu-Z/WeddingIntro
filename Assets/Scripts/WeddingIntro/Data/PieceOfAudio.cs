using UnityEngine;
using WeddingIntro.Utility;

namespace WeddingIntro.Data
{
    [CreateAssetMenu(fileName = "PieceOfAudio", menuName = "WeddingIntro/ScriptableObject/PieceOfAudio")]
    public class PieceOfAudio : ScriptableObject
    {
        public AudioEntry[] AudioEntries;

        public AudioClip FindClip(string clipName)
        {
            foreach (AudioEntry entry in AudioEntries)
            {
                if (entry.EntryName == clipName)
                {
                    Debug.Assert(entry.ClipAsset != null);
                    return entry.ClipAsset;
                }
            }
            return null;
        }
    }
}
