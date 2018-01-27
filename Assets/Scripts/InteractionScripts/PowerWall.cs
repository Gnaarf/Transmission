using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerWall : PlayerActionPoint {

    public float powerDrain;

    public override void Update()
    {

    }

    public override void Start()
    {

    }

    public override bool CheckPlayerAction(PlayerController playerController)
    {
        return true;
    }

    public override bool isActive()
    {

        return false;
    }

    public override void EffectGamePlay(PlayerController playerController, float reactionRating)
    {
        if (playerController.handleInput.IsPowerPressed() == false)
        {
            playerController.DrainPower(powerDrain);
        }
    }
}
