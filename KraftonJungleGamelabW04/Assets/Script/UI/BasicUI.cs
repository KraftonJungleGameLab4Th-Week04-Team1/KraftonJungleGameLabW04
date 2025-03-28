using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using DG.Tweening;
using UnityEngine.SceneManagement;

public class BasicUI : MonoBehaviour, IPointerDownHandler
{
    private RectTransform _rectTransform;

    [Header("Panel State")]
    [SerializeField] private bool _isPanelOn;
    [SerializeField] private float _panelMoveSpeed = 0.4f;

    [Header("UI Elements")]
    [SerializeField] private TMP_Text _foodText;
    [SerializeField] private TMP_Text _boltText;
    [SerializeField] private TMP_Text _nutText;
    [SerializeField] private TMP_Text _fuelText;
    [SerializeField] private TMP_Text _currentWeightText;
    [SerializeField] private TMP_Text _currentAircraftStateText;

    private void Start()
    {
        // 자식 오브젝트의 RectTransform을 가져옵니다.
        _rectTransform = gameObject.transform.GetChild(0).GetComponent<RectTransform>();
        UpdateBasicUI();
    }

    public void Init()
    {
        GameManager.Instance.OnChangedGameTimeAction += _ => UpdateBasicUI();
        GameManager.Instance.OnMoveNodeAction += _ => UpdateBasicUI();
        GameManager.Instance.OnConfirmAction += IgnoreParamsUpdateBasicUI;
        GameManager.Instance.OnArriveAction += _ => UpdateBasicUI();
    }

    // 애니메이션 만들어서 위아래 이동하게 만들기
    public void OnPointerDown(PointerEventData eventData)
    {
        _isPanelOn = !_isPanelOn;
        if (_isPanelOn)
        {
            _rectTransform.DOAnchorPosY(-160, _panelMoveSpeed).SetEase(Ease.OutExpo);
        }
        else
        {
            _rectTransform.DOAnchorPosY(-40, _panelMoveSpeed).SetEase(Ease.OutExpo);
        }
    }

    private void IgnoreParamsUpdateBasicUI(ResourceDto nodeValue, ResourceDto aircraftValue)
    {
        UpdateBasicUI();
    }

    private void UpdateBasicUI()
    {
        AircraftManager aircraft = GameManager.Aircraft;
        _foodText.text = $"{aircraft.Food}";
        _boltText.text = $"{aircraft.Bolt}";
        _nutText.text = $"{aircraft.Nut}";
        _fuelText.text = $"{aircraft.Fuel}";
        _currentWeightText.text = $"{aircraft.CurrentWeight}";
        _currentAircraftStateText.text = $"{aircraft.CurrentAircraftState}";
    }
}
