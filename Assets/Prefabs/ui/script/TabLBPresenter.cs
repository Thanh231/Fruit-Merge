using TMPro;
using UnityEngine;
using R3;

public class TabLBPresenter : TabBehavior
{   
    public RectTransform text;
    [SerializeField] TextMeshProUGUI currentLevelText;
    [SerializeField] TextMeshProUGUI nextLevelText;
    [SerializeField] TextMeshProUGUI nextNextLevelText;
    private static LevelModel levelModel => PlayerModelManager.instance.GetPlayerModel<LevelModel>();
    public override void OnTabSelected(bool nextState)
    {
        base.OnTabSelected(nextState);
    }

    protected override void Start()
    {
        base.Start();
        text.transform.localScale = Vector3.zero;

        levelModel.lLevel.Subscribe(count =>
        {
            int currentLevel = count;
            currentLevelText.text = "" + currentLevel;
            nextLevelText.text = "" + (currentLevel + 1);
            nextNextLevelText.text = "" + (currentLevel + 2);
            // nextNextLevelText1.text = "" + (currentLevel + 3);
        }).AddTo(this);
    }
}
