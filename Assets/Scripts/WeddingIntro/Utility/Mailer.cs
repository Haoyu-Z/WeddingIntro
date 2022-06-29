using System.Collections;
using UnityEngine;
using System.Text;
using UnityEngine.Networking;
using WeddingIntro.Ui;

namespace WeddingIntro.Utility
{
    public class Mailer : MonoBehaviour
    {
        public interface IJsonSerializable
        {
            public string ToJsonObject();
        }

        public struct SingleValueSerializableField<T> : IJsonSerializable
        {
            public string Key;

            public T Value;

            public SingleValueSerializableField(string key, T value)
            {
                Key = key;
                Value = value;
            }

            public string ToJsonObject()
            {
                return $"\"{Key.Replace('\"', '\'')}\" : \"{Value.ToString().Replace('\"', '\'')}\"";
            }
        }

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

        private static Mailer instance = null;

        public static Mailer Instance => instance;

        private void Awake()
        {
            Debug.Assert(instance == null);
            instance = this;

            WorldEvent.RegisterEvent(WorldEvent.WorldEventType.Login,
                () => { SendPlayerInfoMail(GameStatics.Instance.SendMailOnLogin, MailType.Login); });
            WorldEvent.RegisterEvent(WorldEvent.WorldEventType.ConfirmComing,
                () => { SendPlayerInfoMail(GameStatics.Instance.SendMailOnConfirmComing, MailType.ConfirmComing); });
            WorldEvent.RegisterEvent(WorldEvent.WorldEventType.RejectComing,
                () => { SendPlayerInfoMail(GameStatics.Instance.SendMailOnRejectComing, MailType.RejectComing); });
            WorldEvent.RegisterEvent(WorldEvent.WorldEventType.Quest_Bride_Finish,
                () => { SendPlayerInfoAndGameTimeMail(GameStatics.Instance.SendMailOnFinishQuest, MailType.FinishBrideQuest); });
            WorldEvent.RegisterEvent(WorldEvent.WorldEventType.Quest_Groom_Win5Finish,
                () => { SendPlayerInfoAndGameTimeMail(GameStatics.Instance.SendMailOnFinishQuest, MailType.HalfFinishGroomQuest); });
            WorldEvent.RegisterEvent(WorldEvent.WorldEventType.Quest_Groom_Finish,
                () => { SendPlayerInfoAndGameTimeMail(GameStatics.Instance.SendMailOnFinishQuest, MailType.FinishGroomQuest); });
        }

        public void SendPlayerInfoMail(bool Condition, MailType mailType)
        {
            if (Condition)
            {
                SendMail(mailType, new IJsonSerializable[] { GameStatics.Instance.PlayerInformation, });
            }
        }

        public void SendPlayerInfoAndGameTimeMail(bool Condition, MailType mailType)
        {
            if (Condition)
            {
                SendMail(mailType, new IJsonSerializable[] { GameStatics.Instance.PlayerInformation, new SingleValueSerializableField<float>("Time", Time.time) });
            }
        }

        public static string FormatJsonObject(IJsonSerializable[] objects, params IJsonSerializable[] otherObjects)
        {
            IJsonSerializable[] total = new IJsonSerializable[objects.Length + otherObjects.Length];
            for (int i = 0; i < objects.Length; i++)
            {
                total[i] = objects[i];
            }
            for (int i = 0; i < otherObjects.Length; i++)
            {
                total[i + objects.Length] = otherObjects[i];
            }

            if (total.Length <= 0)
            {
                return "{ }";
            }

            StringBuilder builder = new StringBuilder();
            builder.Append("{ ");

            for (int i = 0; i < total.Length - 1; i++)
            {
                builder.Append(total[i].ToJsonObject());
                builder.Append(", ");
            }
            builder.Append(total[total.Length - 1].ToJsonObject());

            builder.Append(" }");

            return builder.ToString();
        }

        public void SendMail(MailType mailType, IJsonSerializable[] objects)
        {
            UIDebugText.Instance.AddDebugText($"Sending web request message of type {mailType}", UIDebugText.DebugTextLevel.Log);
            string JsonMessage = FormatJsonObject(objects, new SingleValueSerializableField<string>("Type", $"{mailType}"));

            byte[] messageBytes = Encoding.UTF8.GetBytes(JsonMessage);
            UnityWebRequest request = UnityWebRequest.Put("http://127.0.0.1:8080", messageBytes);
            StartCoroutine(SendWebRequest(request));
        }

        private IEnumerator SendWebRequest(UnityWebRequest request)
        {
            yield return request.SendWebRequest();

            if (request.result != UnityWebRequest.Result.Success)
            {
                UIDebugText.Instance.AddDebugText($"Send web request error {request.error}", UIDebugText.DebugTextLevel.Error);
            }
            else
            {
                UIDebugText.Instance.AddDebugText($"Send web request success.", UIDebugText.DebugTextLevel.Log);
            }
        }
    }
}
