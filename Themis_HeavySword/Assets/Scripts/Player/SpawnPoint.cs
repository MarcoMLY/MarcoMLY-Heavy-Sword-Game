using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnPoint : MonoBehaviour
{
    [SerializeField] private DayDataHolder _dayData;

    private void Start()
    {
        transform.position = new Vector3(_dayData.Day.StartPosition[0], _dayData.Day.StartPosition[1], _dayData.Day.StartPosition[2]);
    }
}
