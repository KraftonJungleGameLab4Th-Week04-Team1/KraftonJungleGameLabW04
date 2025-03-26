using UnityEngine;

public class SolarSystem : MonoBehaviour
{
    void Start()
    {
        GameManager.Instance.OnChangedGameTimeAction += SolarRotation;
    }

    void Update()
    {
        
    }

    void SolarRotation(float addedGameTime)
    {
        // 1440 주기로 회전 각도를 계산 (0~360도 반복)
        float cycleTime = addedGameTime % 1440; // 1440마다 리셋
        float rotationAngle = -(cycleTime / 4f); // 1440일 때 -360도

        // 초기 회전 (0, 90, 0)에 주기적 회전 적용
        transform.rotation = Quaternion.Euler(0, 90 + rotationAngle, 0);
    }
}
