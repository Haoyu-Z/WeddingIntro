using UnityEngine;
using WeddingIntro.Avatar;

namespace WeddingIntro.Data
{
    [CreateAssetMenu(fileName = "PieceOfInputSetting", menuName = "WeddingIntro/ScriptableObject/PieceOfInputSetting")]
    public class PieceOfInputSetting : ScriptableObject
    {
        public GameKeyDefinition[] InputSettings;
    }
}
