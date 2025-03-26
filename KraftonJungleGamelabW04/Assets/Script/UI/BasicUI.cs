using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class BasicUI : MonoBehaviour, IPointerDownHandler
{
    [SerializeField] private Canvas _canvas;
    private Animator _animator;

    [SerializeField] private TMP_Text _foodText;
    [SerializeField] private TMP_Text _boltText;
    [SerializeField] private TMP_Text _nutText;
    [SerializeField] private TMP_Text _fuelText;
    [SerializeField] private TMP_Text _currentWeightText;
    [SerializeField] private TMP_Text _currentAircraftStateText;
    [SerializeField] private Button _panelOnOffBtn;

    [SerializeField] private bool _isPanelOn = true;

    private void Start()
    {
        //_panelOnOffBtn.onClick.AddListener(OnClickPanelOnOffBtn);
        _animator = GetComponent<Animator>();

        GameManager.Instance.OnChangedGameTimeAction += _ => UpdateBasicUI();
        GameManager.Instance.OnConfirmUseAction += _ => UpdateBasicUI();
        GameManager.Instance.OnConfirmGainAction += _ => UpdateBasicUI();
        GameManager.Instance.OnMoveNodeAction += _ => UpdateBasicUI();
    }

    // 애니메이션 만들어서 위아래 이동하게 만들기
    public void OnPointerDown(PointerEventData eventData)
    {
        _isPanelOn = !_isPanelOn;
        if (_isPanelOn)
        {
            _animator.SetTrigger("Off");
        }
        else
        {
            _animator.SetTrigger("On");
        }
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
