using UnityEngine;

public class SolarController : MonoBehaviour
{
    [SerializeField] private float _rotationSpeed;
    [SerializeField] private float _fastRotationSpeed;
    private Camera _mainCamera;
    private float _cameraLerpSpeed = 2f;

    private bool _isFast = false;
    private float _currentAngle = 0;
    private float _targetAngle;

    public void Start()
    {
        _mainCamera = Camera.main;
    }

    public void Init()
    {
        _currentAngle = 0f; 
        _isFast = false;
        if (_mainCamera == null)
            _mainCamera = Camera.main;
    }

    private void Update()
    {
        // 테스트용 코드
        if (Input.GetKeyDown(KeyCode.Space))
        {
            SetRotate(50f);
        }

        // 이 부분에서 GameManager의 시간과 동기화 필요
        if (!_isFast)
            transform.Rotate(0, _rotationSpeed * Time.deltaTime, 0);
        else
        {
            float rotationThisFrame = _fastRotationSpeed * Time.deltaTime;
            transform.Rotate(0, rotationThisFrame, 0);

            _currentAngle += rotationThisFrame;

            if (_currentAngle >= _targetAngle)
            {
                _isFast = false;
                _currentAngle = 0;
            }
        }

        // 빨리감기할 때 줌인(4), 평소 상태일 때 줌아웃(5)
        float targetSize = _isFast ? 4f : 5f;
        _mainCamera.orthographicSize = Mathf.Lerp(
            _mainCamera.orthographicSize,
            targetSize,
            Time.deltaTime * _cameraLerpSpeed
        );
    }

    /// <summary>
    /// targetAngle만큼 회전한다.
    /// </summary>
    /// <param name="targetAngle"></param>
    private void SetRotate(float targetAngle)
    {
        _isFast = true;
        _targetAngle = targetAngle;
        _currentAngle = 0f;
    }
}
