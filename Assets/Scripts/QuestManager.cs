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

    private static readonly Dictionary<QuestHolder, QuestStates> questState = new Dictionary<QuestHolder, QuestStates>();

    public static bool IsQuestFinished(QuestHolder holder) => questState.ContainsKey(holder) && questState[holder] == QuestStates.CommonFinished;

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
    }

    public static void Init() { }
}
