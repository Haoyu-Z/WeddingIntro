using System.Text;
using UnityEngine;
using UnityEngine.UI;
using WeddingIntro.Utility;

namespace WeddingIntro.Ui
{
    public class UIDebugText : MonoBehaviour
    {
        public enum DebugTextLevel
        {
            Log,
            Warning,
            Error,
        }

        public enum LogType
        {
            None,
            Screen,
            UnityLog,
        }

        [SerializeField]
        private Text debugText;

        [SerializeField]
        private int lineCount = 20;

        [SerializeField]
        private Color logColor;

        [SerializeField]
        private Color warningColor;

        [SerializeField]
        private Color errorColor;

        private int startFrom, count;

        private string[] text;

        private static UIDebugText instance;

        public static UIDebugText Instance => instance;

        private void Awake()
        {
            Debug.Assert(instance == null);
            instance = this;
        }

        private void WriteUnityLog(string inText, DebugTextLevel level)
        {
            switch (level)
            {
                case DebugTextLevel.Log:
                    Debug.Log(inText);
                    break;
                case DebugTextLevel.Warning:
                    Debug.LogWarning(inText);
                    break;
                case DebugTextLevel.Error:
                    Debug.LogError(inText);
                    break;
            }
        }

        public void AddDebugText(string inText, DebugTextLevel level)
        {
            if (GameStatics.Instance == null || GameStatics.Instance.logType == LogType.UnityLog)
            {
                WriteUnityLog(inText, level);
            }
            else if (GameStatics.Instance.logType == LogType.Screen)
            {
                Color color;
                switch (level)
                {
                    case DebugTextLevel.Warning:
                        color = warningColor;
                        break;
                    case DebugTextLevel.Error:
                        color = errorColor;
                        break;
                    default:
                        color = logColor;
                        break;
                }

                inText = string.Format("<color=#{0:X02}{1:X02}{2:X02}{3:X02}>{4}</color>", Mathf.FloorToInt(255 * color.r), Mathf.FloorToInt(255 * color.g), Mathf.FloorToInt(255 * color.b), Mathf.FloorToInt(255 * color.a), inText);

                if (text == null)
                {
                    text = new string[lineCount];
                    startFrom = 0;
                    count = 0;
                }

                if (count < lineCount)
                {
                    int nextIndex = startFrom + count;
                    if (nextIndex >= text.Length)
                    {
                        nextIndex -= text.Length;
                    }

                    count += 1;
                    text[nextIndex] = inText;
                }
                else
                {
                    text[startFrom] = inText;
                    startFrom += 1;
                    if (startFrom >= text.Length)
                    {
                        startFrom -= text.Length;
                    }
                }

                UpdateText();
            }
        }

        private void UpdateText()
        {
            StringBuilder builder = new StringBuilder();

            for (int i = 0; i < count; i++)
            {
                int lineIndex = i + startFrom;
                if (lineIndex >= text.Length)
                {
                    lineIndex -= text.Length;
                }

                builder.AppendLine(text[lineIndex]);
            }

            debugText.text = builder.ToString();
        }
    }
}
