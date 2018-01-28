using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IWeightable
{   
    float GetWeight();
    void SetChance(float chance);

    void SetAccumChance(float chance);
    float GetAccumChance();
}
