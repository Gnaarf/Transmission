using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class GlobalState : MonoBehaviour
{
    [System.Serializable]
    public class Event : UnityEvent<float> { }

    private bool _finished = false;
    public bool Finished { get { return _finished; } }



    private static GlobalState _instance;
    public static GlobalState Instance { get { return _instance; } }

    [SerializeField, Range(0.0f, 1.0f)]
    private float _trauma;
    public float Trauma { get { return _trauma; } }

    [SerializeField]
    private float _speedTrauma = 0.2f;

    [SerializeField]
    private int _traumaPower;

    [SerializeField]
    private float _traumaDampen;

    [SerializeField]
    private float _powerGainedFactor = 5.0f;

    [SerializeField]
    private float _pointAddTimer = 0.5f;
    [SerializeField]
    private float _pointPerTick = 1.0f;

    [SerializeField]
    private Material _laneMaterial;

    [SerializeField, Range(0.0f, 1.0f)]
    private float _powerGainedMaxTrauma = 0.5f;

    [HideInInspector]
    public Event _onTraumaStarted;
    [HideInInspector]
    public Event _onTraumaUpdate;
    [HideInInspector]
    public Event _onTraumaEnd;

    [SerializeField, ReadOnly]
    private float _curPointAddTimer = 0.0f;
    [SerializeField, ReadOnly]
    private float _pointsToAdd = 0.0f;
    [SerializeField, ReadOnly]
    private float _totalPoints = 0.0f;

    public float GetTraumaPow()
    {
        return Mathf.Pow(_trauma, _traumaPower);
    }

    public void OnPowerGainUpdate(float amountPerSecond)
    {
        if (_trauma < _powerGainedMaxTrauma)
            AddTrauma(amountPerSecond * _powerGainedFactor);
    }

    private void Awake()
    {
        Debug.Assert(_instance == null);
        _instance = this;
    }

    public void AddTrauma(float percent)
    {
        float oldTrauma = _trauma;
        _trauma = Mathf.Clamp(_trauma + percent, 0.0f, 1.0f);

        if (oldTrauma <= 0.0f && percent > 0.0f)
            _onTraumaStarted.Invoke(_trauma);
    }

    public void SetCurSpeedPercent(float speedPercent)
    {
        float targetTrauma = speedPercent * _speedTrauma;

        float delta = targetTrauma - _trauma;

        if(delta > 0.0f)
            AddTrauma(delta);
    }

    // Update is called once per frame
    private void Update()
    {
        if (_trauma > 0.0f)
        {
            _trauma -= _traumaDampen * Time.deltaTime;
            _onTraumaUpdate.Invoke(_trauma);

            if (_trauma <= 0.0f)
            {
                _trauma = 0.0f;
                _onTraumaEnd.Invoke(_trauma);
            }
        }


        if(_pointsToAdd > 0.0f)
        {
            _curPointAddTimer -= Time.deltaTime;


            if (_curPointAddTimer <= 0.0f)
            {
                float pointDelta = _pointPerTick * _pointsToAdd;

                if (pointDelta < 1.0f)
                    pointDelta = _pointsToAdd;

                _pointsToAdd -= pointDelta;
                _totalPoints += pointDelta;

                UiManager.Instance.SetTotalPoints(_totalPoints);
                UiManager.Instance.SetPointsToAddFromReduce(_pointsToAdd);

                _curPointAddTimer = _pointAddTimer;
            }
        }
    }

    public void Finish()
    {
        _finished = true;
        Time.timeScale = 0.0f;
        UiManager.Instance.OnFinish();
    }



    public void AddPoints(float points)
    {
        _curPointAddTimer = _pointAddTimer;
        _pointsToAdd += points;

    }
}
