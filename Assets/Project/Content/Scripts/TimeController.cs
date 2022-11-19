using System.Collections.Generic;
using UnityEngine;

public class TimeController : MonoBehaviour
{
    private static TimeController _instance;

    public static TimeController Inctance
    {
        get
        {
            if (_instance == null)
                _instance = FindObjectOfType<TimeController>();
            return _instance;
        }
    }



    private Dictionary<int, TimerData> _timerData = new Dictionary<int, TimerData>();

    public TimerData GetTimer(int index)
    {
        if (_timerData.TryGetValue(index, out var data))
            return data;

        data = new TimerData(index);
        _timerData.Add(index, data);
        return data;
    }
}
