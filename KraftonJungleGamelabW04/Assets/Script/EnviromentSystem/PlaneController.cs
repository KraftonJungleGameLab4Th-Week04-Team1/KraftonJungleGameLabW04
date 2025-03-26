using UnityEngine;

public class PlaneController : MonoBehaviour
{
    private Transform[] _nodeTransforms = new Transform[22]; // Node1 ~ Node22
    private Transform[] _planes = new Transform[22];
    private int _curNodeIdx = 6; 
    private int _targetNodeIdx;
    private Transform _targetPosition;     // 이동 목표 위치
    private float speed = 0.5f;           // 초당 이동 속도 (유닛/초, 조정 가능)
    private bool _isMoving = false;      // 이동 중 여부

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

                    // Node 아래의 Plane 찾기
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
        
        //움직일 때
        transform.position = Vector3.MoveTowards(transform.position, _targetPosition.position, speed * Time.deltaTime);
        if (Vector3.Distance(_targetPosition.position, transform.position) < 0.1f)
        {
            transform.rotation = _planes[_targetNodeIdx].rotation;
            _curNodeIdx = _targetNodeIdx;
            _isMoving = false;

            GameManager.Instance.OnArriveAction?.Invoke(_curNodeIdx);
        }
    }

    private void MovePlayer(int nodeIdx)
    {
        _targetNodeIdx = nodeIdx;
        _targetPosition = _planes[nodeIdx - 1];
        _isMoving = true;
    }
}