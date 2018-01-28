using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SortWeight : MonoBehaviour
{

    public static T GetRandWeighted<T>(List<T> elements, float chance, System.Predicate<T> predicate = null) where T : class, IWeightable
    {
        Debug.Assert(chance >= 0.0f && chance <= 1.0f);

        T defaultResult = null;

        for (int i = 0; i < elements.Count; i++)
        {
            bool matchesPredicate = (predicate == null || predicate(elements[i]));

            //Make sure that we choose at least the set that CAN be choosen for that room, even tough the chance is not valid:
            if (matchesPredicate && defaultResult == null)
                defaultResult = elements[i];

            if (chance <= elements[i].GetAccumChance() && matchesPredicate)
                return elements[i];
        }

        return defaultResult;
    }

    public static void CalcWeights<T>(List<T> elements) where T : class, IWeightable
    {
        if (elements == null || elements.Count <= 0)
            return;

        float totalWeight = 0.0f;

        for (int i = 0; i < elements.Count; i++)
        {
            if (elements[i] == null)
                continue;

            float curWeight = elements[i].GetWeight();
            totalWeight += curWeight;
            elements[i].SetAccumChance(curWeight);
        }

        float prevChance = 0.0f;
        for (int i = 0; i < elements.Count; i++)
        {
            if (elements[i] == null)
                continue;

            float myChance = (elements[i].GetAccumChance() / totalWeight);

            elements[i].SetChance(myChance);
            elements[i].SetAccumChance(myChance + prevChance);
            prevChance = elements[i].GetAccumChance();
        }
    }
}
