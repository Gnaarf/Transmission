using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UiManager : MonoBehaviour
{
    public static UiManager Instance { get; private set; }

    [SerializeField]
    private Feedback[] _feedbacks;

    [SerializeField]
    private Text _finishText;

    [SerializeField]
    private Text _perfectText;
    [SerializeField]
    private Text _pointsToAddText;
    [SerializeField]
    private AnimationCurve _pointsToAddAlpha;
    [SerializeField]
    private float _addTime = 0.5f;
    private float _curAddTime = 0.01f;

    [SerializeField]
    private Text _drainingPointsText;
    [SerializeField]
    private float _drainDuration = 0.5f;

    [SerializeField]
    private Outline _perfectOutline;
    [SerializeField]
    private Text _pointsText;

    [SerializeField]
    private float _perfectStayDur;
    [SerializeField]
    private AnimationCurve _perfectAlphaCurve;
    [SerializeField]
    private AnimationCurve _perfectScaleCurve;

    [SerializeField, ReadOnly]
    private float _curPerfectDur = 0.015f;

    private float _curDrainTime = 0.01f;

    private void Awake()
    {
        Debug.Assert(Instance == null);
        Instance = this;
    }

    public void OnFinish()
    {
        _finishText.gameObject.SetActive(true);
    }

    public void SetDrainingPoints(string baseString, float totalPointsToAdd)
    {
        _curDrainTime = _drainDuration;
        _drainingPointsText.text = baseString + (int)totalPointsToAdd;
    }

    /// <summary>0: bad, 1 normal, 2 perfect PERFECT</summary>
    public void TriggerPerfect(int level, float points)
    {   
        _curPerfectDur = _perfectStayDur;
        _feedbacks[level].Apply(_perfectText, _perfectOutline);

        int pointsInt = (int)points;

        if(pointsInt > 0)
            SetDrainingPoints("", pointsInt);
    }

    private void Update()
    {
        if (_curPerfectDur > 0.0f)
        {
            _curPerfectDur -= Time.deltaTime;

            float t = Mathf.Clamp01(1.0f - (_curPerfectDur / _perfectStayDur));
            Color color = _perfectText.color;
            color.a = _perfectAlphaCurve.Evaluate(t);


            _perfectText.transform.localScale = Vector3.one * _perfectScaleCurve.Evaluate(t);
            _perfectText.color = color;
        }

        if(_curAddTime > 0.0f)
        {
            _curAddTime -= Time.deltaTime;
            float t = Mathf.Clamp01(1.0f - (_curAddTime / _addTime));

            Color color = _pointsToAddText.color;
            color.a = _pointsToAddAlpha.Evaluate(t);
            _pointsToAddText.color = color;
        }

        if(_curDrainTime > 0.0f)
        {
            _curDrainTime -= Time.deltaTime;

            float t = Mathf.Clamp01(1.0f - (_curDrainTime / _drainDuration));


            _drainingPointsText.transform.localScale = Vector3.one * _perfectScaleCurve.Evaluate(t);

            Color color = _drainingPointsText.color;
            color.a = _pointsToAddAlpha.Evaluate(t);
            _drainingPointsText.color = color;
        }
    }

    [System.Serializable]
    private class Feedback
    {
        public Color color = Color.white;
        public string text = "";    

        public void Apply(Text uiText, Outline outline)
        {
            outline.effectColor = color;
            uiText.text = text;
        }
    }

    public void SetTotalPoints(float totalPoints)
    {
        _pointsText.text = "Score: " + (int)totalPoints;
    }

    /// <summary> Called when pointsToAdd changed from being reduced (added to total score) </summary>
    public void SetPointsToAddFromReduce(float pointsToAdd)
    {
        int num = (int)pointsToAdd;

        _curAddTime = _addTime;
        _pointsToAddText.text = "+" + num;
    }
}
