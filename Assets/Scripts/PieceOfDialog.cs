using UnityEngine;

[CreateAssetMenu(fileName = "Data", menuName = "ScriptableObject/PieceOfDialog")]
public class PieceOfDialog : ScriptableObject
{
    public DialogEntry[] DialogEntries;
}