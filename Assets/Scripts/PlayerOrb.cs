using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerOrb : MonoBehaviour
{

    [SerializeField]
    private ParticleSystem _sparkParticles;

    [SerializeField]
    private WindZone _windZone;

    private bool _sparksEnabled;
    public bool SparksEnabled { get { return _sparksEnabled; } set { EnableSparks(value); } }

    private bool _windZoneEnabled;
    public bool WindZoneEnabled { get { return _windZoneEnabled; } set { EnableWindZone(value); } }

    private void Awake()
    {
        _sparksEnabled = _sparkParticles.isPlaying;
        _windZoneEnabled = _windZone.gameObject.activeSelf;
    }

    public void ToggleSparks()
    {
        EnableSparks(!SparksEnabled);
    }

    private void EnableWindZone(bool value)
    {
        _windZone.gameObject.SetActive(value);
    }

    private void EnableSparks(bool sparks)
    {
        if (sparks)
            _sparkParticles.Play();

        else
            _sparkParticles.Stop();

        _sparksEnabled = sparks;
    }

    /// <summary> 180 is facing backwards, 0 is facing forward, -90 is left, 90 is right </summary>
    public void SetSparkRotation(float rotationDegree)
    {
        _sparkParticles.transform.rotation = Quaternion.Euler(0.0f, rotationDegree, 0.0f);
    }
}
