using UnityEngine;

public class EndingRocket : MonoBehaviour
{
    public float speed = 10f;

    void Update()
    {
        transform.position += transform.forward * speed * Time.deltaTime;
    }
}
