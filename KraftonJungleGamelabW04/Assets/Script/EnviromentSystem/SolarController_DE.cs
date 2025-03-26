using UnityEngine;

public class SolarController_DE : MonoBehaviour
{
    public float rotationSpeed = 0.2f;

    private Vector2 _lastMousePos;
    private bool _isDragging = false;

    private int _targetNodeIndex = 2;
    public Node currentNode;
    public Node targetNode;
    
    void Start()
    {
        GameManager.Instance.OnChangedGameTimeAction += RotateEarth;

        // 테스트용 Node
        currentNode = NodeManager.NodeDic[1];
        targetNode = NodeManager.NodeDic[_targetNodeIndex];
    }

    // 시간 변화에 따른 지구 회전
    private void RotateEarth(float gameTime)
    {
        transform.eulerAngles = Vector3.up * gameTime * 0.25f;
    }
    
    void Update()
    {
        // 드래그에 따른 카메라 회전 
        if (Input.GetMouseButtonDown(0))
        {
            _lastMousePos = Input.mousePosition;
            _isDragging = true;
        }

        if (Input.GetMouseButton(0) && _isDragging)
        {
            Vector2 delta = (Vector2)Input.mousePosition - _lastMousePos;

            float rotY = delta.x * rotationSpeed;
            Camera.main.transform.RotateAround(transform.position, Vector3.up, rotY);

            _lastMousePos = Input.mousePosition;
        }

        if (Input.GetMouseButtonUp(0))
        {
            _isDragging = false;
        }

        // 노드 거리 차에 따른 지구 회전 (1칸당 45분 지남) 
        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            var block = targetNode.NodeIdx - currentNode.NodeIdx;
            if (block < 0)
            {
                block += 32;
            }
            GameManager.Instance.ChangeGameTime(block * 45);
            
            Debug.Log($"@@DE ---> {block}칸 이동 / {block * 45}분 지남");
            
            currentNode = NodeManager.NodeDic[_targetNodeIndex];
            _targetNodeIndex++;
            if (_targetNodeIndex >= NodeManager.NodeDic.Count)
            {
                _targetNodeIndex = 1;
            }
            targetNode = NodeManager.NodeDic[_targetNodeIndex];
        }
    }
}
