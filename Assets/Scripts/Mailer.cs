using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Net.Mail;

public static class Mailer
{
    public enum MailType
    {
        Login,
        ConfirmComing,
        RejectComing,
        MessageBoard,
        FinishBrideQuest,
        HalfFinishGroomQuest,
        FinishGroomQuest,
    }

    public struct MailInfo
    {
        public MailType MailType;
        public object[] Message;

        public MailInfo(MailType type, object[] message)
        {
            MailType = type;
            Message = message;
        }
    }

    static Mailer()
    {
        WorldEvent.RegisterEvent(WorldEvent.WorldEventType.Login,
            () =>
            {
                if (GameStatics.Instance.SendMailOnLogin)
                {
                    SendMail(new MailInfo(MailType.Login, new object[] { GameStatics.Instance.PlayerInformation, }));
                }
            });
        WorldEvent.RegisterEvent(WorldEvent.WorldEventType.ConfirmComing,
            () =>
            {
                if (GameStatics.Instance.SendMailOnConfirmComing)
                {
                    SendMail(new MailInfo(MailType.ConfirmComing, new object[] { GameStatics.Instance.PlayerInformation, }));
                }
            });
        WorldEvent.RegisterEvent(WorldEvent.WorldEventType.RejectComing,
            () =>
            {
                if (GameStatics.Instance.SendMailOnRejectComing)
                {
                    SendMail(new MailInfo(MailType.RejectComing, new object[] { GameStatics.Instance.PlayerInformation, }));
                }
            });
        WorldEvent.RegisterEvent(WorldEvent.WorldEventType.Quest_Bride_Finish,
            () =>
            {
                if (GameStatics.Instance.SendMailOnFinishQuest)
                {
                    SendMail(new MailInfo(MailType.FinishBrideQuest, new object[] { GameStatics.Instance.PlayerInformation, }));
                }
            }
            );
        WorldEvent.RegisterEvent(WorldEvent.WorldEventType.Quest_Groom_Win5Finish,
            () =>
            {
                if(GameStatics.Instance.SendMailOnFinishQuest)
                {
                    SendMail(new MailInfo(MailType.HalfFinishGroomQuest, new object[] { GameStatics.Instance.PlayerInformation }));
                }
            });
        WorldEvent.RegisterEvent(WorldEvent.WorldEventType.Quest_Groom_Finish,
            () =>
            {
                if(GameStatics.Instance.SendMailOnFinishQuest)
                {
                    SendMail(new MailInfo(MailType.FinishGroomQuest, new object[] { GameStatics.Instance.PlayerInformation }));
                }
            });
    }

    private static string GetMailSubject(MailInfo info)
    {
        switch (info.MailType)
        {
            case MailType.Login:
                return $"Player {info.Message[0] as PlayerInfo?} has played your game!";
            case MailType.ConfirmComing:
                return $"Player {info.Message[0] as PlayerInfo?} has confirmed to come!";
            case MailType.RejectComing:
                return $"Player {info.Message[0] as PlayerInfo?} has rejected to come!";
            case MailType.MessageBoard:
                return $"Player {info.Message[0] as PlayerInfo?} send you two a message! Check it out.";
            case MailType.FinishBrideQuest:
                return $"Player {info.Message[0] as PlayerInfo?} accompllishes bride's request !";
            case MailType.HalfFinishGroomQuest:
                return $"Player {info.Message[0] as PlayerInfo?} wins bridegroom's punch machine score !";
            case MailType.FinishGroomQuest:
                return $"Player {info.Message[0] as PlayerInfo?} beats the punch machine !";
            default:
                return null;
        }
    }

    private static string GetMailBody(MailInfo info)
    {
        switch (info.MailType)
        {
            case MailType.Login:
                return $"Congratulations for having a new player!";
            case MailType.ConfirmComing:
                return $"Player + 1! \u2764";
            case MailType.RejectComing:
                return $"Player - 1! Sad...";
            case MailType.MessageBoard:
                return $"Player {(info.Message[0] as PlayerInfo?)} says: {info.Message[1] as string}";
            case MailType.FinishBrideQuest:
            case MailType.HalfFinishGroomQuest:
            case MailType.FinishGroomQuest:
                return $"It happens at FrameCount={Time.frameCount}, Time={Time.time}";
            default:
                return null;
        }
    }

    public static void SendMail(MailInfo info)
    {
        // TODO use other method to post a message to myself
    }

    public static void Init() { }
}
