using UnityEngine;
using WeddingIntro.Utility;

namespace WeddingIntro.Data
{
    [CreateAssetMenu(fileName = "QuestCondition", menuName = "WeddingIntro/ScriptableObject/QuestCondition")]
    public class QuestCondition : ScriptableObject, IDialogCondition
    {
        public QuestManager.QuestHolder holder;

        public QuestManager.QuestStates state;

        public bool TestCondition()
        {
            return QuestManager.IsQuestAtState(holder, state);
        }
    }
}
