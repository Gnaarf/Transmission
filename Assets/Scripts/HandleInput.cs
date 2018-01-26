using Rewired;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandleInput : MonoBehaviour
{   
    private Player _player;

    [SerializeField]
    private float _slidingThreshold = 0.5f;

    private void Start ()
    {
        _player = ReInput.players.GetPlayer("Default");
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

    public bool IsPowerClicked()
    {
        return _player.GetButtonDown("Power");
    }

    public bool IsPowerPressed()
    {
        return _player.GetButton("Power");
    }

    public bool IsPowerReleased()
    {
        return _player.GetButtonUp("Power");
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

    public void Vibrate()
    {
        
    }
}
