using UnityEngine;

public class SolarController : MonoBehaviour
{
    [SerializeField] private float _rotationSpeed = 100f;     // 드래그 회전 속도
    [SerializeField] private float _orbitSpeed = 360f;        // Sun Light 기본 회전 속도
    [SerializeField] private float _autoRotationSpeed = 30f;  // 자동 회전 속도
    [SerializeField] private float _cameraLerpSpeed = 2f;     // 카메라 크기 변화 속도
    [SerializeField] private float _fastMult = 2f;            // 빠른 회전 배수

    private Camera _mainCamera;
    private bool _isDragging = false;
    private Vector3 _lastMousePosition;
    private Transform _sunLightTransform;
    private float _currentLightAngle = 85f;

    private bool _isFast = false;
    private float _rotateAngle;
    private float _currentCameraAngle = 0f;

    private float rotationAmount;
    private Vector3 dir;

    private void Start()
    {
        _mainCamera = Camera.main;
        _sunLightTransform = GameObject.Find("Sun Light").transform;
        Init();
    }

    public void Init()
    {
        if (_mainCamera == null)
            _mainCamera = Camera.main;
        if (_sunLightTransform == null)
            _sunLightTransform = GameObject.Find("Sun Light").transform;

        _currentLightAngle = 85f;
        _currentCameraAngle = 0f;
        _isFast = false;
    }

    private void Update()
    {
        // 마우스 입력 처리
        if (!_isFast && Input.GetMouseButtonDown(0))
        {
            _isDragging = true;
            _lastMousePosition = Input.mousePosition;
        }
        else if (Input.GetMouseButtonUp(0))
        {
            _isDragging = false;
        }

        // 테스트용
        if (Input.GetKeyDown(KeyCode.Space))
        {
            SetRotate(90f);
        }

        // 회전량 계산
        if (_isFast)
        {
            rotationAmount = _rotationSpeed * _fastMult * Time.deltaTime;
            _currentCameraAngle += rotationAmount;
            dir = Vector3.down;
            if (_currentCameraAngle >= _rotateAngle)
            {
                _isFast = false;
                _currentCameraAngle = 0f;
            }
        }
        else if (_isDragging)
        {
            Vector3 currentMousePosition = Input.mousePosition;
            float deltaX = currentMousePosition.x - _lastMousePosition.x;
            rotationAmount = deltaX * _rotationSpeed * Time.deltaTime;
            _lastMousePosition = currentMousePosition;
            dir = Vector3.up;
        }
        else if (!_isDragging && !_isFast)
        {
            rotationAmount = _autoRotationSpeed * Time.deltaTime;
            dir = Vector3.down;
        }

        // 카메라 회전
        _mainCamera.transform.RotateAround(
            transform.position,
            dir,
            rotationAmount
        );

        // 카메라 크기 조정
        float targetSize = _isFast ? 4f : 5f;
        _mainCamera.orthographicSize = Mathf.Lerp(
            _mainCamera.orthographicSize,
            targetSize,
            Time.deltaTime * _cameraLerpSpeed
        );

        // Sun Light 회전
        float lightRotationAmount;
        if (_isFast)
        {
            // 카메라와 동일한 속도로 회전
            lightRotationAmount = rotationAmount;
            _currentLightAngle -= lightRotationAmount;
        }
        else
        {
            lightRotationAmount = _orbitSpeed * Time.deltaTime;
            _currentLightAngle -= lightRotationAmount;
        }

        if (_currentLightAngle <= 85f - 360f)
        {
            _currentLightAngle += 360f;
        }

        if (_sunLightTransform != null)
            _sunLightTransform.eulerAngles = new Vector3(0, _currentLightAngle, 0);
    }

    private void SetRotate(float rotateAngle)
    {
        _isFast = true;
        _rotateAngle = rotateAngle;
        _currentCameraAngle = 0f;
    }
}