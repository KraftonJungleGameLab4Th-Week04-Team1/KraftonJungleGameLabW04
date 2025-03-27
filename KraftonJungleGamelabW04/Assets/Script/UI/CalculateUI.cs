using DG.Tweening;
using UnityEngine;
using UnityEngine.EventSystems;

public class CalculateUI : MonoBehaviour, IPointerDownHandler
{
    private RectTransform _rectTransform;

    [Header("Panel State")]
    [SerializeField] private bool _isPanelOn;
    [SerializeField] private float _panelMoveSpeed = 0.3f;

    private void Start()
    {
        // 자식 오브젝트의 RectTransform을 가져옵니다.
        _rectTransform = gameObject.transform.GetChild(0).GetComponent<RectTransform>();
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        _isPanelOn = !_isPanelOn;
        if (_isPanelOn)
        {
            _rectTransform.DOAnchorPosY(140, _panelMoveSpeed).SetEase(Ease.OutExpo);
        }
        else
        {
            _rectTransform.DOAnchorPosY(30, _panelMoveSpeed).SetEase(Ease.OutExpo);
        }
    }
}
