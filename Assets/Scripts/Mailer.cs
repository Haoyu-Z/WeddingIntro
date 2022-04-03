using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Net.Mail;

public class Mailer
{
    public enum MailType
    {
        Login,
    }

    public struct MailInfo
    {
        public MailType MailType;
        public PlayerInfo PlayerInfo;

        public MailInfo(MailType type, PlayerInfo playerInfo)
        {
            MailType = type;
            PlayerInfo = playerInfo;
        }
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
                return $"Player {info.PlayerInfo.Name}({info.PlayerInfo.Gender}) has played your game!";
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

        Debug.Log("Trying to send an email...");
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
