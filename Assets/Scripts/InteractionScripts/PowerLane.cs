using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerLane : PlayerActionPoint {

    public float pointsPerSecond = 20;

    public float powerGain;


    private float _totalTimeDraining = 0.0f;
    private float _totalPointsToAdd = 0.0f;

    public override void Update()
    {

    }

    public override void Start()
    {
       
    }

    public override bool CheckPlayerAction(PlayerController playerController)
    {
        bool used = playerController.AbsorbUsed();
        return used;
    }

    public override bool isActive()
    {

        return false;
    }

    public override void EffectGamePlay(PlayerController playerController, float reactionRating)
    {
        _totalTimeDraining += Time.fixedDeltaTime;
        _totalPointsToAdd += _totalTimeDraining * pointsPerSecond;

        //TODO: spawn some text for IMMEDIATE FEEDBACK: like "Draining: XY"

        float amountPerSecond = powerGain * Time.fixedDeltaTime;
        GlobalState.Instance.OnPowerGainUpdate(amountPerSecond);
        playerController.PowerGain(amountPerSecond);
    }

    public override void OnFailedAction(PlayerController player)
    {
        
    }

    public override void OnCollidingExit(PlayerController playerController)
    {
        if(_totalPointsToAdd > 0.0f)
        {       
            GlobalState.Instance.AddPoints(_totalPointsToAdd);
        }
    }
}
