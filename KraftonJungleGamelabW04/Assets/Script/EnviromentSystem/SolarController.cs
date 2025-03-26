using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using static UnityEditor.ShaderGraph.Internal.KeywordDependentCollection;

public class SolarController : MonoBehaviour
{
    [SerializeField] private float _rotationSpeed;
    

    private GameObject _sunLight;
    private GameObject _sunCylinder;
   
    private Transform _sunLightTransform;
    private Transform _deadZoneTransform;
    private Transform _sunCylinderTransform;
    [SerializeField] private float _currentRotationAngle = 0f;

    private bool _isFast = false;

    private Vector3 _cameraDir;

    private void Start()
    {
        _sunLight = GameObject.Find("SunLight");
        _sunCylinder = GameObject.Find("LightArea");
        _sunLightTransform = _sunLight.transform;
        _sunCylinderTransform = _sunCylinder.transform;
        //_deadZoneTransform = _sunLight.transform.GetChild(0).transform;
    }

    public void Init()
    {
        _currentRotationAngle = 0f;
    }

    private void Update()
    {
        float _cylinderRotationAmount = 0f;

        // 자동 회전
        _cylinderRotationAmount = _rotationSpeed * Time.deltaTime;

        _cameraDir = Vector3.down;

        _currentRotationAngle += _cylinderRotationAmount; // 현재 각도 추적
        if (_currentRotationAngle >= 360f)
            _currentRotationAngle %= 360f;

        // 태양 실린더는 항상 같은 방향으로 회전
        _sunCylinderTransform.RotateAround(transform.position, _cameraDir, _cylinderRotationAmount);
    }
}