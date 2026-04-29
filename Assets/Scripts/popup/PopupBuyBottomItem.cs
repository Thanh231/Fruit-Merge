using R3;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Cysharp.Threading.Tasks;
public class PopupBuyBottomItem : BasePopup
{
    [SerializeField] private Sprite addTray;
    [SerializeField] private Sprite hand;
    [SerializeField] private Sprite shuffle;
    [SerializeField] private Sprite superCat;
    [SerializeField] private Image icon;
    [SerializeField] private TextMeshProUGUI txtPrice;
    [SerializeField] private TextMeshProUGUI itemName;
    [SerializeField] private TextMeshProUGUI amountTxt;
    [SerializeField] private TextMeshProUGUI textDescription;
    [SerializeField] private Button close;
    [SerializeField] private Button buybtn;

    // public BoosterIndex boosterIndex;
    protected override void Start()
    {
        base.Start();
        close.OnClickAsObservable()
            .Subscribe(_ =>
            {
                ClosePopup();
            }).AddTo(this);
        // buybtn.OnClickAsObservable()
        //     .Subscribe(_ =>
        //     {
        //         if(CurrencyController.GetGold() >= BoosterPrice.GetPrice(boosterIndex))
        //         {
        //             CurrencyController.SubtractGold(BoosterPrice.GetPrice(boosterIndex));
        //             BoosterController.AddBooster(boosterIndex, BoosterPrice.GetBoosterAmount(boosterIndex));
        //             ClosePopup();
        //         }
        //         else
        //         {
        //             PopupManager.instance.OpenPopup<PopupShop>().Forget();
        //             // ClosePopup();
        //         }
        //     }).AddTo(this);
    }

    // public void SetData(BoosterIndex index)
    // {
    //     boosterIndex = index;
    //     switch (index)
    //     {
    //         case BoosterIndex.tray:
    //             icon.sprite = addTray;
    //             itemName.text = "Add Tray";
    //             amountTxt.text = "x" + BoosterPrice.GetBoosterAmount(BoosterIndex.tray);
    //             textDescription.text = "Add an extra tray to the conveyor!";
    //             break;
    //         case BoosterIndex.hand:
    //             icon.sprite = hand;
    //             itemName.text = "Balloon";
    //             amountTxt.text = "x" + BoosterPrice.GetBoosterAmount(BoosterIndex.hand);
    //              textDescription.text = "Pick any cat or item in the queue.";
    //             break;
    //         case BoosterIndex.shuffle:
    //             icon.sprite = shuffle;
    //             itemName.text = "Shuffle";
    //             amountTxt.text = "x" + BoosterPrice.GetBoosterAmount(BoosterIndex.shuffle);
    //             textDescription.text = "Shuffle the cats in the queue.";
    //             break;
    //         case BoosterIndex.super:
    //             icon.sprite = superCat;
    //             itemName.text = "Super Cat";
    //             amountTxt.text = "x" + BoosterPrice.GetBoosterAmount(BoosterIndex.super);
    //             textDescription.text = "Select a color to shoot with super powers!";
    //             break;
    //     }
    //     txtPrice.text = BoosterPrice.GetPrice(index).ToString();
    // }

}
