
using R3;
using TMPro;
using UnityEngine.UI;

public class PopupCheat : BasePopup
{
    public TMP_InputField inputLevel;
    public Button btnSetLevel;

    public TMP_InputField inputGold;
    public Button btnSetGold;

    public TMP_InputField inputLives;
    public Button btnSetLives;

    private static CurrencyModel currencyModel => PlayerModelManager.instance.GetPlayerModel<CurrencyModel>();
    private static LevelModel levelModel => PlayerModelManager.instance.GetPlayerModel<LevelModel>();

    override protected void Start()
    {
        base.Start();

        inputGold.text = currencyModel.gold.Value.ToString();
        // inputLevel.text = levelModel.lLevel.Count.ToString();
        inputLives.text = currencyModel.lives.Value.ToString();

        // btnSetLevel.OnClickAsObservable().Subscribe(_ =>
        // {
        //     var level = StaticUtils.StringToLong(inputLevel.text);

        //     levelModel.lLevel.Clear();
        //     for (int i = 0; i < level; i++)
        //     {
        //         levelModel.lLevel.Add(new LevelModelItem { level = i + 1 });
        //     }
        //     levelModel.Save();
        // });

        btnSetGold.OnClickAsObservable().Subscribe(_ =>
        {
            var gold = StaticUtils.StringToLong(inputGold.text);
            currencyModel.gold.Value = gold;
            currencyModel.Save();
        });

        btnSetLives.OnClickAsObservable().Subscribe(_ =>
        {
            var lives = StaticUtils.StringToLong(inputLives.text);
            currencyModel.lives.Value = lives;
            currencyModel.Save();
        });
    }
}