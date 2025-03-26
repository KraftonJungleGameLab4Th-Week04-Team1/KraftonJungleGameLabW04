using UnityEngine;

public class PlaneController : MonoBehaviour
{
    private Transform[] _nodeTransforms = new Transform[22]; // Node1 ~ Node22
    private Transform[] _planes = new Transform[22];
    private int _curNodeIdx = 6; 
    private int _targetNodeIdx;
    private Vector3 _startPosition;      // 이동 시작 위치
    private Vector3 _targetPosition;     // 이동 목표 위치
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

        for (int i = 0; i < 22; i++)
        {
            Debug.Log(_nodeTransforms[i]);
        }
        for (int i = 0; i < 22; i++)
        {
            Debug.Log(_planes[i]);
        }

        //GameManager.Instance.OnMoveNodeAction += MovePlayer;

    }

    int idx = 7;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            if (idx == 0) idx = 21;
            else idx--; MovePlayer(idx);
        }
        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            if (idx == 21) idx = 0;
            else idx++;
            MovePlayer(idx);
        }
        
        if (!_isMoving) return;

        
        transform.position = Vector3.MoveTowards(transform.position, _targetPosition, speed * Time.deltaTime);
        if (Vector3.Distance(_targetPosition, transform.position) < 0.1f)
        {
            transform.rotation = _planes[_targetNodeIdx].rotation;
            _curNodeIdx = _targetNodeIdx;
            _isMoving = false;
        }
    }

    private void MovePlayer(int nodeIdx)
    {
        _targetNodeIdx = nodeIdx;
        _targetPosition = _planes[nodeIdx - 1].position;
        _isMoving = true;
    }
}