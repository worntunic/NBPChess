using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Utils
{
    public static bool IsBetween(this int value, int firstBound, int secondBound, bool inclusive = false)
    {
        int lower, upper;
        if (firstBound > secondBound)
        {
            lower = firstBound;
            upper = secondBound;
        } else
        {
            lower = secondBound;
            upper = firstBound;
        }
        if (inclusive)
        {
            return (value >= lower && value <= upper);
        } else
        {
            return (value > lower && value < upper);
        }
    }
}
