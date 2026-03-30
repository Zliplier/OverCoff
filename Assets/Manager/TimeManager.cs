using TMPro;
using UnityEngine;
using UnityEngine.Events;
using Zlipacket.CoreZlipacket.Tools;

namespace Manager
{
    public class TimeManager : Singleton<TimeManager>
    {
        public UnityEvent onHourChanged;
        public UnityEvent onMinuteChanged;
        
        [SerializeField] private TextMeshProUGUI timeText;
        
        public int minute { get; private set; } = 0;
        public int hour { get; private set; } = 8;
        public float minuteToRealtime { get; private set; } = 1f;
        public int endDayTime { get; private set; } = 481;

        public bool isRunning = false;

        private void Start()
        {
            onMinuteChanged.AddListener(UpdateText);
            onMinuteChanged.AddListener(CheckLevelConfig);
        }

        public void StartDay()
        {
            InvokeRepeating(nameof(UpdateTimer), 0, minuteToRealtime);
            isRunning = true;
            Debug.Log("Starting Day");
        }

        public void EndDay()
        {
            CancelInvoke(nameof(UpdateTimer));
            isRunning = false;
            Debug.Log("Ending Day");
        }

        public void UpdateTimer()
        {
            minute++;
            onMinuteChanged?.Invoke();

            if (minute >= 60)
            {
                hour++;
                onHourChanged?.Invoke();
                minute = 0;
            }

            if ((hour * 60) + minute - endDayTime >= endDayTime)
            {
                EndDay();
            }
        }

        public void UpdateText()
        {
            timeText.SetText($"{hour.ToString("00")}:{minute.ToString("00")}");
        }

        public void CheckLevelConfig()
        {
            for (int i = GameplayManager.Instance.levelConfig.spawnTime.Count - 1; i >= 0; i--)
            {
                if ((hour * 60) + minute - endDayTime == GameplayManager.Instance.levelConfig.spawnTime[i])
                {
                    int maxIndex = GameplayManager.Instance.levelConfig.customerList.Count;
                    GameplayManager.Instance.SpawnCustomer
                        (GameplayManager.Instance.levelConfig.customerList[Random.Range(0, maxIndex)]);
                    return;
                }
            }
        }
        
        
    }
}