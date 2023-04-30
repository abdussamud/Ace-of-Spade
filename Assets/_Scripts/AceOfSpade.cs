using System.Collections.Generic;
using UnityEngine;

public class AceOfSpade : MonoBehaviour
{
    public static void Shuffle<T>(List<T> list)
    {
        System.Random rand = new();
        int n = list.Count;
        while (n > 1)
        {
            int k = rand.Next(n);
            n--;
            (list[n], list[k]) = (list[k], list[n]);
        }
    }
}
