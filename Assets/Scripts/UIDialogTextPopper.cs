using System;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

public class UIDialogTextPopper : MonoBehaviour
{
    [SerializeField]
    private float DialogTextPopTime = 0.07f;

    [SerializeField]
    private float DialogSelectionFadingPeriod = 1.5f;

    public enum DialogTextType
    {
        PlainText,
        TextWithSelector,
    }

    private DialogTextType textType;

    public DialogTextType TextType { get { return textType; } }

    private string[] selections;

    private string hintText;

    private bool textVisible;

    private int selectedIndex = 0;

    public int SelectedIndex { get { return selectedIndex; } }

    private Text textComponent;

    private static readonly float popStartTimeDisabled = -10000.0f;

    private float popStartTime = popStartTimeDisabled;

    private UIDialogBoxController.UIDialogPanelFinishEvent popFinishEvent;

    private void Start()
    {
        textComponent = GetComponent<Text>();
        if (textComponent != null)
        {
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

        if (!textVisible || DialogTextPopTime <= 0.0f || hintText == null || hintText.Length == 0)
        {
            return;
        }

        if (textType == DialogTextType.PlainText && popStartTime < 0.0f)
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
            if (textType == DialogTextType.PlainText)
            {
                return hintText;
            }
            else
            {
                StringBuilder dialogTextBuilder = new StringBuilder(hintText);
                int alpha = Mathf.FloorToInt(255 * Mathf.Abs(Mathf.Sin(Time.time / DialogSelectionFadingPeriod * Mathf.PI)));

                for (int selectionIdx = 0; selectionIdx < selections.Length; selectionIdx++)
                {
                    string currentLine = null;
                    if (selectionIdx == selectedIndex)
                    {
                        currentLine = string.Format("{0}  - <color=#8C0E0E{1:X02}>{2}</color>", Environment.NewLine, alpha, selections[selectionIdx]);
                    }
                    else
                    {
                        currentLine = string.Format("{0}  - {1}", Environment.NewLine, selections[selectionIdx]);
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

    public void ResetText(DialogEntry dialogEntry)
    {
        popStartTime = popStartTimeDisabled;
        textComponent.text = "";
        textVisible = false;

        if (dialogEntry != null)
        {
            hintText = dialogEntry.HintText;
            selections = dialogEntry.Selections;
            textType = selections.Length > 0 ? DialogTextType.TextWithSelector : DialogTextType.PlainText;
            selectedIndex = 0;
        }
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
            if (selectedIndex >= selections.Length)
            {
                selectedIndex = 0;
            }
        }
        else
        {
            selectedIndex--;
            if (selectedIndex < 0)
            {
                selectedIndex = selections.Length - 1;
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
