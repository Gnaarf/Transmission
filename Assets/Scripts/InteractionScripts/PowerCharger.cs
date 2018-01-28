using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerCharger : PlayerActionPoint
{
    public float _minPoints = 10;
    public float _maxPoints = 100;


    public float minPower;
    public float maxPower;
    bool _isActive;

    [SerializeField]
    private ParticleSystem _objToSpawn;

    [SerializeField]
    private GameObject _objectToScale;
    [SerializeField]
    private float _feedbackScale = 1.5f;

    private Vector3 _objToScaleDefault;

    [Header("NewParticles")]
    [SerializeField]
    private float _newMinParticles = 30.0f;
    [SerializeField]
    private float _newMaxParticles = 45.0f;

    [Header("PlayerParticles")]
    [SerializeField]
    private float _minTrauma = 0.3f;
    [SerializeField]
    private float _maxTrauma = 0.6f;
    [SerializeField]
    private float _minParticles = 30.0f;
    [SerializeField]
    private float _maxParticles = 45.0f;

    public override void Update()
    {

    }

    public override void Start()
    {
        _isActive = false;
        _objToScaleDefault = _objectToScale.transform.localScale;
    }

    public override bool CheckPlayerAction(PlayerController playerController)
    {
        return playerController.DidExplode();
    }

    public override bool isActive()
    {   
        return _isActive;
    }

    public override void OnCollidingEnter(PlayerController playerController)
    {
        _objectToScale.transform.localScale = _objToScaleDefault * _feedbackScale;
    }

    public override void OnCollidingExit(PlayerController playerController)
    {
        _objectToScale.transform.localScale = _objToScaleDefault;
    }

    public override void EffectGamePlay(PlayerController playerController, float reactionRating)
    {
        float percent = 0.0f;

        if(reactionRating >= 0.5f)
        {
            percent = 1.0f;
            UiManager.Instance.TriggerPerfect(2);
        }

        else if(reactionRating >= 0.25f)
        {
            percent = 0.5f;
            UiManager.Instance.TriggerPerfect(1);
        }

        else
        {
            percent = 0.0f;
            UiManager.Instance.TriggerPerfect(0);
        }

        float points = Mathf.Lerp(_minPoints, _maxPoints, percent);
        GlobalState.Instance.AddPoints(points);

        playerController.TryExplode(Mathf.Lerp(_minParticles, _maxParticles, reactionRating), Mathf.Lerp(_minTrauma, _maxTrauma, reactionRating));
        
        playerController.PowerGain(Mathf.Lerp(minPower, maxPower, reactionRating));
        _isActive = true;

        if (_objToSpawn)
        {
            var instance = Instantiate(_objToSpawn, transform.position, Quaternion.identity);
            var emission = instance.emission;
            emission.SetBurst(0, new ParticleSystem.Burst(0.0f, Mathf.Lerp(_newMinParticles, _newMaxParticles, reactionRating)));
        }

        Destroy(gameObject);
    }

    public override void OnFailedAction(PlayerController player)
    {
        
    }
}
