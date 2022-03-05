using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class DialogEntry
{
    public string DialogId;

    [Tooltip("Will jump to this dialog if there's no selection candidate.")]
    public string NextDialogId;

    [TextArea]
    public string HintText;

    public string[] Selections;

    public string[] SelectionsNextDialogId;
}

public class DialogData : MonoBehaviour
{
    public static DialogData Instance = null;

    public DialogEntry[] DialogCollection;

    private readonly Dictionary<string, DialogEntry> DialogLUT = new Dictionary<string, DialogEntry>();

    public DialogEntry LookupDialog(string dialogId)
    {
        if(DialogLUT.ContainsKey(dialogId))
        {
            return DialogLUT[dialogId];
        }
        else
        {
            return null;
        }
    }

    void Awake()
    {
        Debug.Assert(Instance == null);
        Instance = this;

        foreach(DialogEntry entry in DialogCollection)
        {
            DialogLUT.Add(entry.DialogId, entry);
        }
    }
}
