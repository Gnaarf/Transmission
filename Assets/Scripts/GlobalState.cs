using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class GlobalState : MonoBehaviour
{
    [System.Serializable]
    public class Event : UnityEvent<float> { }

    private static GlobalState _instance;
    public static GlobalState Instance { get { return _instance; } }

    [SerializeField, Range(0.0f, 1.0f)]
    private float _trauma;
    public float Trauma { get { return _trauma; } }

    [SerializeField]
    private int _traumaPower;
    public float GetTraumaPow()
    {
        return Mathf.Pow(_trauma, _traumaPower);
    }

    [SerializeField]
    private float _traumaDampen;

    [HideInInspector]
    public Event _onTraumaStarted;
    [HideInInspector]
    public Event _onTraumaUpdate;
    [HideInInspector]
    public Event _onTraumaEnd;

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


        if (Input.GetKeyDown(KeyCode.Space))
        {
            AddTrauma(0.5f);
        }
    }
}
