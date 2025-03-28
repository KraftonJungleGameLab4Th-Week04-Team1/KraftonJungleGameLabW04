using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ButtonHoldHandler : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    private bool _isButtonHold = false;

    private float _holdDuration = 0.2f;
    private float _repeatInterval = 0.1f;
    private float _holdTimer = 0f;
    private float _repeatTimer = 0f;

    private Button _button; // Button 컴포넌트 참조

    [SerializeField] private bool _isIncrease;
    [SerializeField] private bool _isFood;
    [SerializeField] private bool _isFuel;
    [SerializeField] private bool _isBolt;
    [SerializeField] private bool _isNut;
    [SerializeField] private bool _isBoltToUse;
    [SerializeField] private bool _isNutToUse;

    private ReportManager _reportSliderUI;
 
    void Start()
    {
        _button = GetComponent<Button>();
        _reportSliderUI = FindAnyObjectByType<ReportManager>();
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        _isButtonHold = true;
        _holdTimer = 0f;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        _isButtonHold = false;
        _holdTimer = 0f;
    }

    private void Update()
    {
        if (_isButtonHold)
        {
            _holdTimer += Time.deltaTime;
            _repeatTimer += Time.deltaTime;
            if (_holdTimer >= _holdDuration)
            {
                if (_repeatTimer >= _repeatInterval)
                {
                    _repeatTimer = 0f;
                    // 증가할때
                    if (_isIncrease)
                    {
                        if (_isFood) _reportSliderUI.TakeFood();
                        if (_isFuel) _reportSliderUI.TakeFuel();
                        if (_isBolt) _reportSliderUI.TakeBolt();
                        if (_isNut) _reportSliderUI.TakeNut();
                        if (_isBoltToUse) _reportSliderUI.AddBoltToUse();
                        if (_isNutToUse) _reportSliderUI.AddNutToUse();
                    }
                    // 감소할때
                    else
                    {
                        if (_isFood) _reportSliderUI.ReleaseFood();
                        if (_isFuel) _reportSliderUI.ReleaseFuel();
                        if (_isBolt) _reportSliderUI.ReleaseBolt();
                        if (_isNut) _reportSliderUI.ReleaseNut();
                        if (_isBoltToUse) _reportSliderUI.DecreaseBoltToUse();
                        if (_isNutToUse) _reportSliderUI.DecreaseNutToUse();
                    }
                }
            }
        }
    }
}