using System;
using UnityEngine;

public class GameCam : MonoBehaviour
{   
    [SerializeField]
    private Vector3 _maxRotation;

    [SerializeField]
    private float _shakeSpeed = 1.0f;
    private Vector3 _seed;
    private float _shakeTime;
    private Quaternion _defaultRotation;

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

    private void OnTraumaEnd(float t)
    {
        transform.rotation = _defaultRotation;
    }

    private Vector3 GetSeed3()
    {
        return new Vector3(UnityEngine.Random.value - 0.5f, UnityEngine.Random.value - 0.5f, UnityEngine.Random.value - 0.5f) * 2.0f;
    }
}
