using UnityEngine;

public class SolarController : MonoBehaviour
{
    [SerializeField] private float _rotationSpeed;
    [SerializeField] private float _autoRotationSpeed;
    [SerializeField] private float _cameraLerpSpeed;
    [SerializeField] private float _fastMult;

    private GameObject _sunLight;
    private GameObject _sunCylinder;

    private Camera _mainCamera;
    private bool _isDragging = false;
    private Vector3 _lastMousePosition;
    private Transform _cachedCameraTransform;
    private Transform _cachedSunLightTransform;
    private Transform _cachedSunCylinderTransform;

    private bool _isFast = false;
    private float _rotateAngle;
    private float _currentCameraAngle = 0f;
    private float _currentLightAngle = 85f;

    private float _rotationAmount;
    private Vector3 _dir;

    private void Start()
    {
        Init();
    }

    public void Init()
    {
        _mainCamera = Camera.main;
        _sunLight = GameObject.Find("SunLight");
        _sunCylinder = GameObject.Find("LightArea");
        _cachedCameraTransform = _mainCamera.transform;
        _cachedSunLightTransform = _sunLight.transform;
        _cachedSunCylinderTransform = _sunCylinder.transform;
        _currentLightAngle = 85f;
        _currentCameraAngle = 0f;
        _isFast = false;
        GameManager.Instance.OnMoveNodeAction += SetRotate;
    }

    private void Update()
    {
        // 입력 처리
        if (!_isFast && Input.GetMouseButtonDown(0))
        {
            _isDragging = true;
            _lastMousePosition = Input.mousePosition;
        }
        else if (Input.GetMouseButtonUp(0))
        {
            _isDragging = false;
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            SetRotate(1);
        }

        // 회전량 계산
        if (_isFast)
        {
            _rotationAmount = _rotationSpeed * _fastMult * Time.deltaTime;
            _currentCameraAngle += _rotationAmount;
            _dir = Vector3.down;
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
            _rotationAmount = deltaX * _rotationSpeed * Time.deltaTime;
            _lastMousePosition = currentMousePosition;
            _dir = Vector3.up;
        }
        else
        {
            _rotationAmount = _autoRotationSpeed * Time.deltaTime;
            _dir = Vector3.down;
        }

        // 회전 적용
        _cachedCameraTransform.RotateAround(transform.position, _dir, _rotationAmount);
        _cachedSunCylinderTransform.RotateAround(transform.position, Vector3.down, _isFast ? _rotationAmount : _autoRotationSpeed * Time.deltaTime);

        // Sun Light 회전
        _currentLightAngle -= _isFast ? _rotationAmount : _autoRotationSpeed * Time.deltaTime;
        if (_currentLightAngle <= 85f - 360f)
        {
            _currentLightAngle += 360f;
        }
        _cachedSunLightTransform.eulerAngles = new Vector3(0, _currentLightAngle, 0);

        // 카메라 크기 조정
        float targetSize = _isFast ? 4f : 5f;
        float currentSize = _mainCamera.orthographicSize;
        if (Mathf.Abs(currentSize - targetSize) > 0.01f)
        {
            _mainCamera.orthographicSize = Mathf.Lerp(currentSize, targetSize, Time.deltaTime * _cameraLerpSpeed);
        }
    }

    // 현재 노드에서 해당 노드까지 이동
    private void SetRotate(int rotateAngle)
    {
        //int cur = GameManager.Instance.CurrentNodeIndex;
        _isFast = true;
        _rotateAngle = 45f;
        _currentCameraAngle = 0f;
    }
}