using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] private float _autoRotationSpeed;
    [SerializeField] private float _rotationSpeed;
    private Camera _mainCamera;
    private Transform _cameraTransform;
    private bool _isDragging = false;
    private Vector3 _lastMousePosition;

    [SerializeField] private float _currentCameraAngle = 0f;
    private Vector3 _cameraDir;

    void Start()
    {
        _mainCamera = Camera.main;
        _cameraTransform = _mainCamera.transform;
    }

    void Update()
    {
        // 입력 처리
        if (Input.GetMouseButtonDown(0))
        {
            _isDragging = true;
            _lastMousePosition = Input.mousePosition;
        }
        else if (Input.GetMouseButtonUp(0))
        {
            _isDragging = false;
        }

        float _cameraRotationAmount = 0f;

        if (!_isDragging)
        {
            _cameraRotationAmount = _autoRotationSpeed * Time.deltaTime; // 초당 0.25도
            _cameraDir = Vector3.down;
            _currentCameraAngle += _cameraRotationAmount; // 현재 각도 추적
        }
        else
        {
            Vector3 currentMousePosition = Input.mousePosition;
            float deltaX = currentMousePosition.x - _lastMousePosition.x;
            _cameraRotationAmount = deltaX * _rotationSpeed * Time.deltaTime;
            _lastMousePosition = currentMousePosition;
            _cameraDir = Vector3.up;
        }
        if (_currentCameraAngle >= 360f)
            _currentCameraAngle %= 360f;

        _cameraTransform.RotateAround(transform.position, _cameraDir, _cameraRotationAmount);
    }
}
