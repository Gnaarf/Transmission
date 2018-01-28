using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    [SerializeField]
    private SegmentStart _startSegment;
    [SerializeField]
    private SegmentStart _interSegmentPrefab;
    [SerializeField]
    private SegmentStart _endSegment;
    [SerializeField]
    private List<SegmentStruct> _segmentPrefabs;
    
    [SerializeField]
    private string _seed;

    [SerializeField]
    private int _numStartSegments;

    [SerializeField]
    private int _levelLength = 100;

    private MyRandom _random;
    private Queue<SegmentStart> _generatedSegments;
    private SegmentStart _mostFrontSegment;
    
    [Header("Debug")]
    [SerializeField, ReadOnly]
    private GameCam _camera;
    [SerializeField, ReadOnly]
    private int _lastSegmentZ;

    [SerializeField]
    private Material _laneMaterial;
    [SerializeField]
    private Material _harmfulLaneMaterial;

    private int _curLevelCount = 0;
    private bool _endSpawned = false;

    private void OnValidate()
    {
        SortWeight.CalcWeights(_segmentPrefabs);
    }
    
    // Use this for initialization
    private void Awake()
    {
        if(Application.isEditor)
        {
            _seed = GenerateRandomString(8);
        }

        _generatedSegments = new Queue<SegmentStart>();
        _random = new MyRandom(_seed.GetHashCode());

        _camera = FindObjectOfType<GameCam>();
        Debug.Assert(_camera);

        GenerateSegment(_startSegment, 2, false);
        GenerateSegments(_numStartSegments);
    }

    /// <summary> Generates the given prefab "num" times. </summary>
    private void GenerateSegment(SegmentStart segmentPrefab, int num, bool generatePreSegment, bool isInterSegment = false)
    {
        for (int i = 0; i < num; i++)
        {
            if (generatePreSegment)
                GenerateSegment(_interSegmentPrefab, 1, false, true);

            var instance = Instantiate(segmentPrefab, transform);

            instance.IsInterSegment = isInterSegment;

            float startZ = 0.0f;

            int segmentLength = 0;

            if (_mostFrontSegment)
            {
                startZ = _mostFrontSegment.transform.position.z;
                segmentLength = _mostFrontSegment.SegmentLength;
            }

            instance.transform.localPosition = new Vector3(0.0f, 0.0f, startZ + segmentLength);
            instance.GenerateConnections(_harmfulLaneMaterial, _laneMaterial);

            _generatedSegments.Enqueue(instance);
            _mostFrontSegment = instance;
        }
    }

    /// <summary> Generates "num" many random segments. </summary>
    private void GenerateSegments(int num)
    {   
        for (int i = 0; i < num; i++)
        {
            GenerateSegment(GetRandSegmentExcept(_mostFrontSegment), 1, true);
        }
    }

    private SegmentStart GetRandSegmentExcept(SegmentStart excluded)
    {
        var element = SortWeight.GetRandWeighted(_segmentPrefabs, _random.FloatValue, (x => x.Prefab != excluded));
        return element.Prefab;
    }

    private void Update()
    {
        if (_endSpawned)
            return;

        int curSegment = (int)_camera.transform.position.z;

        if(curSegment != _lastSegmentZ)
        {
            _lastSegmentZ = curSegment;

            var lastSegment = _generatedSegments.Peek();

            if (_lastSegmentZ > (int)lastSegment.transform.position.z + lastSegment.SegmentLength)
            {
                var nextToDestroy = _generatedSegments.Dequeue();

                if (_curLevelCount >= _levelLength)
                {
                    //TODO: spawn end:
                    _endSpawned = true;
                    GenerateSegment(_endSegment, 1, true, false);
                }

                else
                {
                    //Only generate new segments, if the last one was NOT an InterSegment (only generate for each NEW REAL segment):
                    if (!nextToDestroy.IsInterSegment)
                    {
                        GenerateSegments(1);
                        _curLevelCount++;
                    }

                    Destroy(nextToDestroy.gameObject);
                }
            }
        }

    }

    /// <summary> Generates a string of length 'size' consisting only of lowercase letters and numbers. </summary>
    public static string GenerateRandomString(int size)
    {
        string seedChars = "abcdefghijklmnopqrstuvwxyz0123456789";
        var stringChars = new char[size];

        for (int i = 0; i < stringChars.Length; i++)
            stringChars[i] = seedChars[UnityEngine.Random.Range(0, seedChars.Length)];

        return new string(stringChars);
    }

    [System.Serializable]
    private class SegmentStruct : IWeightable
    {
        [SerializeField]
        private SegmentStart _prefab;
        public SegmentStart Prefab { get { return _prefab; } }
    
        [SerializeField]
        private float _weight = 1.0f;
        [SerializeField, ReadOnly]
        private float _accumChance = 1.0f;
        [SerializeField, ReadOnly]
        private float _chance = 1.0f;

        public float GetAccumChance()
        {
            return _accumChance;
        }

        public float GetWeight()
        {
            return _weight;
        }

        public void SetAccumChance(float chance)
        {
            _accumChance = chance;
        }

        public void SetChance(float chance)
        {
            _chance = chance;
        }
    }

}
