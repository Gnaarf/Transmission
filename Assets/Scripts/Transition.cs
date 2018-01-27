using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class Transition : MonoBehaviour {

    public GameObject startPoint;
    public GameObject endPoint;

    public bool isTrackEnd = false;

    [HideInInspector]
    public LineRenderer line;

    Collider startCollider;
    [HideInInspector]
    public bool isActive;

    [HideInInspector]
    public int transitionDirection;

    // Use this for initialization
	void Start () {
        line = gameObject.GetComponent<LineRenderer>();
        setLineToKeypoints();

        transitionDirection = (int) (endPoint.transform.position.x - startPoint.transform.position.x);

        startCollider = startPoint.GetComponent<Collider>();

        isActive = false;
	}
	
	// Update is called once per frame
	void Update () {
#if(UNITY_EDITOR)
        setLineToKeypoints();
#endif
        
    }

    public void TakeControlOfPlayer(PlayerController playerController, float reactionRating)
    {
        Debug.Log("Taking Control from Player");
        isActive = true;
        playerController.TakeControl(this, reactionRating);
    }

    void setLineToKeypoints()
    {
        line.positionCount = 2;
        line.SetPosition(0, startPoint.transform.position);
        line.SetPosition(1, endPoint.transform.position);
    }
}
