using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldEvent
{
    public enum WorldEventType
    {
        None,
        Login,
        ConfirmComing,
        RejectComing,
        Quest_Bride_FoundItem,
        Quest_Bride_Finish,
        Quest_Groom_Win5,
        Quest_Groom_Win5Finish,
        Quest_Groom_WinAll,
        Quest_Groom_Finish,
    }

    static WorldEvent()
    {
        // make sure static class is statically initialized
        Mailer.Init();
        QuestManager.Init();
    }

    private static Dictionary<WorldEventType, List<Action>> worldEventLUT = null;

    private static Dictionary<WorldEventType, List<Action>> WorldEventLUT
    {
        get
        {
            if (worldEventLUT == null)
            {
                worldEventLUT = new Dictionary<WorldEventType, List<Action>>();
            }
            return worldEventLUT;
        }
    }

    public static void RegisterEvent(WorldEventType eventType, Action eventFunc)
    {
        if (!WorldEventLUT.ContainsKey(eventType))
        {
            WorldEventLUT[eventType] = new List<Action>();
        }

        WorldEventLUT[eventType].Add(eventFunc);
    }

    public static void TriggerEvent(WorldEventType eventType)
    {
        UIDebugText.Instance.AddDebugText($"WorldEvent.{eventType} is triggered.", UIDebugText.DebugTextLevel.Warning);

        if (WorldEventLUT.ContainsKey(eventType))
        {
            foreach (Action act in WorldEventLUT[eventType])
            {
                act.Invoke();
            }
        }
    }
}
