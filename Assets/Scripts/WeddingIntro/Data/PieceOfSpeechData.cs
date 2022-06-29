using UnityEngine;
using WeddingIntro.Character;

namespace WeddingIntro.Data
{
    [CreateAssetMenu(fileName = "PieceOfSpeechData", menuName = "WeddingIntro/ScriptableObject/PieceOfSpeechData")]
    public class PieceOfSpeechData : ScriptableObject
    {
        public SpeechEntry[] SpeechEntries;

        public SpeechEntry FindSpeechEntry(string id)
        {
            foreach (SpeechEntry speechEntry in SpeechEntries)
            {
                if (speechEntry.EntryId == id)
                {
                    return speechEntry;
                }
            }

            return null;
        }
    }
}
