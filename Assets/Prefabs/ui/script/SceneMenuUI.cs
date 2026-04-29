using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using R3;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SceneMenuUI : SingletonMonoBehaviour<SceneMenuUI>
{
    public List<GameObject> tabScreens = new List<GameObject>();
    [SerializeField] private Button iconButton;
    [SerializeField] private Button plusLiveButton;
    [SerializeField] private Button plusGoldButton;
    [SerializeField] private Button settingButton;
    [SerializeField] private TextMeshProUGUI goldText;
    [SerializeField] private TextMeshProUGUI livesText;
    [SerializeField] private TextMeshProUGUI livesTimerText;
    private float _tickAccumulator;
    private const int MAX_LIVES = 5;
    private const int REGEN_TIME_SECONDS = 30 * 60;

    private static CurrencyModel currencyModel => PlayerModelManager.instance.GetPlayerModel<CurrencyModel>();
    private void Start()
    {
        // AudioGameManger.instance.InitAudioGameManager();
        // AudioController.instance.PlayMusic(AudioIndex.bgm.ToString());
        LoaderOverlayManager.instance.EndOverlay();

        currencyModel.gold.ObserveOnMainThread().Subscribe(gold =>
        {
            goldText.text = gold.ToString();
        }).AddTo(this);

        currencyModel.lives.ObserveOnMainThread().Subscribe(lives =>
        {
            livesText.text = lives.ToString();
            if (lives >= MAX_LIVES)
            {
                livesTimerText.text = "MAX";
                currencyModel.nextLifeTime.Value = 0; // Reset timer khi đầy tim
            }
        }).AddTo(this);

        iconButton.OnClickAsObservable()
            .Subscribe(_ =>
            {
            }).AddTo(this);
        plusLiveButton.OnClickAsObservable()
            .Subscribe(_ =>
            {
                if (CurrencyController.GetLives() >= MAX_LIVES)
                {
                    return;
                }
                // PopupManager.instance.OpenPopup<PopupRefill>().Forget();
            }).AddTo(this);
        plusGoldButton.OnClickAsObservable()
            .Subscribe(_ =>
            {
                ChangeTab(tabScreens[0]);
            }).AddTo(this);
        settingButton.OnClickAsObservable()
            .Subscribe(_ =>
            {
                // PopupManager.instance.OpenPopup<PopupSettings>().Forget();
            }).AddTo(this);
    }

    public void ChangeTab(GameObject tab, bool noNeedPlayAnimation = false)
    {
        var index = tabScreens.IndexOf(tab);
        SetActiveTabScreen(index, noNeedPlayAnimation);
    }
    private void SetActiveTabScreen(int index, bool noNeedPlayAnimation = false)
    {
        for (int i = 0; i < tabScreens.Count; i++)
        {
            var tabComponent = tabScreens[i].GetComponent<TabBehavior>();
            if (i == index)
            {
                tabScreens[i].SetActive(true);
            }
            else
            {
                tabScreens[i].SetActive(false);
            }
            if (!noNeedPlayAnimation)
            {
                tabComponent.OnTabSelected(i == index);
            }
        }
    }

    private void Update()
    {
        if (currencyModel == null) return;

        long currentLives = CurrencyController.GetLives();
        if (currentLives >= 5) return;

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
            livesTimerText.text = $"{minutes:00}:{seconds:00}";
        }
    }
}