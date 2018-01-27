using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartNode : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public Vector3 getPosition()
    {
        return gameObject.transform.position;
    }

    private void OnDrawGizmos()
    {
        Vector3 endPos = Vector3.zero;
        if(transform.parent.GetComponentInChildren<EndNode>() != null) endPos = transform.parent.GetComponentInChildren<EndNode>().transform.position;
        Gizmos.color = Color.red;
        Gizmos.DrawLine(transform.position, endPos);
    }
}
