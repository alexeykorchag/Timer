namespace Project
{
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;

    public class TimeController : MonoBehaviour
    {
        //TODO: start and stop the timer only when needed

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
            _coroutineUpdateTimer = StartCoroutine(UpdateRemainingTime());
        }

        private void StopTimers()
        {
            StopCoroutine(_coroutineUpdateTimer);
        }

        private IEnumerator UpdateRemainingTime()
        {
            var wfs = new WaitForSecondsRealtime(1f);

            while (true)
            {
                foreach (var data in _timerData)
                    data.Value.UpdateRemainingTime();

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
}