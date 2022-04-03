using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

public class UIDebugText : MonoBehaviour
{
    [SerializeField]
    private bool enableDebug = false;

    [SerializeField]
    private Text debugText;

    [SerializeField]
    private int lineCount = 20;

    private int startFrom, count;

    private string[] text;

    public void AddDebugText(string inText)
    {
        if(!enableDebug)
        {
            return;
        }

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
