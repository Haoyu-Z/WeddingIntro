using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class QuestManager
{
    public enum QuestHolder
    {
        Bride,
        Bridegroom,
    }

    public enum QuestStates
    {
        NotStarted,
        CommonFinished,
        Bride_FoundItem,
    }

    [System.Serializable]
    public struct QuestCondition
    {
        public QuestHolder holder;
        public QuestStates state;
    }

    private static readonly Dictionary<QuestHolder, QuestStates> questState = new Dictionary<QuestHolder, QuestStates>();

    public static bool IsQuestAtState(QuestHolder holder, QuestStates state) => questState.ContainsKey(holder) && questState[holder] == state;

    static QuestManager()
    {
        WorldEvent.RegisterEvent(WorldEvent.WorldEventType.Quest_Bride_FoundItem, () =>
        {
            AudioManager.Instance.PlayerSoundEffect("GetItem");
            if (!questState.ContainsKey(QuestHolder.Bride) || questState[QuestHolder.Bride] == QuestStates.NotStarted)
            {
                questState[QuestHolder.Bride] = QuestStates.Bride_FoundItem;
            }
        });

        WorldEvent.RegisterEvent(WorldEvent.WorldEventType.Quest_Bride_Finish, () =>
        {
            AudioManager.Instance.PlayerSoundEffect("Winner");
            if(IsQuestAtState(QuestHolder.Bride, QuestStates.Bride_FoundItem))
            {
                questState[QuestHolder.Bride] = QuestStates.CommonFinished;
            }
        });
    }

    public static void Init() { }
}
