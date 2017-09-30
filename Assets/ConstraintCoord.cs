using UnityEngine;
using System.Collections;

[System.Serializable]
public struct ConstraintCoord2D
{
    public ConstraintCoord xAxis;
    public ConstraintCoord yAxis;

    public ConstraintCoord2D(float xMin, float xMax, float yMin, float yMax)
    {
        xAxis = new ConstraintCoord(xMin, xMax);
        yAxis = new ConstraintCoord(yMin, yMax);
    }
    public ConstraintCoord2D(ConstraintCoord xAxis, ConstraintCoord yAxis)
    {
        this.xAxis = xAxis;
        this.yAxis = yAxis;
    }
}

[System.Serializable]
public struct ConstraintCoord
{
    public float min;
    public float max;

    public ConstraintCoord(float min, float max)
    {
        this.min = min;
        this.max = max;
    }

    public bool IsZero()
    {
        return min == default(float) && max == default(float);
    }
    /// <summary>
    /// Return clamp value in min & max.
    /// </summary>
    public float Clamp(float position)
    {
        return Mathf.Clamp(position, min, max);
    }
    /// <summary>
    /// Return random value in range (min,max)
    /// </summary>
    public float GetRandom()
    {
        return Random.Range(min, max);
    }
    public bool IsOutOfLimits(float position)
    {
        return position > max || position < min;
    }
    public bool IsOutOfLimits(float position, bool directionToMax)
    {
#if UNITY_EDITOR
        if (max < min)
            Debug.LogError("ConstraintCoord error max < min");
#endif
        if (directionToMax)
        {
            if (position > max)
                return true;
        }
        else
        {
            if (position < min)
                return true;
        }
        return false;
    }

    public static implicit operator Vector2(ConstraintCoord l)
    {
        return new Vector2(l.min, l.max);
    }
    public static implicit operator ConstraintCoord(Vector2 v)
    {
        return new ConstraintCoord(v.x, v.y);
    }
    public static implicit operator ConstraintCoord(Vector3 v)
    {
        return new ConstraintCoord(v.x, v.y);
    }
    public static implicit operator bool(ConstraintCoord limit)
    {
        return !limit.IsZero();
    }
}
