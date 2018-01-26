using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class Transition : MonoBehaviour {

    public GameObject startPoint;
    public GameObject endPoint;

    [HideInInspector]
    public LineRenderer line;


    // Use this for initialization
	void Start () {
        line = gameObject.GetComponent<LineRenderer>();
        setLineToKeypoints();

	}
	
	// Update is called once per frame
	void Update () {
#if(UNITY_EDITOR)
        Debug.Log("Drop the beat!");
        setLineToKeypoints();
#endif

    }

    void setLineToKeypoints()
    {
        line.positionCount = 2;
        line.SetPosition(0, startPoint.transform.position);
        line.SetPosition(1, endPoint.transform.position);
    }
}
