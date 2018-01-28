using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnObject : MonoBehaviour
{   
    [SerializeField]
    private GameObject _obj;

    // Use this for initialization
    void Start()
    {   
        Instantiate(_obj, transform.position, Quaternion.identity, transform);
    }
}
