using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class Transition : PlayerActionPoint
{

    public GameObject startPoint;
    public GameObject endPoint;

    public bool isTrackEnd = false;

    Collider startCollider;
    bool _isActive;

    [HideInInspector]
    public int transitionDirection;

    // Use this for initialization
	public override void Start ()
    {
        transitionDirection = (int) (endPoint.transform.position.x - startPoint.transform.position.x);

        startCollider = startPoint.GetComponent<Collider>();

        _isActive = false;
	}
	

    public override void EffectGamePlay(PlayerController playerController, float reactionRating)
    {
        Debug.Log("Taking Control from Player");
        _isActive = true;
        playerController.TakeControl(this, reactionRating);
    }

    //void setLineToKeypoints()
    //{
    //    line.positionCount = 2;
    //    line.SetPosition(0, startPoint.transform.position);
    //    line.SetPosition(1, endPoint.transform.position);
    //}

    public override bool CheckPlayerAction(PlayerController playerController)
    {
        return (playerController.Input.IsRightSliding() && this.transitionDirection > 0) || (playerController.Input.IsLeftSliding() && this.transitionDirection < 0) || this.isTrackEnd == true;
    }

    public override bool isActive()
    {
        return _isActive;
    }

    public override void OnFailedAction(PlayerController player)
    {
        
    }
}
