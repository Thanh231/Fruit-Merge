using R3;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PopupGiveup : BasePopup
{
    [SerializeField] Button btnGiveUp;
    [SerializeField] Button btnClose;
    [SerializeField] TextMeshProUGUI textDescription;

    protected override void Start()
    {
        base.Start();

        if (CurrencyController.GetLives() <= 0)
        {
            textDescription.text = "You have no more hearts left!";
        }
        else
        {
            textDescription.text = "You will lose 1 heart!";
        }

        btnClose.OnClickAsObservable()
            .Subscribe(_ =>
            {
                ClosePopup();
            }).AddTo(this);

        btnGiveUp.OnClickAsObservable()
            .Subscribe(_ =>
            {
                ClosePopup();
                // SceneManager.LoadScene("3.menu");
                // HapticController.PlayHaptic(HapticType.valid_button);
                // if(GameManager.Instance.playerAction)
                // {
                //     CurrencyController.SubtractLives(1);
                // }
            }).AddTo(this);
    }

    protected override void AfterRunAnimClose()
    {
        // GameManager.Instance.StartGame();

        base.AfterRunAnimClose();
        LoaderOverlayManager.instance.LoadScene("3.menu");

    }
}
