using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace Characters.Customer
{
    public class ObjectTimer : MonoBehaviour
    {
        [SerializeField] private float duration;
        private float timeElapsed = 0f;
        private bool isRunning = false;

        [Header("UI")]
        [SerializeField] private GameObject timerBar;
        private Image uiFill;
        
        [Header("Events")]
        public UnityEvent onTimerStart;
        public UnityEvent onTimerFinished;
        public UnityEvent onTimerPaused;
        public UnityEvent onTimerReset;

        private void Start()
        {
            uiFill = timerBar.GetComponent<Image>();
        }

        public void SetDuration(float duration)
        {
            this.duration = duration;
        }
        
        public void StartTimer()
        {
            onTimerStart?.Invoke();
            isRunning = true;
        }

        public void PauseTimer()
        {
            onTimerPaused?.Invoke();
            isRunning = false;
        }

        public void ResetTimer()
        {
            onTimerReset?.Invoke();
            timeElapsed = 0f;
            isRunning = false;
            uiFill.fillAmount = 1;
        }
        
        private void Update()
        {
            if (!isRunning) return;
            
            timeElapsed += Time.deltaTime;
            
            uiFill.fillAmount = Mathf.InverseLerp(0, duration, duration - timeElapsed);

            if (timeElapsed >= duration)
            {
                TimerFinished();
                isRunning = false;
            }
        }

        private void TimerFinished()
        {
            onTimerFinished?.Invoke();
        }
    }
}