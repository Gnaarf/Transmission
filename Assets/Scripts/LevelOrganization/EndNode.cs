using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndNode : MonoBehaviour {

    public List<StartNode> outgoingNodes;
    public bool isLastNode;

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
        foreach (StartNode node in outgoingNodes)
        {
            Gizmos.color = Color.green;
            if(node != null) Gizmos.DrawLine(transform.position, node.transform.position);
        }
    }
}
