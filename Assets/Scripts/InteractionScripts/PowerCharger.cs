using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerCharger : PlayerActionPoint {

    public float maxPower;
    bool _isActive;

    [SerializeField]
    private GameObject _objToSpawn;

    [SerializeField]
    private GameObject _objectToScale;
    [SerializeField]
    private float _feedbackScale = 1.5f;

    private Vector3 _objToScaleDefault;

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
        return playerController.Input.IsPowerClicked();
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
        //Debug.Log("Reaction: " + reactionRating);
        playerController.PowerGain(maxPower * reactionRating);
        _isActive = true;

        if (_objToSpawn)
            Instantiate(_objToSpawn, transform.position, Quaternion.identity);

        Destroy(gameObject);
    }

    public override void OnFailedAction(PlayerController player)
    {
        
    }
}
