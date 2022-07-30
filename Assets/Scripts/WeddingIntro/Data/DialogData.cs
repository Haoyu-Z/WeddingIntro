using System.Collections.Generic;
using UnityEngine;
using WeddingIntro.Utility;

namespace WeddingIntro.Data
{
    public interface IDialogCondition
    {
        public bool TestCondition();
    }

    [System.Serializable]
    public class ConditionedNextDialog
    {
        public string NextDialogId;

        public ScriptableObject ConditionObject;
    }

    [System.Serializable]
    public class DialogNextSelection
    {
        public string HintString;

        public List<ConditionedNextDialog> ConditionedDialogs;

        public string NextDialogId;
    }

    [System.Serializable]
    public class DialogEntry
    {
        public string DialogId;

        public string StandaloneAudioEffectKey;

        public WorldEvent.WorldEventType TriggerWorldEvent;

        [Tooltip("Will jump to this dialog if there's no selection candidate.")]
        public string NextDialogId;

        [TextArea]
        public string HintText;

        public DialogNextSelection[] NextSelections;
    }

    public class DialogData : MonoBehaviour
    {
        public static DialogData Instance = null;

        public PieceOfDialog[] AllDialogs;

        private readonly Dictionary<string, DialogEntry> DialogLUT = new Dictionary<string, DialogEntry>();

        public DialogEntry LookupDialog(string dialogId)
        {
            if (DialogLUT.ContainsKey(dialogId))
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

            foreach (PieceOfDialog piece in AllDialogs)
            {
                if (piece == null)
                {
                    continue;
                }

                foreach (DialogEntry entry in piece.DialogEntries)
                {
                    DialogLUT.Add(entry.DialogId, entry);
                }
            }
        }
    }
}
