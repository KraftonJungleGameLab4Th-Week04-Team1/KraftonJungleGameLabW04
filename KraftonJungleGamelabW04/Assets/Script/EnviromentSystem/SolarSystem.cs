using UnityEngine;

public class SolarSystem : MonoBehaviour
{
    private bool _isRotating = false;        // 회전 중 여부
    private Quaternion _startRotation;       // 시작 회전
    private Quaternion _targetRotation;      // 목표 회전
    private float _rotationTime = 0f;        // 현재 회전 시간
    private float _rotationDuration = 2.5f;    // 회전 지속 시간 (3초)

    void Start()
    {
        GameManager.Instance.OnChangedGameTimeAction += SolarRotation;
        // 초기 회전 설정
        transform.rotation = Quaternion.Euler(0, 90, 0);
    }

    void Update()
    {
        if (!_isRotating) return;

        _rotationTime += Time.deltaTime;
        float t = _rotationTime / _rotationDuration; // 보간 비율 (0~1)

        if (t >= 1f)
        {
            // 목표 회전에 도달
            transform.rotation = _targetRotation;
            _isRotating = false;
            _rotationTime = 0f;
            return;
        }

        // 부드럽게 회전 보간
        transform.rotation = Quaternion.Lerp(_startRotation, _targetRotation, t);
    }

    void SolarRotation(float addedGameTime)
    {
        // 1440 주기로 회전 각도를 계산 (0~360도 반복)
        float cycleTime = addedGameTime % 1440; // 1440마다 리셋
        float rotationAngle = -(cycleTime / 4f); // 1440일 때 -360도

        // 시작 회전 저장
        _startRotation = transform.rotation;

        // 목표 회전 설정
        _targetRotation = Quaternion.Euler(0, 90 + rotationAngle, 0);

        // 회전 시작
        _isRotating = true;
        _rotationTime = 0f;
    }
}