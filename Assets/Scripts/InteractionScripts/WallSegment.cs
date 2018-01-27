using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallSegment : PlayerActionPoint {

    int walldirection;

    public override void Update()
    {

    }

    public override void Start()
    {
        if (transform.position.x > 0)
            walldirection = 1;
        else if (transform.position.x < 0)
            walldirection = -1;
        else
        {
            walldirection = 0;
            Debug.Log("Do not place a wall in the centerLane!");
        }

        Transform cube = transform.GetComponentInChildren<MeshFilter>().transform;
        cube.rotation = Quaternion.AngleAxis(90 * walldirection, Vector3.forward);

        cube.position += new Vector3(walldirection * 0.5f, 0, cube.position.x);
    }

    public override bool CheckPlayerAction(PlayerController playerController)
    {
        if (walldirection == 1)
            return playerController.handleInput.IsRightSliding() == false;
        if (walldirection == -1)
            return playerController.handleInput.IsLeftSliding() == false;
        return true;
    }

    public override bool isActive()
    {
        return false;
    }

    // TODO: Refactor this stuff!
    public override void EffectGamePlay(PlayerController playerController, float reactionRating)
    {
        playerController.EndGame();
    }
}
