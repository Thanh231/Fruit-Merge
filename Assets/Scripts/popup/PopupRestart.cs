using MoreMountains.Feedbacks;
using R3;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PopupRestart : BasePopup
{
    [SerializeField] Button btnRestart;
    [SerializeField] Button btnClose;
    [SerializeField] TextMeshProUGUI textDescription;

    public MMF_Player noEnoughLivesFeedback;

    protected override void Start()
    {
        base.Start();
        btnClose.OnClickAsObservable()
            .Subscribe(_ =>
            {
                ClosePopup();
            }).AddTo(this);

        textDescription.text = "You will lose 1 heart!";
        btnRestart.OnClickAsObservable()
            .Subscribe(_ =>
            {
                if(CurrencyController.GetLives() <= 0)
                {
                    textDescription.text = "You can not restart the level right now!";
                    noEnoughLivesFeedback?.PlayFeedbacks();
                    return;
                }
                // GameManager.Instance.StartGame();
                // HapticController.PlayHaptic(HapticType.valid_button);
                CurrencyController.SubtractLives(1);
                ClosePopup();
            }).AddTo(this);
    }

    protected override void AfterRunAnimClose()
    {
        // GameManager.Instance.StartGame();

        base.AfterRunAnimClose();
    }
}
