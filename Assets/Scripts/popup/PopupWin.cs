using R3;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PopupWin : BasePopup
{
    [SerializeField] TextMeshProUGUI txtRewardAmount;
    [SerializeField] Button btnContinue;
    [SerializeField] TextMeshProUGUI txtRewardBonusAmount;
    [SerializeField] Button btnWatchAd;

    protected override void Start()
    {
        base.Start();
        // txtRewardAmount.text = $"{HardCodeInGame.REWARD_GOLD_WIN}";
        // txtRewardBonusAmount.text = $"{HardCodeInGame.REWARD_GOLD_WIN * HardCodeInGame.BOUNE_REWARD_GOLD_MULTI}";

        btnContinue.OnClickAsObservable()
            .Subscribe(_ =>
            {
                // CurrencyController.AddGold(HardCodeInGame.REWARD_GOLD_WIN);
                // GameManager.Instance.StartGame();
                // HapticController.PlayHaptic(HapticType.coin_animation);
                // SceneManager.LoadScene("3.menu");
                ClosePopup();

            }).AddTo(this);

        btnWatchAd.OnClickAsObservable()
            .Subscribe(_ =>
            {
                // CurrencyController.AddGold(HardCodeInGame.REWARD_GOLD_WIN * HardCodeInGame.BOUNE_REWARD_GOLD_MULTI);
                // HapticController.PlayHaptic(HapticType.valid_button);
                Debug.Log("Add Logic: Watch ad to double reward");
                // GameManager.Instance.StartGame();
                // SceneManager.LoadScene("3.menu");
                ClosePopup();
            }).AddTo(this);
    }

    protected override void AfterRunAnimClose()
    {

        base.AfterRunAnimClose();
        LoaderOverlayManager.instance.LoadScene("3.menu");
    }
}
