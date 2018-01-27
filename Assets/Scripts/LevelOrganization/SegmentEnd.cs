using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SegmentEnd : MonoBehaviour {

    public EndNode[] EndNodes;
    SegmentStart nextSegment;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    StartNode getNextNode(int endNodeIndex)
    {
        if(nextSegment != null)
        {
            return nextSegment.GetComponent<SegmentStart>().StartNodes[endNodeIndex];
        }
        return null;
    }
}
