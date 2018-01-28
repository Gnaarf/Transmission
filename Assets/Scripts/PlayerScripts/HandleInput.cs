﻿using Rewired;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandleInput : MonoBehaviour
{   
    private Player _player;

    [SerializeField]
    private float _slidingThreshold = 0.5f;

    [SerializeField]
    private float _vibrateFactor = 2.0f;

    private void Start ()
    {
        _player = ReInput.players.GetPlayer("Default");

        if(GlobalState.Instance)
        {
            GlobalState.Instance._onTraumaUpdate.AddListener(OnTraumaUpdate);
            GlobalState.Instance._onTraumaEnd.AddListener(OnTraumaEnd);
        }
	}

    private void OnTraumaEnd(float t)
    {
        Vibrate(0.0f, 0.5f);
    }

    private void OnTraumaUpdate(float t)
    {
        float shake = GlobalState.Instance.GetTraumaPow() * _vibrateFactor;
        Vibrate(shake, 0.5f);
    }

    public bool IsLeftSliding()
    {
        return GetSlidingValue() < 0.0f;
    }

    public bool IsRightSliding()
    {
        return GetSlidingValue() > 0.0f;
    }

    public bool IsSliding()
    {
        return GetSlidingValue() != 0.0f;
    }

    /// <summary> -1 -> left, 1 -> right, 0 -> none </summary>
    public float GetSlidingValue()
    {
        float var = _player.GetAxis("MoveX");

        if(Mathf.Abs(var) > _slidingThreshold)
            return Mathf.Sign(var);

        return 0.0f;
    }

    public float GetX()
    {
        return _player.GetAxis("MoveX");
    }

    public float GetY()
    {
        return _player.GetAxis("MoveY");
    }

    public Vector2 GetMovement()
    {
        return new Vector2(GetX(), GetY());
    }

    public bool AnyKey()
    {
        return _player.GetAnyButtonDown();
    }

    //public bool IsPowerClicked()
    //{
    //    return _player.GetButtonDown("Power");
    //}

    //public bool IsPowerPressed()
    //{
    //    return _player.GetButton("Power");
    //}

    //public bool IsPowerReleased()
    //{
    //    return _player.GetButtonUp("Power");
    //}

    public bool IsAbsorbReleased(float time)
    {
        return _player.GetButtonTimedPressUp("Absorb", time);
    }

    public bool IsAbsorbPressed(float time)
    {
        return _player.GetButtonTimedPress("Absorb", time);
    }

    public bool IsAbsorbClicked()
    {
        return _player.GetButtonDown("Absorb");
    }

    public bool IsAbsorbPressed()
    {
        return _player.GetButton("Absorb");
    }

    public bool IsAbsorbReleased()
    {
        return _player.GetButtonUp("Absorb");
    }

    public bool IsStartClicked()
    {
        return _player.GetButtonDown("Start");
    }

    public bool IsStartPressed()
    {
        return _player.GetButton("Start");
    }

    public bool IsStartReleased()
    {
        return _player.GetButtonUp("Start");
    }

    public void Vibrate(float power, float duration)
    {
        foreach(var v in _player.controllers.Joysticks)
        {
            if (v.supportsVibration)
            {
                for (int i = 0; i < v.vibrationMotorCount; i++)
                    _player.SetVibration(i, power, duration);
                
            }
                

        }

    }
}
