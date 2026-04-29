using MoreMountains.Feedbacks;
using R3;
using UnityEngine;
using UnityEngine.UI;

public abstract class TabBehavior : MonoBehaviour
{

    [SerializeField] private Button buttonTab;
    [SerializeField] private bool onInitSelect;
    public RectTransform bg;
    public RectTransform icon;
    public RectTransform desBg;
    public RectTransform desIcon;
    private bool currentState = false;

    public MMF_Player animationOnActive;
    public MMF_Player subAnimationOnActive;
    public MMF_Player animationOnInactive;
    public MMF_Player subAnimationOnInactive;
    public MMF_Player comingSoonAnimation;
    public MMF_Player stopComingSoonAnimation;
    public bool isComingSoon = false;

    protected virtual void Start()
    {
        if (onInitSelect)
        {
            currentState = true;
            SceneMenuUI.instance.ChangeTab(gameObject, true);
            bg.anchoredPosition = desBg.anchoredPosition;
            icon.anchoredPosition = desIcon.anchoredPosition;
            icon.localScale = new Vector3(1.3f, 1.3f, 1);
        }

        buttonTab.OnClickAsObservable()
        .Subscribe(_ =>
        {
                // Debug.Log("congthanh1");

            if (isComingSoon)
            {
                Debug.Log("congthanh1123132");
                comingSoonAnimation?.PlayFeedbacks();
                currentState = true;
                return;
            }
            SceneMenuUI.instance.ChangeTab(gameObject);
        }).AddTo(this);

    }
    public virtual void OnTabSelected(bool nextState)
    {
            // Debug.Log(11111);

        if (currentState == nextState && !isComingSoon) return;

        if (isComingSoon && currentState)
        {
            stopComingSoonAnimation?.PlayFeedbacks();
        }

        currentState = nextState;

        if (currentState)
        {
            Debug.Log("Tab " + gameObject.name + " selected");
            animationOnActive?.PlayFeedbacks();
            subAnimationOnActive?.PlayFeedbacks();
        }
        else
        {
            Debug.Log("Tab " + gameObject.name + " deselected");
            animationOnInactive?.PlayFeedbacks();
            subAnimationOnInactive?.PlayFeedbacks();
        }
    }


}
