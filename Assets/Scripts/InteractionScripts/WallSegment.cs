using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallSegment : PlayerActionPoint {

    public int walldirection;
    public float threshold = 1;
    public float targetXOffset = 0.5f;

    BoxCollider box;
    float objectLength;

    public override void Update()
    {

    }

    public override void Start()
    {
        box = this.GetComponentInChildren<BoxCollider>();
        objectLength = box.size.z * box.transform.lossyScale.z;
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
        if ( playerController.HasControl)
            playerController.TakeControl(this, reactionRating);

        if(IsInThreshold(playerController.transform.position))
        {
            if (IsCorrectWallSlidePressed(playerController) == false)
            {
                playerController.SlowDownByWall();
            }
        }
    }

    public override void OnFailedAction(PlayerController player)
    {
        
    }

    bool IsInThreshold(Vector3 position)
    {
        return Mathf.Abs(transform.position.z - position.z) < (objectLength / 2 - threshold);
    }
    
    public bool IsInSegment(Vector3 position)
    {
        return Mathf.Abs(transform.position.z - position.z) < objectLength / 2;
    }

    public bool IsCorrectWallSlidePressed(PlayerController playerController)
    {
        if (walldirection < 0)
            return playerController.Input.IsLeftSliding();
        if (walldirection > 0)
            return playerController.Input.IsRightSliding();
        return false;
    }

    public float DistanceToColliderEndThreshold(Vector3 position)
    {
        return ((transform.position.z + objectLength / 2) - threshold) - position.z;
    }

    public float DistanceFromColliderStartThreshold(Vector3 position)
    {
        return position.z - ((transform.position.z - objectLength / 2) + threshold);
    }

    // Do not look into this!!
    public float TargetOffsetValue(Vector3 position)
    {
        float value = 0;

        if (DistanceFromColliderStartThreshold(position) < 0)
        {
            value = DistanceFromColliderStartThreshold(position) / -threshold;
            if (value > 1)
                value = 0;
            value = 1 - value;
        }
        else if(DistanceToColliderEndThreshold(position) < 0 )
        {
            value = DistanceToColliderEndThreshold(position) / -threshold;
            value = 1 - value;
            if (value < 1)
                value = 0;
        }
        else
        {
            value = 1;
        }

        return walldirection * targetXOffset * value;
    }
}
