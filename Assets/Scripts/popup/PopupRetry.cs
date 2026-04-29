using R3;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PopupRetry : BasePopup
{
    [SerializeField] TextMeshProUGUI txtLevel;
    [SerializeField] Button btnRetry;
    [SerializeField] Button btnClose;

    protected override void Start()
    {
        base.Start();

        btnClose.OnClickAsObservable()
            .Subscribe(_ =>
            {
                ClosePopup();
                // HapticController.PlayHaptic(HapticType.light_impact);
                SceneManager.LoadScene("3.menu");
            }).AddTo(this);

        txtLevel.text = $"Level {LevelController.GetMaxLevelUnlock()}";

        btnRetry.OnClickAsObservable()
            .Subscribe(_ =>
            {
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
