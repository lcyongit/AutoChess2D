using System.Collections.Generic;
using UnityEngine;

public class CustomUtil
{
    public static int RandomEnum(List<int> randomList)
    {
        return randomList[Random.Range(0, randomList.Count)];
    }
}
