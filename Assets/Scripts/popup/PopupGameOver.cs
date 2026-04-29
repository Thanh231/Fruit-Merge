using Cysharp.Threading.Tasks;
using R3;
using TMPro;
using Unity.VectorGraphics;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.Video;

public class PopupGameOver : BasePopup
{
    [SerializeField] TextMeshProUGUI txtGoldContinue;
    [SerializeField] VideoPlayer loseGameVideo;
    [SerializeField] Button btnContinue;
    [SerializeField] Button closeContinue;

    protected override void Start()
    {
        base.Start();
        loseGameVideo.Play();

        // txtGoldContinue.text = $"{HardCodeInGame.COST_GOLD_CONTINUE}";

        btnContinue.OnClickAsObservable()
            .Subscribe(_ =>
            {
                if(CurrencyController.GetGold() < 900)
                {
                    return;
                }
                // GameManager.Instance.ContinueGame();
                // CurrencyController.SubtractGold(HardCodeInGame.COST_GOLD_CONTINUE);
                // HapticController.PlayHaptic(HapticType.valid_button);
                ClosePopup();
            }).AddTo(this);
        closeContinue.OnClickAsObservable()
            .Subscribe(_ =>
            {
                // HapticController.PlayHaptic(HapticType.valid_button);
                if (CurrencyController.GetLives() <= 0)
                {
                    // CurrencyController.SubtractLives(1);
                    // PopupManager.instance.OpenPopup<PopupGiveup>().Forget();
                    SceneManager.LoadScene("3.menu");
                    ClosePopup();
                    return;
                }
                PopupManager.instance.OpenPopup<PopupRetry>().Forget();
                ClosePopup();
            }).AddTo(this);

    }

    public override void OnClosePopup(bool isRunAnim = true)
    {
        base.OnClosePopup(isRunAnim);
    }
}
