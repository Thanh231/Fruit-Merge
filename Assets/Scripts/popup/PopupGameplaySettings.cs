using Cysharp.Threading.Tasks;
using R3;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PopupGameplaySettings : BasePopup
{
    [SerializeField] SliderSettingPresenter sliderBGM;
    [SerializeField] SliderSettingPresenter sliderSFX;
    [SerializeField] OnOffSettingPresenter onOffHaptic;

    [SerializeField] Button btnPolicy;
    [SerializeField] Button btnSupport;
    [SerializeField] Button btnRestartLevel;
    [SerializeField] Button btnHome;
    [SerializeField] Button btnClose;



    private bool noloadMenu = false;

    protected override void Start()
    {
        base.Start();

        // sliderBGM.Init(SettingController.bgmVolumeRx.Value, SettingController.UpdateBGMVolume);
        // sliderSFX.Init(SettingController.sfxVolumeRx.Value, SettingController.UpdateSFXVolume);
        // onOffHaptic.Init(SettingController.hapticOnRx, SettingController.UpdateHapticOn);

        // if(HandlePigBehavior.instance != null)
        // {
        //     HandlePigBehavior.instance.PausePigsOnConveyor();
        // }

        if (LevelController.GetMaxLevelUnlock() == 1)
        {
            btnRestartLevel.gameObject.SetActive(false);
            btnHome.gameObject.SetActive(false);
        }
        else
        {
            btnRestartLevel.gameObject.SetActive(true);
            btnHome.gameObject.SetActive(true);
        }

        btnRestartLevel.OnClickAsObservable()
            .Subscribe(_ =>
            {

                // HapticController.PlayHaptic(HapticType.valid_button);
                // ClosePopup();
                // noloadMenu = true;
                // if (GameManager.Instance.playerAction)
                // {
                //     // CurrencyController.SubtractLives(1);
                //     PopupManager.instance.OpenPopup<PopupRestart>().Forget();
                // }
                // else
                // {
                //     GameManager.Instance.StartGame();
                // }
                // PopupManager.instance.OpenPopup<PopupRestart>().Forget();
            }).AddTo(this);

        btnClose.OnClickAsObservable()
            .Subscribe(_ =>
            {
                // HapticController.PlayHaptic(HapticType.valid_button);
                // HandlePigBehavior.instance?.ResumePigsOnConveyor();
                noloadMenu = true;
                ClosePopup();
            }).AddTo(this);
    
        btnHome.OnClickAsObservable()
            .Subscribe(_ =>
            {
                // HapticController.PlayHaptic(HapticType.valid_button);
                ClosePopup();

                // if (GameManager.Instance.playerAction)
                // {
                //     PopupManager.instance.OpenPopup<PopupGiveup>().Forget();
                // }
            }).AddTo(this);
    }

    protected override void AfterRunAnimClose()
    {

        base.AfterRunAnimClose();
        // if (!noloadMenu && !GameManager.Instance.playerAction)
        // {
        //     LoaderOverlayManager.instance.LoadScene("3.menu");
        // }

    }
}