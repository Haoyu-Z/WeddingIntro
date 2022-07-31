using System.Collections.Generic;

namespace WeddingIntro.Utility
{
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
            Groom_Win5,
            Groom_WinAll,
            Groom_Win5Finished,
        }

        [System.Serializable]
        public struct QuestCondition
        {
            public QuestHolder holder;
            public QuestStates state;
        }

        private static readonly Dictionary<QuestHolder, QuestStates> questState = new Dictionary<QuestHolder, QuestStates>();

        public static bool IsQuestHolderRegistered(QuestHolder holder) => questState.ContainsKey(holder);

        public static bool IsQuestAtState(QuestHolder holder, QuestStates state) => questState.ContainsKey(holder) && questState[holder] == state;

        static QuestManager()
        {
            WorldEvent.RegisterEvent(WorldEvent.WorldEventType.Quest_Bride_FoundItem, () =>
            {
                AudioManager.Instance.PlayerSoundEffect("GetItem");
                if (!IsQuestHolderRegistered(QuestHolder.Bride) || IsQuestAtState(QuestHolder.Bride, QuestStates.NotStarted))
                {
                    questState[QuestHolder.Bride] = QuestStates.Bride_FoundItem;
                }
            });

            WorldEvent.RegisterEvent(WorldEvent.WorldEventType.Quest_Bride_Finish, () =>
            {
                AudioManager.Instance.PlayerSoundEffect("Winner");
                if (IsQuestAtState(QuestHolder.Bride, QuestStates.Bride_FoundItem))
                {
                    questState[QuestHolder.Bride] = QuestStates.CommonFinished;
                }
            });

            WorldEvent.RegisterEvent(WorldEvent.WorldEventType.Quest_Groom_Win5, () =>
            {
                if (!IsQuestHolderRegistered(QuestHolder.Bridegroom) || IsQuestAtState(QuestHolder.Bridegroom, QuestStates.NotStarted))
                {
                    questState[QuestHolder.Bridegroom] = QuestStates.Groom_Win5;
                }
            });

            WorldEvent.RegisterEvent(WorldEvent.WorldEventType.Quest_Groom_WinAll, () =>
            {
                if (IsQuestAtState(QuestHolder.Bridegroom, QuestStates.Groom_Win5) || IsQuestAtState(QuestHolder.Bridegroom, QuestStates.Groom_Win5Finished))
                {
                    questState[QuestHolder.Bridegroom] = QuestStates.Groom_WinAll;
                }
            });

            WorldEvent.RegisterEvent(WorldEvent.WorldEventType.Quest_Groom_Win5Finish, () =>
            {
                if (IsQuestAtState(QuestHolder.Bridegroom, QuestStates.Groom_Win5))
                {
                    questState[QuestHolder.Bridegroom] = QuestStates.Groom_Win5Finished;
                }
            });

            WorldEvent.RegisterEvent(WorldEvent.WorldEventType.Quest_Groom_Finish, () =>
            {
                if (IsQuestAtState(QuestHolder.Bridegroom, QuestStates.Groom_WinAll))
                {
                    questState[QuestHolder.Bridegroom] = QuestStates.CommonFinished;
                }
            });

            WorldEvent.RegisterEvent(WorldEvent.WorldEventType.Misc_Send1M, () =>
            {
                AudioManager.Instance.PlayerSoundEffect("Winner");
                Mailer.Instance.SendMail(Mailer.MailType.Gift1M, new Mailer.IJsonSerializable[] { GameStatics.Instance.PlayerInformation, });
            });
        }

        public static void Init() { }
    }
}
