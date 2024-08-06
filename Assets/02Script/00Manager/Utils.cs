using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Utils
{
    public static T WeightedRandomPick<T>(List<(T, float)> weights)
    {
        float totalWeight = 0f;
        foreach (var weight in weights)
        {
            totalWeight += weight.Item2;
        }

        float pick = UnityEngine.Random.Range(0f, 1f);
        for (int i = 0; i < weights.Count; i++)
        {
            if (weights[i].Item2 / totalWeight >= pick)
            {
                return weights[i].Item1;
            }
            pick -= weights[i].Item2 / totalWeight;
        }

        return weights[weights.Count - 1].Item1;
    }

    public static float ScissorRockPaper(Elements targetElement, Elements attackerElement)
    {
        if (targetElement == Elements.NONE || attackerElement == Elements.NONE)
            return 1f;

        var diff = attackerElement - targetElement;
        
        switch (diff)
        {
            case 1:
            case -2:
                return 1.25f;
            case -1:
            case 2:
                return 0.75f;
        }

        return 1f;
    }
}
