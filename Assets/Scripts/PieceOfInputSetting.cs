using UnityEngine;

[CreateAssetMenu(fileName = "PieceOfInputSetting", menuName = "WeddingIntro/ScriptableObject/PieceOfInputSetting")]
public class PieceOfInputSetting : ScriptableObject
{
    public GameKeyDefinition[] InputSettings;
}