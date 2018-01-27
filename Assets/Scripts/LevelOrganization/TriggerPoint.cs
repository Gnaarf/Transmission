using Rewired;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerPoint : MonoBehaviour {

    PlayerActionPoint playerActionPoint;

    // Use this for initialization
    void Start ()
    {
        playerActionPoint = GetComponentInParent<PlayerActionPoint>();
    }
    
    private void OnTriggerStay(Collider collider)
    {
        if (collider.transform.parent.gameObject.CompareTag("Player"))
        {
            PlayerController playerController = collider.gameObject.GetComponentInParent<PlayerController>();
            gameObject.GetComponentInParent<PlayerActionPoint>();

            if (playerController.hasControl == true && playerActionPoint.isActive() == false && playerActionPoint.CheckPlayerAction(playerController))
            {
                Debug.Log("Time to do stuff");

                float radius;
                if(collider.GetType() == typeof(SphereCollider))
                {
                    SphereCollider col = (SphereCollider)collider;
                    radius = col.radius;
                }
                else if(collider.GetType() == typeof(BoxCollider))
                {
                    BoxCollider col = (BoxCollider)collider;
                    radius = col.size.z;
                }
                else
                {
                    radius = 1;
                }
                float relativeDistance = 1 - Vector3.Distance(playerController.transform.position, this.transform.position) / radius;

                if (relativeDistance < 1)
                    relativeDistance = 0;

                float reactionRating = Vector3.Distance(playerController.transform.position, this.transform.position) * relativeDistance;

                playerActionPoint.EffectGamePlay(playerController, reactionRating);
            }

            else
            {
                playerActionPoint.OnFailedAction(playerController);
            }
        }
    }
}
