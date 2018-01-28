using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour {

    

    [Header("MainSpeed")]
    [SerializeField]
    private float minSpeed = 4.0f;
    [SerializeField]
    private float maxSpeed = 7.5f;
    [SerializeField]
    private float deltaSpeed = 0.5f;

    [Header("BonuSpeed")]
    [SerializeField]
    private float bonusSpeed = 7.5f;
    [SerializeField]
    private float bonusDampen = 7.5f;

    [SerializeField]
    private float _slowDownFactor = 5.0f;

    [HideInInspector]
    private HandleInput handleInput;
    public HandleInput Input { get { return handleInput; } }


    Transition activeTransition;
    WallSegment activeSegment;

    [Header("Other")]
    [SerializeField]
    private PlayerOrb _playerOrb;

    [SerializeField]
    private SphereCollider _myCollider;

    [Header("Debug")]
    [SerializeField, ReadOnly]
    private float slowDown;
    [SerializeField, ReadOnly]
    private float curBonusSpeed;
    [SerializeField, ReadOnly]
    private float selfSpeed;
    [SerializeField, ReadOnly]
    private float curSpeed;
    public float GetSpeedPercent()
    {
        float totalMax = maxSpeed + bonusSpeed;
        return (curSpeed - minSpeed) / (totalMax - minSpeed);
    }

    [SerializeField, ReadOnly]
    private bool hasControl;
    public bool HasControl { get { return hasControl; } }

    public float ColliderRadius { get { return _myCollider.radius * _myCollider.transform.lossyScale.z; } }

    private bool _hasBeenCharged = false;
    private float _numExplodeParticles;
    private float _targetExplodeTrauma;

    private float _curBonusDampen;

    // Use this for initialization
    void Start ()
    {
        _playerOrb.WindZoneEnabled = false;

        curSpeed = minSpeed;

        hasControl = true;
        handleInput = GetComponent<HandleInput>();
    }
	
    public bool IsExploding()
    {
        return !_hasBeenCharged && handleInput.IsAbsorbReleased();
    }

	// Update is called once per frame
	private void Update ()
    {
        if(GlobalState.Instance.Finished)
        {
            if (handleInput.IsAbsorbClicked())
            {
                Time.timeScale = 1.0f;
                SceneManager.LoadScene(SceneManager.GetActiveScene().name);
                return;
            }
        }

        float yInput = handleInput.GetY();

        //pressing nothing, slows down aswell, press backwards to slow down faster, press forward to get faster
        if(yInput <= 0.0f)
            yInput -= 1.0f;

        selfSpeed = Mathf.Clamp(selfSpeed + yInput * deltaSpeed * Time.deltaTime, minSpeed, maxSpeed);

        //Lose 10% per second:
        curBonusSpeed = Mathf.Clamp(curBonusSpeed - bonusDampen * Time.deltaTime, 0.0f, bonusSpeed);

        slowDown = Mathf.Clamp(slowDown + Time.deltaTime * Mathf.Abs(_slowDownFactor), _slowDownFactor, 0.0f);

        curSpeed = Mathf.Max(curBonusSpeed + selfSpeed + slowDown, minSpeed);

        float speedPercent = GetSpeedPercent();

        GlobalState.Instance.SetCurSpeedPercent(speedPercent);
        _playerOrb.SetSparkPercent(speedPercent);

        TryExplode(5, 0.25f);

        if (handleInput.IsAbsorbPressed(0.5f))
        {
            _hasBeenCharged = true;
            _playerOrb.WindZoneEnabled = true;
            _playerOrb.AbsorbEffectEnabled = true;
        }

        else if (handleInput.IsAbsorbReleased())
        {
            _hasBeenCharged = false;
            _playerOrb.WindZoneEnabled = false;
            _playerOrb.AbsorbEffectEnabled = false;
        }
        
        if (hasControl == true)
        {
            float xInput = handleInput.GetX();

            if(Time.timeScale != 0.0f)
            {
                _playerOrb.SetSparkRotation(xInput * 45.0f + 180.0f);
            }

            transform.position += new Vector3(0, 0, curSpeed * Time.deltaTime);
        }
        else
        {
            if(activeTransition != null)
            {
                Vector3 moveVector = (activeTransition.endPoint.transform.position - transform.position);
                float distanceToEndpoint = moveVector.magnitude;

                if (distanceToEndpoint > curSpeed * Time.deltaTime)
                {
                    transform.position += moveVector.normalized * curSpeed * Time.deltaTime;
                }

                else
                {
                    transform.position = activeTransition.endPoint.transform.position;
                    activeTransition = null;
                    hasControl = true;
                    
                    //float rmainingSpeed = curSpeed - distanceToEndpoint;
                    transform.position += new Vector3(0, 0, curSpeed * Time.deltaTime);
                }
            }
            else if(activeSegment != null)
            {
                if (!activeSegment.IsInSegment(transform.position))
                {
                    transform.position += new Vector3(0, 0, curSpeed * Time.deltaTime);
                    if(transform.position.x != activeSegment.transform.position.x)
                    {
                        transform.position += new Vector3(activeSegment.transform.position.x - transform.position.x, 0, 0);
                    }
                    activeSegment = null;
                    hasControl = true;
                }
                else
                {
                    float targetXValue = 0;
                    if (activeSegment.IsCorrectWallSlidePressed(this))
                    {
                        targetXValue = (activeSegment.transform.position.x + activeSegment.TargetOffsetValue((transform.position + Vector3.forward))) - transform.position.x;
                    }
                    else
                    {
                        targetXValue = (activeSegment.transform.position.x - transform.position.x);
                    }

                    Vector3 moveVector = new Vector3(targetXValue, 0, 1);

                    transform.position += moveVector.normalized * curSpeed * Time.deltaTime;
                }
            }
        }

        //if (UnityEngine.Input.GetKeyDown(KeyCode.Space))
        //    curBonusSpeed = bonusSpeed;
	}

    public void SlowDownByWall()
    {
        slowDown = _slowDownFactor;
    }

    private void LateUpdate()
    {
        if(_targetExplodeTrauma > 0.0f)
        {
            Explode();
            _targetExplodeTrauma = 0.0f;
        }
    }

    public void TryExplode(float particles, float trauma)
    {
        if(DidExplode() && trauma >= _targetExplodeTrauma)
        {
            _targetExplodeTrauma = trauma;
            _numExplodeParticles = particles;
        }
    }

    public bool DidExplode()
    {   
        return !AbsorbUsed() && handleInput.IsAbsorbReleased() && !_hasBeenCharged;
    }

    public bool AbsorbUsed()
    {
        return handleInput.IsAbsorbPressed(0.25f);
    }

    private void Explode()
    {   
        _playerOrb.PlayExplode(_numExplodeParticles, _targetExplodeTrauma);
    }

    public void TakeControl(Transition transition, float reactionRating)
    {
        activeTransition = transition;
        hasControl = false;
    }

    public void TakeControl(WallSegment wallsegment, float reatcionRating)
    {
        activeSegment = wallsegment;
        hasControl = false;
    }

    public void EndGame()
    {
        Debug.Log("Restarting Scene");
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
    
    public void PowerGain(float externalPowerGain)
    {
        curBonusSpeed += externalPowerGain;
    }
}
