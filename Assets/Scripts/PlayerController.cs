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

	// Use this for initialization
	void Start () {
        curSpeed = speed;
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

        transform.position += new Vector3(0, 0, curSpeed);
	}
}
