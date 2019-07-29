using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public enum DoTweenFade
{
    To = 0,
    From = 1
}

public enum DoTweenType
{
    None,
    TweenFadeCanvasGroup,
    TweenFadeImage,
    TweenRectTransformPosition,
    TweenRectTransformScale,
    TweenTargetPosition,
    TutorialTween
}

public class DoTweenController : MonoBehaviour
{
    [SerializeField]
    private float doTweenDelay;

    [SerializeField]
    private Vector3 toLocation = Vector3.zero;

    [SerializeField]
    private Vector3 fromLocation = Vector3.zero;

    [Range(0.0f, 10.0f), SerializeField]
    private float moveDuration = 1.0f;

    [SerializeField]
    private Ease moveEase = Ease.Linear;

    //[SerializeField]
    //private Color targetColor;

    [Range(1.0f, 500.0f), SerializeField]
    private float scaleMultiplier = 3.0f;

    //[Range(1.0f, 10.0f), SerializeField]
    //private float colorChangeDuration = 1.0f;

    [SerializeField]
    private DoTweenFade fade;

    [SerializeField]
    private DoTweenType _doTweenType = DoTweenType.None;

    [SerializeField]
    private LoopType loopType;

    [SerializeField]
    private int loops;

    public float ScaleMultiplier { get => scaleMultiplier; set => scaleMultiplier = value; }
    public float MoveDuration { get => moveDuration; set => moveDuration = value; }
    public float DoTweenDelay { get => doTweenDelay; set => doTweenDelay = value; }
    public DoTweenFade FadeValue { get => fade; set => fade = value; }
    public Vector3 FromLocation { get => fromLocation; set => fromLocation = value; }
    public Vector3 ToLocation { get => toLocation; set => toLocation = value; }

    private void Start()
    {
    }

    public void DOTweenPlay()
    {
        switch (_doTweenType)
        {
            case DoTweenType.None:
                break;
            case DoTweenType.TweenFadeCanvasGroup:
                GetComponent<CanvasGroup>().DOFade((int)fade, moveDuration).SetDelay(doTweenDelay);
                break;
            case DoTweenType.TweenFadeImage:
                GetComponent<Image>().DOFade((int)fade, moveDuration).SetDelay(doTweenDelay);
                break;
            case DoTweenType.TweenRectTransformPosition:
                GetComponent<RectTransform>().anchoredPosition = fromLocation;
                GetComponent<RectTransform>().DOAnchorPos(toLocation, moveDuration).SetEase(moveEase).SetDelay(doTweenDelay).SetLoops(loops, loopType);
                break;
            case DoTweenType.TweenRectTransformScale:
                GetComponent<RectTransform>().DOScale(scaleMultiplier, moveDuration / 2.0f).SetEase(moveEase).SetDelay(doTweenDelay);
                break;
            case DoTweenType.TweenTargetPosition:
                transform.position = fromLocation;
                transform.DOMove(toLocation, moveDuration).SetEase(moveEase).SetDelay(doTweenDelay).SetLoops(loops, loopType);
                break;
            case DoTweenType.TutorialTween:
                DOTweenUp();
                break;
            default:
                break;
        }
    }

    private void DOTweenUp()
    {
        transform.DOMove(toLocation, moveDuration).SetEase(moveEase).OnComplete(OnCompleteUp);
    }

    private void DOTweenDown()
    {
        transform.position = toLocation;
        transform.DOMove(FromLocation, moveDuration).SetEase(moveEase).OnComplete(OnCompleteDown);
    }

    private void OnCompleteUp()
    {
        var image = transform.Find("Image");
        transform.GetComponent<Image>().enabled = false;
        image.gameObject.SetActive(false);
        DOTweenDown();
    }

    private void OnCompleteDown()
    {
        var image = transform.Find("Image");
        transform.GetComponent<Image>().enabled = true;
        image.gameObject.SetActive(true);
        DOTweenUp();
    }
}
