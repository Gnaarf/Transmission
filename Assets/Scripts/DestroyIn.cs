using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyIn : MonoBehaviour {

    [SerializeField]
    private float _timer;

	// Use this for initialization
	void Start ()
    {
        Destroy(gameObject, _timer);
	}
}
