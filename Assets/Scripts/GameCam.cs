using System;
using UnityEngine;

public class GameCam : MonoBehaviour
{
    [SerializeField]
    private float _minFov = 55.0f;
    [SerializeField]
    private float _maxFox = 90.0f;

    [SerializeField]
    private Camera _camera;

    [SerializeField]
    private Vector3 _maxRotation;
    [SerializeField]
    private PlayerController _target;

    [SerializeField]
    private float _shakeSpeed = 1.0f;
    private Vector3 _seed;
    private float _shakeTime;

    [SerializeField]
    private Transform _minSpeedTrans;
    [SerializeField]
    private Transform _maxSpeedTrans;
    [SerializeField]
    private float _changeFactor = 10.0f;


    private float _curSpeedPercent;

    private void Start()
    {
        _seed = GetSeed3();

        if (GlobalState.Instance)
        {
            GlobalState.Instance._onTraumaStarted.AddListener(OnTraumaStart);
            GlobalState.Instance._onTraumaUpdate.AddListener(OnTraumaUpdate);
            GlobalState.Instance._onTraumaEnd.AddListener(OnTraumaEnd);
        }
    }

    private void OnTraumaStart(float t)
    {
        _shakeTime = 0.0f;
        _seed = GetSeed3();
    }

    private void OnTraumaUpdate(float t)
    {
        float shake = GlobalState.Instance.GetTraumaPow();
        _shakeTime += Time.deltaTime * _shakeSpeed;

        Vector3 rotation = Vector3.zero;

        rotation.x = _maxRotation.x * shake * (Mathf.PerlinNoise(_shakeTime + _seed.x, _seed.x * 10.0f) - 0.5f) * 2.0f;
        rotation.y = _maxRotation.y * shake * (Mathf.PerlinNoise(_shakeTime + _seed.y, _seed.y * 10.0f) - 0.5f) * 2.0f;
        rotation.z = _maxRotation.z * shake * (Mathf.PerlinNoise(_shakeTime + _seed.z, _seed.z * 10.0f) - 0.5f) * 2.0f;

        transform.rotation = GetCurRotation() * Quaternion.Euler(rotation);
    }

    private Quaternion GetCurRotation()
    {
        return Quaternion.Lerp(_minSpeedTrans.rotation, _maxSpeedTrans.rotation, _curSpeedPercent);
    }

    private Vector3 GetLocalOffset()
    {
        return Vector3.Lerp(_minSpeedTrans.position, _maxSpeedTrans.position, _curSpeedPercent);
    }

    private void Update()
    {
        _curSpeedPercent = Mathf.Lerp(_curSpeedPercent, _target.GetSpeedPercent(), Time.deltaTime * _changeFactor);
        _camera.fieldOfView = Mathf.Lerp(_minFov, _maxFox, _curSpeedPercent);

        var offset = GetLocalOffset();
        transform.position = new Vector3(offset.x, offset.y, _target.transform.position.z + offset.z);

     
    }

    private void OnTraumaEnd(float t)
    {
        transform.rotation = GetCurRotation();
    }

    private Vector3 GetSeed3()
    {
        return new Vector3(UnityEngine.Random.value - 0.5f, UnityEngine.Random.value - 0.5f, UnityEngine.Random.value - 0.5f) * 2.0f;
    }
}
