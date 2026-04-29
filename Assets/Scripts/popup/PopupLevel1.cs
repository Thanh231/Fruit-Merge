using TMPro;
using UnityEngine;
using R3;

public class PopupLevel1 : BasePopup
{
    public TextMeshProUGUI des;
    private RectTransform _rect;
    private RectTransform _rectHand;
    public GameObject skeletonAnimation;

    private Canvas _canvas;
    private Camera _cam;

    private static readonly Vector2 ReferenceResolution = new Vector2(1080f, 1920f);


    protected override void Start()
    {
        base.Start();

        _canvas = GetComponentInParent<Canvas>();
        _cam = _canvas.renderMode == RenderMode.ScreenSpaceOverlay
            ? null
            : _canvas.worldCamera;

        _rectHand = skeletonAnimation.GetComponent<RectTransform>();
        _rect = GetComponent<RectTransform>();

        if (des == null)
        {
            Debug.LogError($"[PopupLevel1] Biến 'des' chưa được kéo vào Inspector trên {gameObject.name}!");
            return;
        }

        skeletonAnimation.SetActive(true);

        // var key = GuideTutorialType.Level_1.ToString();
        // var item = TutorialController.GetTutorialItem(key);

        // if (item != null && !item.isCompleted.Value)
        // {
        //     item.currentStep
        //         .Subscribe(step =>
        //         {
        //             if (des == null || _rect == null) return;

        //             Debug.Log($"Step changed: {step} for {key}");

        //             switch (step)
        //             {
        //                 case 0:
        //                     des.text = "Pick cat and start collecting yarn!";
        //                     _rect.anchoredPosition = ScalePos(new Vector2(0f, 250f));
        //                     SetTargetToWorldObject(new Vector3(-0.2f, -22f, 0f), new Vector2(1f, 1f));
        //                     skeletonAnimation.SetActive(true);
        //                     break;
        //                 case 1:
        //                     des.text = "Wait for the cat to travel!";
        //                     _rect.anchoredPosition = ScalePos(new Vector2(0f, -500f));
        //                     skeletonAnimation.SetActive(false);
        //                     break;
        //                 case 2:
        //                     des.text = "The cat isn't full yet. Send it out again!";
        //                     _rect.anchoredPosition = ScalePos(new Vector2(0f, 250f));
        //                     SetTargetToWorldObject(new Vector3(-1.6f, -19.5f, 0f), new Vector2(1f, 1f));
        //                     skeletonAnimation.SetActive(true);
        //                     break;
        //                 default:
        //                     break;
        //             }
        //         })
        //         .AddTo(this);
        // }
    }


    private void SetTargetToWorldObject(Vector3 worldPos, Vector2 worldScale)
    {
        if (_canvas == null) return;

        Camera cam3D = Camera.main;
        if (cam3D == null)
        {
            Debug.LogWarning("[TutorialViewManager] Không tìm thấy camera 3D để chiếu object.");
            return;
        }

        RectTransform canvasRect = _canvas.GetComponent<RectTransform>();

        Vector3 halfExtent = new Vector3(worldScale.x * 0.5f, worldScale.y * 0.5f, 0f);
        Vector3 worldMin = worldPos - halfExtent;
        Vector3 worldMax = worldPos + halfExtent;

        Vector2 screenCenter = cam3D.WorldToScreenPoint(worldPos);
        Vector2 screenMin = cam3D.WorldToScreenPoint(worldMin);
        Vector2 screenMax = cam3D.WorldToScreenPoint(worldMax);

        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            canvasRect, screenCenter, _cam, out Vector2 localCenter);
        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            canvasRect, screenMin, _cam, out Vector2 localMin);
        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            canvasRect, screenMax, _cam, out Vector2 localMax);

        Vector2 localSize = new Vector2(
            Mathf.Abs(localMax.x - localMin.x),
            Mathf.Abs(localMax.y - localMin.y)
        );

        _rectHand.anchorMin = new Vector2(0.5f, 0.5f);
        _rectHand.anchorMax = new Vector2(0.5f, 0.5f);
        _rectHand.pivot = new Vector2(0.5f, 0.5f);
        _rectHand.sizeDelta = localSize;
        _rectHand.anchoredPosition = localCenter;
        _rectHand.localScale = Vector3.one;
    }
    private Vector2 ScalePos(Vector2 refPos)
    {
        if (_canvas == null) return refPos;
        RectTransform canvasRect = _canvas.GetComponent<RectTransform>();
        float scaleX = canvasRect.rect.width / ReferenceResolution.x;
        float scaleY = canvasRect.rect.height / ReferenceResolution.y;
        return new Vector2(refPos.x * scaleX, refPos.y * scaleY);
    }
}