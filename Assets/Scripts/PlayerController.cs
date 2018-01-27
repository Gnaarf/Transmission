using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {

    public float speed;
    public float maxSpeed;
    public float minSpeed;

    public float deltaSpeed;
    [HideInInspector]
    public float curSpeed;
    
    [HideInInspector]
    public HandleInput handleInput;

    Transition activeTransition;
    [HideInInspector]
    public bool hasControl;

    // Use this for initialization
    void Start () {
        curSpeed = speed;
        hasControl = true;
        handleInput = GetComponent<HandleInput>();
        
    }
	
	// Update is called once per frame
	void Update () {
        
        if (Input.GetAxis("Vertical") > 0)
        {
            curSpeed = (curSpeed + deltaSpeed > maxSpeed)? maxSpeed : curSpeed + deltaSpeed;
            
        }
        else if(Input.GetAxis("Vertical") < 0)
        {
            curSpeed = (curSpeed - deltaSpeed < minSpeed) ? minSpeed: curSpeed - deltaSpeed;
        }

        if (hasControl == true)
        {
            transform.position += new Vector3(0, 0, curSpeed);
        }
        else
        {
            if(activeTransition != null)
            {
                Vector3 moveVector = (activeTransition.endPoint.transform.position - transform.position);
                float distanceToEndpoint = moveVector.magnitude;
                if (distanceToEndpoint > curSpeed)
                {
                    transform.position += moveVector.normalized * curSpeed;
                }
                else
                {
                    transform.position = activeTransition.endPoint.transform.position;
                    activeTransition = null;
                    hasControl = true;
                    
                    float rmainingSpeed = curSpeed - distanceToEndpoint;
                    transform.position += new Vector3(0, 0, curSpeed);
                }
            }
        }
	}

    public void TakeControl(Transition transition, float reactionRating)
    {
        activeTransition = transition;
        hasControl = false;
    }
}
