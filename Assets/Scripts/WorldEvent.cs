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
        if (WorldEventLUT.ContainsKey(eventType))
        {
            foreach (Action act in WorldEventLUT[eventType])
            {
                act.Invoke();
            }
        }
    }
}
