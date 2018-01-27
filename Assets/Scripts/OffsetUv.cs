using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OffsetUv : MonoBehaviour
{
    [SerializeField]
    private Vector2 _tiling;

    [SerializeField]
    private Vector2 _speed;
    [SerializeField]
    private Vector2 _amplitude;
    [SerializeField]
    private Renderer _renderer;

    private Material _mat;
    private Vector2 _offset;
    private float _time;

    private void Start()
    {
        _mat = _renderer.material;
        _offset = _mat.mainTextureOffset;
        _time = Random.Range(0.0f, Mathf.PI * 2.0f);
    }

    // Update is called once per frame
    private void Update ()
    {
        _time += Time.deltaTime;

        Vector2 curSpeed = new Vector2(Mathf.Sin(_time * _speed.x) * _amplitude.x, Mathf.Cos(_time * _speed.y) * _amplitude.y);

        _offset += curSpeed * Time.deltaTime;
        _mat.mainTextureOffset = _offset;
        _mat.mainTextureScale = _tiling;
    }
}
