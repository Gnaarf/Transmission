using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MyRandom : System.Random
{
    private int _seed;
    public int Seed { get { return _seed; } }

	public MyRandom(int seed)
        : base(seed)
    {
        this._seed = seed;
    }

    public float FloatValue { get { return (float)this.NextDouble(); } }

    public float FloatRange(float min, float max)
    {
        return FloatValue * (max - min) + min;
    }

    public int IntRangeExclusive(int min, int max)
    {
        return Next(max - min) + min;
    }

    public int IntRangeInclusive(int min, int max)
    {
        return Next(max + 1 - min) + min;
    }

    public Vector2 GetRandVec(Vector2 min, Vector2 max)
    {
        float x = FloatRange(min.x, max.x);
        float y = FloatRange(min.y, max.y);
        return new Vector2(x, y);
    }

}
