using UnityEngine;

public enum InteractiveType
{
    Dialog,
    MessageBoard,
}

public class InteractiveWatcher : MonoBehaviour
{
    [SerializeField]
    private InteractiveType interactiveType;

    [SerializeField]
    private string dialogEntrance;

    [SerializeField]
    private string dialogVoice;

    private void Start()
    {
        GameStatics.Instance?.PlayerAvatarInteraction?.RegisterInteractiveWatcher(this);
    }

    private void OnDestroy()
    {
        GameStatics.Instance?.PlayerAvatarInteraction?.RegisterInteractiveWatcher(this);
    }

    public void InvokeInteract()
    {
        switch (interactiveType)
        {
            case InteractiveType.Dialog:
                UIDialogBoxController.Instance.StartDialog(dialogEntrance, dialogVoice);
                break;
            case InteractiveType.MessageBoard:
                UIMessageBoardController.Instance.ShowMessageBoard();
                break;
        }
    }
}
