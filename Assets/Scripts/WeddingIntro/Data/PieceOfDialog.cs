using UnityEngine;

namespace WeddingIntro.Data
{
    [CreateAssetMenu(fileName = "PieceOfDialog", menuName = "WeddingIntro/ScriptableObject/PieceOfDialog")]
    public class PieceOfDialog : ScriptableObject
    {
        public DialogEntry[] DialogEntries;
    }
}
