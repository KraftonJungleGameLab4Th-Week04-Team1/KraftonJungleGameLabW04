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
