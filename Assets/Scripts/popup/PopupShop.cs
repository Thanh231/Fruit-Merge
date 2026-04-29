using R3;
using UnityEngine;
using UnityEngine.UI;

public class PopupShop : BasePopup
{
    [SerializeField] Button closebtn;
    protected override void Start()
    {
        closebtn.OnClickAsObservable()
            .Subscribe(_ =>
            {
                ClosePopup();
            }).AddTo(this);
    }
}
