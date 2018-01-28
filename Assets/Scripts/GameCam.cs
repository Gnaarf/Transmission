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
    private Quaternion _defaultRotation;

    private float _zOffset;

    private void Start()
    {
        _defaultRotation = transform.rotation;
        _seed = GetSeed3();

        if (GlobalState.Instance)
        {
            GlobalState.Instance._onTraumaStarted.AddListener(OnTraumaStart);
            GlobalState.Instance._onTraumaUpdate.AddListener(OnTraumaUpdate);
            GlobalState.Instance._onTraumaEnd.AddListener(OnTraumaEnd);
        }

        _zOffset = transform.position.z - _target.transform.position.z;
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

        transform.rotation = _defaultRotation * Quaternion.Euler(rotation);
    }

    private void Update()
    {
        float playerSpeedPercent = _target.GetSpeedPercent();
        _camera.fieldOfView = Mathf.Lerp(_minFov, _maxFox, playerSpeedPercent);

        transform.position = new Vector3(0.0f, transform.position.y, _target.transform.position.z + _zOffset);
    }

    private void OnTraumaEnd(float t)
    {
        transform.rotation = _defaultRotation;
    }

    private Vector3 GetSeed3()
    {
        return new Vector3(UnityEngine.Random.value - 0.5f, UnityEngine.Random.value - 0.5f, UnityEngine.Random.value - 0.5f) * 2.0f;
    }
}
