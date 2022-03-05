using UnityEngine;

public class UIDialogPanelScaler : MonoBehaviour
{
    [SerializeField]
    private float ScaleInTime = 0.2f;

    [SerializeField]
    private float ScaleOutTime = 0.2f;

    private float scaleSpeed;

    private float scalePercent = -1.0f;

    private UIDialogBoxController.UIDialogPanelFinishEvent scaleFinishEvent;

    private void Start()
    {
        gameObject.transform.localScale = new Vector3(0.0f, 0.0f, 0.0f);
        scaleSpeed = 0.0f;
        scalePercent = -1.0f;
    }

    private void Update()
    {
        if (0.0f <= scalePercent && scalePercent <= 1.0f)
        {
            scalePercent += scaleSpeed * Time.deltaTime;

            float clampedScale = Mathf.Clamp(scalePercent, 0.0f, 1.0f);
            gameObject.transform.localScale = new Vector3(clampedScale, clampedScale, 1.0f);

            if (scalePercent < 0.0f || scalePercent > 1.0f)
            {
                InvokeFinishEvent();
            }
        }
    }

    public void TriggerScaleIn(UIDialogBoxController.UIDialogPanelFinishEvent finishDelegate = null)
    {
        scaleSpeed = 1 / ScaleInTime;
        if (scalePercent < 0.0f)
        {
            scalePercent = 0.0f;
        }

        if (scalePercent <= 1.0f)
        {
            scaleFinishEvent = finishDelegate;
        }
    }

    public void TriggerScaleOut(UIDialogBoxController.UIDialogPanelFinishEvent finishDelegate = null)
    {
        scaleSpeed = -1 / ScaleOutTime;
        if (scalePercent > 1.0f)
        {
            scalePercent = 1.0f;
        }

        if (scalePercent >= 0.0f)
        {
            scaleFinishEvent = finishDelegate;
        }
    }

    public void ForceFinishScaleIn(bool callFinishEvent = false)
    {
        if (scalePercent <= 1.0f && scaleSpeed > 0.0f)
        {
            scalePercent = 2.0f;

            if (callFinishEvent)
            {
                InvokeFinishEvent();
            }
            scaleFinishEvent = null;


            gameObject.transform.localScale = Vector3.one;
        }
    }

    private void InvokeFinishEvent()
    {
        if (scaleFinishEvent != null)
        {
            // In case the callback has something to do with the trigger.
            UIDialogBoxController.UIDialogPanelFinishEvent eventRef = scaleFinishEvent;
            scaleFinishEvent = null;

            eventRef.Invoke();
        }
    }
}