using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour {

    public GameObject Player;

    PlayerController playerController;

    public Vector3 Offset;

    public 

	// Use this for initialization
	void Start () {
        playerController = Player.GetComponent<PlayerController>();
	}
	
	// Update is called once per frame
	void Update () {
        gameObject.transform.position = Player.transform.position + Offset;
	}
}
