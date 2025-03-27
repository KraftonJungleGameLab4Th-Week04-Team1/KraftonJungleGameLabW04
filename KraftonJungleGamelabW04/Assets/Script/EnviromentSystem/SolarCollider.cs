using UnityEngine;

public class SolarCollider : MonoBehaviour
{
    private Rigidbody _rb;

    private void Start()
    {
        _rb = GetComponent<Rigidbody>();
    }

    private void OnTriggerEnter(Collider other)
    {
        // 레이어 Plane이면
        if(other.gameObject.layer == LayerMask.NameToLayer("Plane"))
        {
            Debug.Log("게임오버");
            GameManager.Instance.GameState = GameState.GameOver;
        }
    }
}
