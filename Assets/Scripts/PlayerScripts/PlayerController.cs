using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour {

    [Header("Speed")]
    [SerializeField]
    private float minSpeed = 4.0f;
    [SerializeField]
    private float maxSpeed = 7.5f;
    [SerializeField]
    private float deltaSpeed = 0.5f;

    [Header("Power")]
    [SerializeField]
    private float startPower;


    [SerializeField]
    private float activePowerDrain;
    [SerializeField]
    private float passivePowerDrain;
    
    [HideInInspector]
    private HandleInput handleInput;
    public HandleInput Input { get { return handleInput; } }


    Transition activeTransition;
    WallSegment activeSegment;

    [SerializeField]
    private PlayerOrb _playerOrb;

    [SerializeField]
    private SphereCollider _myCollider;

    [Header("Debug")]
    [SerializeField, ReadOnly]
    private float curPower;
    [SerializeField, ReadOnly]
    private float curSpeed;
    public float GetSpeedPercent()
    {
        return (curSpeed - minSpeed) / (maxSpeed - minSpeed);
    }


    [SerializeField, ReadOnly]
    private bool hasControl;
    public bool HasControl { get { return hasControl; } }

    public float ColliderRadius { get { return _myCollider.radius * _myCollider.transform.lossyScale.z; } }

    // Use this for initialization
    void Start ()
    {
        _playerOrb.WindZoneEnabled = false;

        curSpeed = minSpeed;
        curPower = startPower;

        hasControl = true;
        handleInput = GetComponent<HandleInput>();
    }
	
	// Update is called once per frame
	void Update ()
    {
        curPower -= passivePowerDrain;

        float speedPercent = GetSpeedPercent();


        GlobalState.Instance.SetCurSpeedPercent(speedPercent);
        _playerOrb.SetSparkPercent(speedPercent);
        float yInput = handleInput.GetY();

        //pressing nothing, slows down aswell, press backwards to slow down faster, press forward to get faster
        if(yInput <= 0.0f)
            yInput -= 1.0f;

        curSpeed = Mathf.Clamp(curSpeed + yInput * deltaSpeed * Time.deltaTime, minSpeed, maxSpeed);

        //TODO: Dont call here, call when the charge thing is activated:
        if(handleInput.IsPowerClicked())
        {
            _playerOrb.PlayExplode();
        }

        else if (handleInput.IsPowerPressed())
        {
            curPower -= activePowerDrain * Time.deltaTime;
        }


        if (handleInput.IsAbsorbClicked())
        {
            _playerOrb.WindZoneEnabled = true;
            _playerOrb.AbsorbEffectEnabled = true;
        }

        else if (handleInput.IsAbsorbReleased())
        {
            _playerOrb.WindZoneEnabled = false;
            _playerOrb.AbsorbEffectEnabled = false;
        }

        if (hasControl == true)
        {
            float xInput = handleInput.GetX();

            //TODO: fix that the direction does not change IMMEDIATLE when pressing opposite direction:
           
            _playerOrb.SetSparkRotation(xInput * 45.0f + 180.0f);

            //GlobalState.Instance.SetMinTrauma();

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

    public void DrainPower(float externalPowerDrain)
    {
        curPower -= externalPowerDrain;
    }

    public void PowerGain(float externalPowerGain)
    {
        curPower += externalPowerGain;
    }
}
