using System;
using R3;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PopupRefill : BasePopup
{
    [SerializeField] Button btnRefill;
    [SerializeField] Button btnWatchAd;
    [SerializeField] Button btnClose;
    [SerializeField] TextMeshProUGUI timerText;
    [SerializeField] TextMeshProUGUI livesText;
    [SerializeField] TextMeshProUGUI livePlus;


    protected override void Start()
    {
        base.Start();

        livesText.text = $"{CurrencyController.GetLives()}";
        livePlus.text = CurrencyController.GetLives() < 5 ? "+" : "";
        btnClose.OnClickAsObservable()
            .Subscribe(_ =>
            {
                ClosePopup();
                // HapticController.PlayHaptic(HapticType.light_impact);
            }).AddTo(this);

        btnRefill.OnClickAsObservable()
            .Subscribe(_ =>
            {
                if (CurrencyController.GetGold() < 900)
                {
                    return;
                }
                CurrencyController.SubtractGold(900);
                CurrencyController.AddLives(CurrencyController.GetLives() < 5 ? 5 - CurrencyController.GetLives() : 0);
                // HapticController.PlayHaptic(HapticType.valid_button);
                ClosePopup();
            }).AddTo(this);

        btnWatchAd.OnClickAsObservable()
            .Subscribe(_ =>
            {
                CurrencyController.AddLives(CurrencyController.GetLives() < 5 ? 1 : 0);
                ClosePopup();
            }).AddTo(this);
    }
    private void Update()
    {
        long currentLives = CurrencyController.GetLives();
        if (currentLives >= 5)
        {
            timerText.text = "MAX";

            btnRefill.interactable = false;
            btnWatchAd.interactable = false;
            return;
        }
        btnRefill.interactable = true;
        btnWatchAd.interactable = true;
        long currentTime = DateTimeOffset.UtcNow.ToUnixTimeSeconds();
        long targetTime = CurrencyController.GetNextLifeTime();

        if (targetTime == 0)
        {
            CurrencyController.SetNextLifeTime(currentTime + (30 * 60));
            return;
        }

        long timeLeft = targetTime - currentTime;
        if (timeLeft <= 0)
        {
            long timePassed = -timeLeft;
            long livesToAdd = 1 + (timePassed / 1800);

            CurrencyController.AddLives(livesToAdd);

            if (CurrencyController.GetLives() < 5)
            {
                long remainder = timePassed % 1800;
                CurrencyController.SetNextLifeTime(currentTime + 1800 - remainder);
            }
        }
        else
        {
            long minutes = timeLeft / 60;
            long seconds = timeLeft % 60;
            timerText.text = $"{minutes:00}:{seconds:00}";
        }
    }
}
