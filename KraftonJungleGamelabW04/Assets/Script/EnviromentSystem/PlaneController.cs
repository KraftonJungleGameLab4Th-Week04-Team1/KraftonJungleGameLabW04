using UnityEngine;

public class PlaneController : MonoBehaviour
{
    private Transform[] _nodeTransforms = new Transform[22]; // Node1 ~ Node22
    private Transform[] _planes = new Transform[22];
    private int _curNodeIdx = 6;
    private int _targetNodeIdx;
    private bool _isMoving = false;

    // 원기둥 설정
    public Transform cylinderCenter; // 원기둥 중심
    public float radius = 5f;       // 원기둥 반지름
    public float moveDuration = 2f; // 고정 이동 시간 (초)

    // 이동 보간용 변수
    private float _startTheta; // 시작 각도
    private float _startY;     // 시작 높이 (Y축)
    private float _targetTheta; // 목표 각도
    private float _targetY;     // 목표 높이 (Y축)
    private float _currentTime; // 현재 이동 시간

    void Start()
    {
        GameObject nodes = GameObject.Find("Nodes");
        if (nodes != null)
        {
            for (int i = 0; i < 22; i++)
            {
                string nodeName = $"Node{i + 1}";
                Transform nodeTransform = nodes.transform.Find(nodeName);
                if (nodeTransform != null)
                {
                    _nodeTransforms[i] = nodeTransform;
                    Transform planeTransform = nodeTransform.Find($"Plane{i + 1}");
                    if (planeTransform != null)
                    {
                        _planes[i] = planeTransform;
                    }
                    else
                    {
                        Debug.LogError($"{nodeName} 아래에 Plane{i + 1}을(를) 찾을 수 없습니다.");
                    }
                }
                else
                {
                    Debug.LogError($"{nodeName}을(를) 찾을 수 없습니다.");
                }
            }
        }

        GameManager.Instance.OnMoveNodeAction += MovePlayer;
    }

    private void Update()
    {
        if (!_isMoving) return;

        _currentTime += Time.deltaTime;
        float t = _currentTime / moveDuration; // 보간 비율 (0~1)

        if (t >= 1f)
        {
            // 목표 지점 도달
            transform.position = _planes[_targetNodeIdx].position;
            transform.rotation = _planes[_targetNodeIdx].rotation;
            _curNodeIdx = _targetNodeIdx;
            _isMoving = false;
            GameManager.Instance.OnArriveAction?.Invoke(_curNodeIdx + 1);
            return;
        }

        // 이전 위치 저장
        Vector3 prevPosition = transform.position;

        // θ와 y 보간
        float theta = Mathf.LerpAngle(_startTheta, _targetTheta, t); // 각도 보간
        float y = Mathf.Lerp(_startY, _targetY, t); // 높이 보간 (Y축)

        // 원기둥 좌표를 직교 좌표로 변환 (Y축이 중심축)
        float x = radius * Mathf.Cos(theta * Mathf.Deg2Rad);
        float z = radius * Mathf.Sin(theta * Mathf.Deg2Rad);
        Vector3 newPosition = cylinderCenter.position + new Vector3(x, y, z);
        transform.position = newPosition;

        // 이동 방향 계산 및 회전 설정
        Vector3 moveDirection = (newPosition - prevPosition).normalized;
        if (moveDirection != Vector3.zero) // 방향 벡터가 0이 아닌 경우에만 회전
        {
            transform.rotation = Quaternion.LookRotation(moveDirection, Vector3.up);
        }
    }

    private void MovePlayer(int nodeIdx)
    {
        _targetNodeIdx = nodeIdx - 1; // 인덱스 조정 (1-based -> 0-based)

        // 시작점과 목표점의 원기둥 좌표 계산
        Vector3 startPos = transform.position - cylinderCenter.position;
        Vector3 targetPos = _planes[_targetNodeIdx].position - cylinderCenter.position;

        _startTheta = Mathf.Atan2(startPos.z, startPos.x) * Mathf.Rad2Deg; // 시작 θ (도, XZ 평면)
        _startY = startPos.y; // 시작 y (높이)
        _targetTheta = Mathf.Atan2(targetPos.z, targetPos.x) * Mathf.Rad2Deg; // 목표 θ (도, XZ 평면)
        _targetY = targetPos.y; // 목표 y (높이)

        // 최단 경로를 위해 θ 조정 (360도 넘지 않도록)
        float deltaTheta = _targetTheta - _startTheta;
        if (deltaTheta > 180f) _targetTheta -= 360f;
        else if (deltaTheta < -180f) _targetTheta += 360f;

        // 이동 거리 계산 (디버깅 참고)
        float arcLength = radius * Mathf.Abs((_targetTheta - _startTheta) * Mathf.Deg2Rad); // 호의 길이
        float heightDiff = Mathf.Abs(_targetY - _startY); // 높이 차이 (Y축)
        float totalDistance = Mathf.Sqrt(arcLength * arcLength + heightDiff * heightDiff); // 측지선 거리

        _currentTime = 0f;
        _isMoving = true;
    }
}