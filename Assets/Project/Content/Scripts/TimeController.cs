using System.Collections;
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
    private Coroutine _coroutineUpdateTimer;

    private void Awake()
    {
        StartTimers();
    }

    private void OnDestroy()
    {
        StopTimers();
    }

    private void StartTimers()
    {
        _coroutineUpdateTimer = StartCoroutine(UpdateTimer());
    }

    private void StopTimers()
    {
        StopCoroutine(_coroutineUpdateTimer);
    }

    private IEnumerator UpdateTimer()
    {
        var wfs = new WaitForSecondsRealtime(1f);

        while (true)
        {
            foreach (var data in _timerData)
                data.Value.GetRemainingTime();

            yield return wfs;
        }
    }


    public TimerData GetTimer(int index)
    {
        if (_timerData.TryGetValue(index, out var data))
            return data;

        data = new TimerData(index);
        _timerData.Add(index, data);
        return data;
    }
}
