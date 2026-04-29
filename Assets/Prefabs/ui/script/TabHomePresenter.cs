using Cysharp.Threading.Tasks;
using R3;
using UnityEngine;
using UnityEngine.UI;

public class TabHomePresenter : TabBehavior
{
    [SerializeField] Button btnPlay;

    protected override void Start()
    {
        base.Start();

        btnPlay.OnClickAsObservable()
            .Subscribe(_ =>
            {
                if (CurrencyController.GetLives() <= 0)
                {
                    // CurrencyController.SubtractLives(1);
                    // PopupManager.instance.OpenPopup<PopupRefill>().Forget();
                    return;
                }
                // CurrencyController.SubtractLives(1);
                LoaderOverlayManager.instance.LoadScene("5.classic");

            }).AddTo(this);
    }
    public override void OnTabSelected(bool nextState)
    {
        base.OnTabSelected(nextState);
    }
}
