using Unity.Cinemachine;
using UnityEngine;

public class CameraShake : MonoBehaviour
{
    public float shakeAmplitude = 0.1f;
    public float shakeFrequency = 5f;

    private Vector3 originalPosition;
    private float shakeTimer = 0f;

    void Start()
    {
        originalPosition = transform.localPosition;
    }

    void LateUpdate()
    {
        shakeTimer += Time.deltaTime * shakeFrequency;

        float offsetX = Mathf.PerlinNoise(shakeTimer, 0f) - 0.5f;
        float offsetY = Mathf.PerlinNoise(0f, shakeTimer) - 0.5f;

        Vector3 shakeOffset = new Vector3(offsetX, offsetY, 0f) * shakeAmplitude;
        transform.localPosition = originalPosition + shakeOffset;
    }
}
