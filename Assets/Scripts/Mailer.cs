using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Net.Mail;

public class Mailer
{
    public enum MailType
    {
        Login,
        ConfirmComing,
        RejectComing,
        MessageBoard,
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
    }

    private static SmtpClient client = null;

    private static SmtpClient Client
    {
        get
        {
            if (client == null)
            {
                client = new SmtpClient("smtp.qq.com");
                client.Credentials = new System.Net.NetworkCredential("====deleted====", "====deleted====");
                client.EnableSsl = true;
                client.SendCompleted += SendMailCallback;
            }

            return client;
        }
    }

    private static string GetMailSubject(MailInfo info)
    {
        switch (info.MailType)
        {
            case MailType.Login:
                return $"Player {(info.Message[0] as PlayerInfo?)?.Name}({(info.Message[0] as PlayerInfo?)?.Gender}) has played your game!";
            case MailType.ConfirmComing:
                return $"Player {(info.Message[0] as PlayerInfo?)?.Name}({(info.Message[0] as PlayerInfo?)?.Gender}) has confirmed to come!";
            case MailType.RejectComing:
                return $"Player {(info.Message[0] as PlayerInfo?)?.Name}({(info.Message[0] as PlayerInfo?)?.Gender}) has rejected to come!";
            case MailType.MessageBoard:
                return $"Player {(info.Message[0] as PlayerInfo?)?.Name}({(info.Message[0] as PlayerInfo?)?.Gender}) send you two a message! Check it out.";
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
                return $"Player {(info.Message[0] as PlayerInfo?)?.Name} says: {info.Message[1] as string}";
            default:
                return null;
        }
    }

    public static void SendMail(MailInfo info)
    {
        MailMessage mail = new MailMessage();
        mail.From = new MailAddress("154480690@qq.com");
        mail.To.Add(new MailAddress("154480690@qq.com"));

        string subject = GetMailSubject(info);
        string body = GetMailBody(info);
        if (subject == null || body == null)
        {
            return;
        }

        mail.Subject = subject;
        mail.SubjectEncoding = System.Text.Encoding.UTF8;
        mail.Body = body;
        mail.BodyEncoding = System.Text.Encoding.UTF8;
        mail.Priority = MailPriority.Normal;

        SmtpClient client = Client;

        Debug.Log($"Trying to send an email : {mail.Body}");
        client.SendAsync(mail, null);
    }

    public static void SendMailCallback(object sender, System.ComponentModel.AsyncCompletedEventArgs e)
    {
        if (e.Error == null)
        {
            Debug.LogWarning("A mail has been sent.");
        }
        else
        {
            Debug.LogError("A mail has met an error: " + e.Error);
        }
    }
}
