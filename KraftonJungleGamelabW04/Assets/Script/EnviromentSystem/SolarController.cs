using Unity.VisualScripting;
using UnityEditor.Experimental.GraphView;
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
    private Transform _cameraTransform;
    private Transform _sunLightTransform;
    private Transform _sunCylinderTransform;

    private bool _isFast = false;
    private float _rotateAngle;
    [SerializeField] private float _currentCameraAngle = 0f;
    private float _currentLightAngle = 85f;

    private float _rotationAmount;
    private Vector3 _dir;

    [SerializeField] private float _targetAngle;

    // 테스트용
    private int curNode = 4;
    private int targetNode;

    private void Start()
    {
        _mainCamera = Camera.main;
        _sunLight = GameObject.Find("SunLight");
        _sunCylinder = GameObject.Find("LightArea");
        _cameraTransform = _mainCamera.transform;
        _sunLightTransform = _sunLight.transform;
        _sunCylinderTransform = _sunCylinder.transform;

        GameManager.Instance.OnMoveNodeAction += SetRotate;
    }

    public void Init()
    {
        _currentLightAngle = 85f;
        _currentCameraAngle = 0f;
        _targetAngle = 0f;
        _isFast = false;
    }

    private void Update()
    {
        float _rotationAmount = 0f;

        if (!_isFast && !_isDragging)
        {
            _rotationAmount = _autoRotationSpeed * Time.deltaTime; // 초당 0.25도
            
            _currentCameraAngle += _rotationAmount; // 현재 각도 추적
            if (_currentCameraAngle >= 360f)
                _currentCameraAngle %= 360f;
        }
        else if (_isFast)
        {
            _rotationAmount = _rotationSpeed * _fastMult * Time.deltaTime;
            _currentCameraAngle += _rotationAmount;
            
            _dir = Vector3.down;

            //카메라는 반대 방향으로 갈 수 있음
            if (Mathf.Abs(_currentCameraAngle - _targetAngle) < 1f)
            {
                _currentCameraAngle = _targetAngle;
                _isFast = false;
            }

            if (_currentCameraAngle >= 360f)
                _currentCameraAngle %= 360f;
        }

        _cameraTransform.RotateAround(transform.position, Vector3.down, _rotationAmount);
        _sunCylinderTransform.RotateAround(transform.position, Vector3.down, _rotationAmount);
    }

    // 현재 노드에서 해당 노드까지 이동
    private void SetRotate(int node)
    {
        targetNode = node;

        if (targetNode < curNode)
        {

        }
        float[] _angles = new float[] { 300, 300, 300, 33, 80, 96, 112, 128, 144, 160, 176, 192, 208, 224, 240, 256, 272, 288, 304, 320, 336, 352 };

        _targetAngle = _angles[node - 1] - _currentCameraAngle;
        if (_targetAngle < 0f)
            _targetAngle += 360f;
        _isFast = true;
    }

    private void OnDestroy()
    {
        GameManager.Instance.OnMoveNodeAction -= SetRotate;
    }
}