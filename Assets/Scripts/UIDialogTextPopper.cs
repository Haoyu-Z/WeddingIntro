using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIDialogTextPopper : MonoBehaviour
{
    [TextArea]
    public string TextToShow;

    public float DialogTextPopTime = 0.07f;

    public float DialogSelectionFadingPeriod = 1.5f;

    public enum DialogTextType
    {
        PlainText,
        TextWithYesOrNo,
    }

    public DialogTextType TextType;

    [Tooltip("Only used when type is set to TextWithYesOrNo")]
    public string YesText;

    [Tooltip("Only used when type is set to TextWithYesOrNo")]
    public string NoText;

    private bool textVisible;

    private bool yesNoSelection = true;

    private Text textComponent;

    private static readonly float popStartTimeDisabled = -10000.0f;

    private float popStartTime = popStartTimeDisabled;

    private UIDialogBoxController.UIDialogPanelFinishEvent popFinishEvent;

    private void Start()
    {
        textComponent = GetComponent<Text>();
        if (textComponent != null)
        {
            if (TextToShow == "")
            {
                TextToShow = textComponent.text;
            }
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

        if (!textVisible || DialogTextPopTime <= 0.0f || TextToShow.Length == 0)
        {
            return;
        }

        if (TextType == DialogTextType.PlainText && popStartTime < 0.0f)
        {
            return;
        }

        int showCount = Mathf.FloorToInt((Time.time - popStartTime) / DialogTextPopTime);
        if (showCount > TextToShow.Length)
        {
            textComponent.text = fullText;
            popStartTime = popStartTimeDisabled;
            InvokeFinishEvent();
        }
        else
        {
            textComponent.text = TextToShow.Substring(0, showCount);
        }
    }

    private string fullText { get { return TextToShow + yesNoText; } }

    private string yesNoText
    {
        get
        {
            if (TextType == DialogTextType.PlainText)
            {
                return "";
            }
            else
            {
                int alpha = Mathf.FloorToInt(255 * Mathf.Abs(Mathf.Sin(Time.time / DialogSelectionFadingPeriod * Mathf.PI)));

                if (yesNoSelection)
                {
                    return string.Format("{0}<color=#8C0E0E{3:X02}>{1}</color>{0}{2}", Environment.NewLine, YesText, NoText, alpha);
                }
                else
                {
                    return string.Format("{0}{1}{0}<color=#8C0E0E{3:X02}>{2}</color>", Environment.NewLine, YesText, NoText, alpha);
                }
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
        textComponent.text = fullText;
        textVisible = true; 

        if (callFinishEvent)
        {
            InvokeFinishEvent();
        }

        popFinishEvent = null;
    }

    public void NextYesNoSelection()
    {
        yesNoSelection = !yesNoSelection;
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
