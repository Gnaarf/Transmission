using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScaleAnim : MonoBehaviour
{
    [SerializeField]
    private AnimationCurve _curve;

    [SerializeField]
    private float _speed = 1.0f;

    private float _time;
	
	// Update is called once per frame
	void Update ()
    {
        _time += Time.deltaTime;

        float scale = _curve.Evaluate(_time * _speed);
        transform.localScale = Vector3.one * scale;	
	}
}
