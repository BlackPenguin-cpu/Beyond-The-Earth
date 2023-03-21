using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MergeSort : MonoBehaviour
{
    private int[] MergeSortFunc(int[] arr)
    {
        int[] targetArray = new int[arr.Length];

        int[] array1 = new int[arr.Length / 2];
        int[] array2 = new int[arr.Length / 2 + arr.Length % 2];
        int point1 = 0, point2 = 0;

        while (array1.Length > point1 && array2.Length > point2)
        {
            if (array1[point1] > array2[point2])
            {
                targetArray[targetArray.Length - 1] = array2[point2];
                point2++;
            }
            else
            {
                targetArray[targetArray.Length - 1] = array1[point1];
                point1++;
            }
        }

        return MergeSortFunc(targetArray);
    }
}
