namespace Softjourn.SJCoins.Core.UI.ViewInterfaces
{
    public interface IQrView : IBaseView
    {
        void UpdateBalance(string remain);

        void SetEditFieldError(string message);

        void ShowImage(string cashJsonString);
    }
}
