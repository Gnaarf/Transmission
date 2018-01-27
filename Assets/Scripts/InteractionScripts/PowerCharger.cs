using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerCharger : PlayerActionPoint {

    public float maxPower;
    bool _isActive;

    public override void Update()
    {

    }

    public override void Start()
    {
        _isActive = false;
    }

    public override bool CheckPlayerAction(PlayerController playerController)
    {
        // TODO: change to absorb!!!
        return playerController.handleInput.IsAbsorbPressed();
    }

    public override bool isActive()
    {

        return _isActive;
    }

    public override void EffectGamePlay(PlayerController playerController, float reactionRating)
    {
        Debug.Log("Charging now!");
        playerController.PowerGain(maxPower * reactionRating);
        _isActive = true;
    }
}
