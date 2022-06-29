using System.Collections.Generic;
using UnityEngine;

namespace WeddingIntro.Data
{
    [System.Serializable]
    public struct SpriteSheetTablePair
    {
        public string Key;
        public string Prefix;
        public Sprite[] SpriteSequence;
    }

    [CreateAssetMenu(fileName = "AvatarSpriteSheetsTable", menuName = "WeddingIntro/ScriptableObject/AvatarSpriteSheetsTable")]
    public class AvatarSpriteSheetsTable : ScriptableObject
    {
        public List<SpriteSheetTablePair> Table;
    }
}
