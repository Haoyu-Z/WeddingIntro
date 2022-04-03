using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldEvent : MonoBehaviour
{
    public enum WorldEventType
    {
        None,
        Login,
        ConfirmComing,
        RejectComing,
    }

    private static readonly Dictionary<WorldEventType, Action> worldEventLUT = new Dictionary<WorldEventType, Action>()
    {
        {WorldEventType.Login, new Action(OnLoginEvent)},
        {WorldEventType.ConfirmComing, new Action(OnConfirmComingEvent)},
        {WorldEventType.RejectComing, new Action(OnRejectComingEvent)},
    };

    public static void TriggerEvent(WorldEventType eventType)
    {
        if (worldEventLUT.ContainsKey(eventType))
        {
            worldEventLUT[eventType].Invoke();
        }
    }

    public static void OnLoginEvent()
    {
        if (GameStatics.Instance.SendMailOnLogin)
        {
            Mailer.SendMail(new Mailer.MailInfo(Mailer.MailType.Login, GameStatics.Instance.PlayerInformation));
        }
    }

    public static void OnConfirmComingEvent()
    {
        if (GameStatics.Instance.SendMailOnConfirmComing)
        {
            Mailer.SendMail(new Mailer.MailInfo(Mailer.MailType.ConfirmComing, GameStatics.Instance.PlayerInformation));
        }
    }

    public static void OnRejectComingEvent()
    {
        if (GameStatics.Instance.SendMailOnRejectComing)
        {
            Mailer.SendMail(new Mailer.MailInfo(Mailer.MailType.RejectComing, GameStatics.Instance.PlayerInformation));
        }
    }
}
