using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerLane : PlayerActionPoint {

    public float powerGain;

    public override void Update()
    {

    }

    public override void Start()
    {
       
    }

    public override bool CheckPlayerAction(PlayerController playerController)
    {
        // TODO: refactor this
        return playerController.handleInput.IsAbsorbPressed();
    }

    public override bool isActive()
    {

        return false;
    }

    // TODO: Refactor this stuff!
    public override void EffectGamePlay(PlayerController playerController, float reactionRating)
    {
        Debug.Log("Charging now!");
        playerController.PowerGain(powerGain);
    }
}
