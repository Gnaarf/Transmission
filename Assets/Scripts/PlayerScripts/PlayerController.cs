using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour {

    public float speed;
    public float maxSpeed;
    public float minSpeed;

    public float deltaSpeed;

    public float startPower;
    [SerializeField]
    float curPower;
    public float activePowerDrain;
    public float passivePowerDrain;


    [HideInInspector]
    public float curSpeed;
    
    [HideInInspector]
    public HandleInput handleInput;

    Transition activeTransition;
    [HideInInspector]
    public bool hasControl;

    [SerializeField]
    private PlayerOrb _playerOrb;

    // Use this for initialization
    void Start ()
    {
        _playerOrb.WindZoneEnabled = false;

        curSpeed = speed;
        curPower = startPower;

        hasControl = true;
        handleInput = GetComponent<HandleInput>();

    }
	
	// Update is called once per frame
	void Update ()
    {
        curPower -= passivePowerDrain;
       
        if (handleInput.GetY() > 0.0f)
        {
            curSpeed = (curSpeed + deltaSpeed > maxSpeed) ? maxSpeed : curSpeed + deltaSpeed;
            
        }
        else if (handleInput.GetY() < 0.0f)
        {
            curSpeed = (curSpeed - deltaSpeed < minSpeed) ? minSpeed: curSpeed - deltaSpeed;
        }

        if(handleInput.IsPowerPressed())
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
        }
	}

    public void TakeControl(Transition transition, float reactionRating)
    {
        activeTransition = transition;
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
