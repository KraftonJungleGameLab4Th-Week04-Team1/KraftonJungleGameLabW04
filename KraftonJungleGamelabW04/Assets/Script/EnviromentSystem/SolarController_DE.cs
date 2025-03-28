using System.Collections;
using UnityEngine;

/// <summary>
/// camera Controller + Solar Controller????
/// </summary>
public class SolarController_DE : MonoBehaviour
{
    public float rotationSpeed = 0.2f;

    private Vector2 _lastMousePos;
    private bool _isDragging = false;

    void Start()
    {
        GameManager.Instance.OnMoveNodeAction += HandleSolarMovement;
    }

    private void HandleSolarMovement(int targetIndex)
    {
        int block = NodeManager.NodeDic[targetIndex].NodeIdx - NodeManager.NodeDic[GameManager.Instance.CurrentNodeIndex].NodeIdx;
        if (block < 0)
        {
            int b1 = -block;
            int b2 = 32 + block;

            block = Mathf.Min(b1, b2);
        }

        GameManager.Instance.ChangeGameTime(Mathf.Abs(block) * 30);
            
        Debug.Log($"@@DE ---> {block}칸 이동 / {block * 30}분 지남");
    }

    
    /// <summary>
    /// 마우스 맵 입력 처리.
    /// </summary>
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
    }
}
