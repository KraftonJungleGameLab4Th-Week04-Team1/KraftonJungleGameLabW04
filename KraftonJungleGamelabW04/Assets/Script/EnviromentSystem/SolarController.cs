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
    private Transform _deadZoneTransform;
    private Transform _sunCylinderTransform;

    private bool _isFast = false;
    private float _currentLightAngle = 85f;
    [SerializeField] private float _currentCameraAngle = 0f;

    private Vector3 _cameraDir;

    [SerializeField] private float _targetAngle;

    //테스트용
    private int _curIdx = 5;
    private int _targetIdx = 10;
    private int idx = 1;

    private void Start()
    {

        _mainCamera = Camera.main;
        _sunLight = GameObject.Find("SunLight");
        _sunCylinder = GameObject.Find("LightArea");
        _cameraTransform = _mainCamera.transform;
        _sunLightTransform = _sunLight.transform;
        //_deadZoneTransform = _sunLight.transform.GetChild(0).transform;
        if (_sunCylinder != null)
            _sunCylinderTransform = _sunCylinder.transform;

        GameManager.Instance.OnMoveNodeAction += SetRotate;

        for (int i = 1; i <= 22; i++)
            Debug.Log(NodeManager.NodeDic[i].NodeName);
    }

    public void Init()
    {
        _currentCameraAngle = 0f;
        _targetAngle = 0f;
        _isFast = false;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            SetRotate(idx);
            idx++;
            if (idx == 23) idx = 1;
        }
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

        float _cameraRotationAmount = 0f;
        float _cylinderRotationAmount = 0f;

        // 자동 회전
        if (!_isFast && !_isDragging)
        {
            // 자동 회전 시 카메라와 태양 같은 속도로 회전
            _cameraRotationAmount = _autoRotationSpeed * Time.deltaTime; // 초당 0.25도
            _cylinderRotationAmount = _cameraRotationAmount;

            _cameraDir = Vector3.down;

            _currentCameraAngle += _cameraRotationAmount; // 현재 각도 추적
            if (_currentCameraAngle >= 360f)
                _currentCameraAngle %= 360f;
        }
        else if (_isFast)
        {
            // 빠른 회전 시 카메라와 태양 같은 속도로 회전
            _cameraRotationAmount = _rotationSpeed * _fastMult * Time.deltaTime;
            _currentCameraAngle += _cameraRotationAmount;
            _cylinderRotationAmount = _cameraRotationAmount;

            if (Mathf.Abs(_currentCameraAngle - _targetAngle) < 1f)
            {
                _currentCameraAngle = _targetAngle;
                _isFast = false;
            }

            if (_currentCameraAngle >= 360f)
                _currentCameraAngle %= 360f;
        }
        else if (_isDragging)
        {
            // 드래그 시 카메라는 드래그 속도로 회전, 태양은 원래 속도로 회전
            Vector3 currentMousePosition = Input.mousePosition;
            float deltaX = currentMousePosition.x - _lastMousePosition.x;
            _cameraRotationAmount = deltaX * _rotationSpeed * Time.deltaTime;
            _lastMousePosition = currentMousePosition;
            _cameraDir = Vector3.up;

             _cylinderRotationAmount = _autoRotationSpeed * Time.deltaTime;
        }

        // 태양 실린더는 항상 같은 방향으로 회전
        _cameraTransform.RotateAround(transform.position, _cameraDir, _cameraRotationAmount);
        if (_sunCylinderTransform != null)
            _sunCylinderTransform.RotateAround(transform.position, Vector3.down, _cylinderRotationAmount);

        // 빛은 태양 실린더와 동일하게 회전
        _currentLightAngle -= _cylinderRotationAmount;
        if (_currentLightAngle > 360f)
            _currentLightAngle %= 360f;
        _sunLightTransform.eulerAngles = new Vector3(0, _currentLightAngle, 0);

        float targetSize = _isFast ? 4f : 5f;
        float currentSize = _mainCamera.orthographicSize;
        if (Mathf.Abs(currentSize - targetSize) > 0.01f)
        {
            _mainCamera.orthographicSize = Mathf.Lerp(currentSize, targetSize, Time.deltaTime * _cameraLerpSpeed);
        }
    }

    // 현재 노드에서 해당 노드까지 이동
    private void SetRotate(int targetNodeIdx)
    {
        //Node targetNode = NodeManager.NodeDic[targetNodeIdx];
        //Node curNode = NodeManager.NodeDic[GameManager.Instance.CurrentNodeIndex];

        Node targetNode = NodeManager.NodeDic[targetNodeIdx];
        Node curNode = NodeManager.NodeDic[4];
        Debug.Log(targetNode.name);
        Debug.Log(curNode.name);

        if (targetNode == null) Debug.Log("targetnode null");
        if (curNode == null) Debug.Log("curNode null");

        // 나중에 삭제
        _targetIdx = targetNodeIdx;

        // 만약 이전 노드그룹이라면 이동할 수 없음
        if (curNode.NodeGroup == 1 && targetNode.NodeGroup == 5) return;
        if (curNode.NodeGroup < 1 && targetNode.NodeGroup - curNode.NodeGroup == 1) return;

        // 이동하려는 노드의 각도를 타겟으로 설정
        // 카메라 회전
        // 만약 같은 그룹 안에 있고 숫자가 나보다 작으면 왼쪽으로,
        // 만약 같은 그룹 안에 있고 숫자가 나보다 크면 오른쪽으로,
        // 만약 다른 그룹에 있으면 오른쪽으로
        float[] _angles = new float[] { 300, 300, 300, 33, 80, 96, 112, 128, 144, 160, 176, 192, 208, 224, 240, 256, 272, 288, 304, 320, 336, 352 };
        _targetAngle = _angles[targetNodeIdx - 1];

        if (curNode.NodeGroup == targetNode.NodeGroup && curNode.NodeIdx > targetNodeIdx)
            _cameraDir = Vector3.up; // 왼쪽? 나중에 확인하고 수정
        else
            _cameraDir = Vector3.down; // 오른쪽? 나중에 확인하고 수정

        _isFast = true;
    }

    private void OnDestroy()
    {
        GameManager.Instance.OnMoveNodeAction -= SetRotate;
    }
}