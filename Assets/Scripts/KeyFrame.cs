using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class KeyFrame
{
    public int Value;
    public float Time;

    public KeyFrame() { }

    public KeyFrame(int value, float time)
    {
        Value = value;
        Time = time;
    }
}

