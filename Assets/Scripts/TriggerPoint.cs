using Rewired;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerPoint : MonoBehaviour {


    // Use this for initialization
    void Start () {

    }

    // Update is called once per frame
    void Update () {
		
	}

    private void OnCollisionStay(Collision collision)
    {
        if (collision.transform.parent.gameObject.CompareTag("Player"))
        {
            PlayerController playerController = collision.gameObject.GetComponentInParent<PlayerController>();
            Transition transition = gameObject.GetComponentInParent<Transition>();

            if (playerController.hasControl == true && transition.isActive == false && ((playerController.handleInput.IsRightSliding() && transition.transitionDirection > 0) || (playerController.handleInput.IsLeftSliding() && transition.transitionDirection < 0) || transition.isTrackEnd == true))
            {
                Debug.Log("Should start Transition now!");
                float reactionRating = Vector3.Distance(playerController.transform.position, this.transform.position);
                GetComponentInParent<Transition>().TakeControlOfPlayer(playerController, reactionRating);
                
            }
        }
    }
}
