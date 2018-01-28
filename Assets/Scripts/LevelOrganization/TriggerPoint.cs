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

    private void OnTriggerEnter(Collider other)
    {
        if (other.transform.parent.gameObject.CompareTag("Player"))
        {
            PlayerController playerController = other.gameObject.GetComponentInParent<PlayerController>();
            playerActionPoint.OnCollidingEnter(playerController);
        }

    }

    private void OnTriggerExit(Collider other)
    {
        if (other.transform.parent.gameObject.CompareTag("Player"))
        {
            PlayerController playerController = other.gameObject.GetComponentInParent<PlayerController>();
            playerActionPoint.OnCollidingExit(playerController);
        }
    }

    private float GetDistanceRating(PlayerController player, Collider obstacleCollider)
    {
        float colliderRadius = 0.0f;

        if (obstacleCollider is SphereCollider)
        {
            SphereCollider col = (SphereCollider)obstacleCollider;
            colliderRadius = col.radius;
        }
        else if (obstacleCollider is BoxCollider)
        {
            BoxCollider col = (BoxCollider)obstacleCollider;
            colliderRadius = col.size.z;
        }
        else
        {
            colliderRadius = 1;
            Debug.Assert(false);
        }

        colliderRadius *= obstacleCollider.transform.lossyScale.z;

        float distToTarget = Vector3.Distance(player.transform.position, this.transform.position);
        float reactionRating = Mathf.Clamp01(distToTarget / (colliderRadius + player.ColliderRadius));

        //invert so that 0 is bad, and 1.0 is good:
        reactionRating = 1.0f - reactionRating;
        return reactionRating;
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.transform.parent.gameObject.CompareTag("Player"))
        {
            PlayerController playerController = other.gameObject.GetComponentInParent<PlayerController>();
            playerActionPoint.OnCollidingStay(playerController);

            if (playerActionPoint.isActive() == false && playerActionPoint.CheckPlayerAction(playerController))
            {
                //Debug.Log("Time to do stuff");

                float reactionRating = GetDistanceRating(playerController, other);

                playerActionPoint.EffectGamePlay(playerController, reactionRating);
            }

            else
            {
                playerActionPoint.OnFailedAction(playerController);
            }
        }
    }
}
