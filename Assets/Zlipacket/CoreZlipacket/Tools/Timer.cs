using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

namespace Zlipacket.CoreZlipacket.Tools
{
    public class Timer : MonoBehaviour
    {
        private Coroutine co_Timing = null;
        public float duration { get; private set; } = 0f;
        public float elapsedTime { get; private set; } = 0f;

        private List<TimeEvent> events = new();
        public UnityEvent onFinished, onStart, onStop, onReset, onPaused, onUnPaused;
        public UnityEvent<float> onTimerUpdate;
        
        public bool isRunning => co_Timing != null;
        public bool isPause = false;
        
        public static Timer CreateTimer(GameObject owner, float duration = 1f, string timerName = "Timer")
        {
            GameObject timerObject = new GameObject(timerName);
            Timer timer =  timerObject.AddComponent<Timer>();
            
            timerObject.transform.SetParent(owner.transform);
            timerObject.transform.localPosition = Vector3.zero;
            
            timer.SetDuration(duration);
            
            return timer;
        }

        public void SetDuration(float duration)
        {
            this.duration = duration;
        }
        
        public Coroutine StartTimer(float duration = 0f)
        {
            if (isRunning)
            {
                StopCoroutine(co_Timing);
                StopTimer();
            }
            
            this.duration = duration;
            
            onStart?.Invoke();
            co_Timing = StartCoroutine(TimerRunning());
            return co_Timing;
        }
        
        public Coroutine RestartTimer()
        {
            if (isRunning)
            {
                StopCoroutine(co_Timing);
            }
            
            elapsedTime = 0f;
            ResetEvents();
            
            onReset?.Invoke();
            co_Timing = StartCoroutine(TimerRunning());
            return co_Timing;
        }
        
        private IEnumerator TimerRunning()
        {
            while (elapsedTime < duration)
            {
                yield return null;

                if (isPause)
                    continue;
                
                for (int i = 0; i < events.Count; i++)
                {
                    if (events[i].isActivated)
                        continue;

                    if (events[i].time <= elapsedTime)
                    {
                        events[i].Invoke();
                        break;
                    }
                }
                
                elapsedTime += Time.deltaTime;
                onTimerUpdate?.Invoke(GetPercentage());
            }
            elapsedTime = duration;
            
            onFinished?.Invoke();
        }
        
        public void AddEvent(float time, Action timeEvent, bool isActivated = false)
        {
            AddEvent(new TimeEvent(time, timeEvent, isActivated));
        }
        
        public void AddEvent(TimeEvent timeEvent)
        {
            events.Add(timeEvent);
            SortEvents();
        }
        
        private void SortEvents()
        {
            //Sort by Descending Order.
            events.Sort((x, y) => y.time.CompareTo(x.time));
        }

        private void ResetEvents()
        {
            foreach (var e in events)
            {
                e.isActivated = false;
            }
        }

        public void Pause()
        {
            isPause = true;
            onPaused?.Invoke();
        }

        public void UnPause()
        {
            isPause = false;
            onUnPaused?.Invoke();
        }
        
        public void StopTimer()
        {
            if (isRunning)
                StopCoroutine(co_Timing);
            
            ResetEvents();
            onStop?.Invoke();
            elapsedTime = 0f;
            duration = 0f;
            co_Timing = null;
        }
        
        public float GetPercentage() => Mathf.Clamp(elapsedTime / duration, 0f, 1f);
    }
    
    public class TimeEvent
    {
        public float time;
        private Action timeEvent;
        public bool isActivated;
        
        public TimeEvent(float time, Action timeEvent, bool isActivated = false)
        {
            this.time = time;
            this.timeEvent = timeEvent;
            this.isActivated = isActivated;
        }
        
        public void Invoke()
        {
            isActivated = true;
            timeEvent?.Invoke();
        }
    }
}