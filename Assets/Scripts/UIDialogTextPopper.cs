using System;
using System.Text;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIDialogTextPopper : MonoBehaviour
{
    public float DialogTextPopTime = 0.07f;

    public float DialogSelectionFadingPeriod = 1.5f;

    public enum DialogTextType
    {
        PlainText,
        TextWithSelector,
    }

    public DialogTextType TextType;

    [Tooltip("Only used when type is set to TextWithSelector")]
    public string[] Selections;

    private string hintText;

    private bool textVisible;

    private int selectedIndex = 0;

    private Text textComponent;

    private static readonly float popStartTimeDisabled = -10000.0f;

    private float popStartTime = popStartTimeDisabled;

    private UIDialogBoxController.UIDialogPanelFinishEvent popFinishEvent;

    private void Start()
    {
        textComponent = GetComponent<Text>();
        if (textComponent != null)
        {
            hintText = textComponent.text;
            textComponent.text = "";
        }

        textVisible = false;
    }

    private void Update()
    {
        if (textComponent == null)
        {
            return;
        }

        if (!textVisible || DialogTextPopTime <= 0.0f || hintText.Length == 0)
        {
            return;
        }

        if (TextType == DialogTextType.PlainText && popStartTime < 0.0f)
        {
            return;
        }

        int showCount = Mathf.FloorToInt((Time.time - popStartTime) / DialogTextPopTime);
        if (showCount > hintText.Length)
        {
            textComponent.text = fullDialogText;
            popStartTime = popStartTimeDisabled;
            InvokeFinishEvent();
        }
        else
        {
            textComponent.text = hintText.Substring(0, showCount);
        }
    }

    private string fullDialogText
    {
        get
        {
            if (TextType == DialogTextType.PlainText)
            {
                return hintText;
            }
            else
            {
                StringBuilder dialogTextBuilder = new StringBuilder(hintText);
                int alpha = Mathf.FloorToInt(255 * Mathf.Abs(Mathf.Sin(Time.time / DialogSelectionFadingPeriod * Mathf.PI)));

                for (int selectionIdx = 0; selectionIdx < Selections.Length; selectionIdx++)
                {
                    string currentLine = null;
                    if (selectionIdx == selectedIndex)
                    {
                        currentLine = string.Format("{0}<color=#8C0E0E{1:X02}>{2}</color>", Environment.NewLine, alpha, Selections[selectionIdx]);
                    }
                    else
                    {
                        currentLine = string.Format("{0}{1}", Environment.NewLine, Selections[selectionIdx]);
                    }

                    dialogTextBuilder.Append(currentLine);
                }

                return dialogTextBuilder.ToString();
            }
        }
    }

    public void TriggerText(UIDialogBoxController.UIDialogPanelFinishEvent finishEvent)
    {
        popStartTime = Time.time;
        popFinishEvent = finishEvent;
        textVisible = true;
    }

    public void ResetText()
    {
        popStartTime = popStartTimeDisabled;
        textComponent.text = "";
        textVisible = false;
    }

    public void ForceFinish(bool callFinishEvent = false)
    {
        popStartTime = popStartTimeDisabled;
        textComponent.text = fullDialogText;
        textVisible = true;

        if (callFinishEvent)
        {
            InvokeFinishEvent();
        }

        popFinishEvent = null;
    }

    public void NextSelection(bool inverse = false)
    {
        if (!inverse)
        {
            selectedIndex++;
            if (selectedIndex >= Selections.Length)
            {
                selectedIndex = 0;
            }
        }
        else
        {
            selectedIndex--;
            if (selectedIndex < 0)
            {
                selectedIndex = Selections.Length - 1;
            }
        }
    }

    private void InvokeFinishEvent()
    {
        if (popFinishEvent != null)
        {
            // In case the callback has something to do with the trigger.
            UIDialogBoxController.UIDialogPanelFinishEvent eventRef = popFinishEvent;
            popFinishEvent = null;

            eventRef.Invoke();
        }
    }
}
