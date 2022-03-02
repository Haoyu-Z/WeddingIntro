using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIDialogTextPopper : MonoBehaviour
{
    [TextArea]
    public string TextToShow;

    public float DialogTextPopTime = 0.07f;

    private Text textComponent;

    private float popStartTime = -1.0f;

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
    }

    private void Update()
    {
        if (textComponent == null)
        {
            return;
        }

        if (popStartTime < 0.0f || DialogTextPopTime <= 0.0f || TextToShow.Length == 0)
        {
            return;
        }

        int showCount = Mathf.FloorToInt((Time.time - popStartTime) / DialogTextPopTime);
        if (showCount > TextToShow.Length)
        {
            textComponent.text = TextToShow;
            popStartTime = -1.0f;
            InvokeFinishEvent();
        }
        else
        {
            textComponent.text = TextToShow.Substring(0, showCount);
        }
    }

    public void TriggerText(UIDialogBoxController.UIDialogPanelFinishEvent finishEvent)
    {
        popStartTime = Time.time;
        popFinishEvent = finishEvent;
    }

    public void ResetText()
    {
        popStartTime = -1.0f;
        textComponent.text = "";
    }

    public void ForceFinish(bool callFinishEvent = false)
    {
        popStartTime = -1.0f;
        textComponent.text = TextToShow;

        if (callFinishEvent)
        {
            InvokeFinishEvent();
        }

        popFinishEvent = null;
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
