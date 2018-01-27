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
    private Renderer _renderer;

    private Material _mat;
    private Vector2 _offset;

    private void Start()
    {
        _mat = _renderer.material;
        _offset = _mat.mainTextureOffset;
        _mat.mainTextureScale = _tiling;
    }

    // Update is called once per frame
    private void Update ()
    {
        _offset += _speed * Time.deltaTime;

        _mat.mainTextureOffset = _offset;
	}
}
