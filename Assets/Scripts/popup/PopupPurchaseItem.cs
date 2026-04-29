using R3;
using UnityEngine;
using UnityEngine.UI;

public class PopupPurchaseItem : BasePopup
{
    [SerializeField] Button btn;

    protected override void Start()
    {
        btn.OnClickAsObservable()
            .Subscribe(_ =>
            {
                // CurrencyController.AddGold(100);
                ClosePopup();
            }).AddTo(this);
    }
}
