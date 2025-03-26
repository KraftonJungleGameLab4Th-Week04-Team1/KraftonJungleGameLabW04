using System.Collections;
using UnityEngine;

public class SolarController_DE : MonoBehaviour
{
    public float rotationSpeed = 0.2f;

    private Vector2 _lastMousePos;
    private bool _isDragging = false;

    private int _targetNodeIndex = 2;

    void Start()
    {
        GameManager.Instance.OnChangedGameTimeAction += RotateEarth;
        GameManager.Instance.OnMoveNodeAction += HandleSolarMovement;
    }

    // 시간 변화에 따른 지구 회전
    private void RotateEarth(float gameTime)
    {
        transform.eulerAngles = Vector3.up * gameTime * 0.25f;
    }

    private void HandleSolarMovement(int targetIndex)
    {
        int block = targetIndex - GameManager.Instance.CurrentNodeIndex;
        if (block < 0)
        {
            block += 32;
        }

        // StartCoroutine(IncreaseTime(block));
        GameManager.Instance.ChangeGameTime(block * 45);
            
        Debug.Log($"@@DE ---> {block}칸 이동 / {block * 45}분 지남");
    }

    IEnumerator IncreaseTime(int block)
    {
        float duration = 3f;
        float targetValue = block * 45f;
        float currentValue = 0f;
        float elapsed = 0f;
        
        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            float t = Mathf.Clamp01(elapsed / duration);

            currentValue = Mathf.Lerp(0f, targetValue, t);
            GameManager.Instance.ChangeGameTime(currentValue);

            yield return null;
        }

        // 마지막에 정확히 targetValue로 고정
        GameManager.Instance.ChangeGameTime(targetValue);
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
    }
}
