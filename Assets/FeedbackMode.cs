using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.PostProcessing;

public class FeedbackMode : MonoBehaviour
{
    public static FeedbackMode Instance;

    [SerializeField]
    private List<PostProcessingBehaviour> _postBehaviors;
    private bool _postEnabled = true;

    [SerializeField]
    private List<GameObject> _particleObjects;
    public bool _particlesEnabled = true;

    public bool camShakes = true;

    public bool camMovement = true;

    private void Awake()
    {
        Instance = this;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            _postEnabled = !_postEnabled;

            for (int i = 0; i < _postBehaviors.Count; i++)
                _postBehaviors[i].enabled = _postEnabled;
        }

        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            _particlesEnabled = !_particlesEnabled;

            for (int i = 0; i < _particleObjects.Count; i++)
                _particleObjects[i].SetActive(_particlesEnabled);
        }

        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            camShakes = !camShakes;
        }

        if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            camMovement = !camMovement;
        }
    }

}
