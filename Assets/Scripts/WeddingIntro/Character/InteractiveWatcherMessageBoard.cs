using WeddingIntro.Ui;

namespace WeddingIntro.Character
{
    public class InteractiveWatcherMessageBoard : InteractiveWatcherBase
    {
        public override void InvokeInteract()
        {
            UIMessageBoardController.Instance.ShowMessageBoard();
        }
    }
}
