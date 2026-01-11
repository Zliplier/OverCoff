using System.Collections.Generic;
using Items.Data;
using Players;
using UI;
using UI.Display;
using UnityEngine;
using UnityEngine.Events;
using Zlipacket.CoreZlipacket.Misc;
using Zlipacket.CoreZlipacket.Tools;

namespace Items.Script.Ingredients
{
    public class Fryable : ItemScript
    {
        public List<SO_Item> fryResults;
        [SerializeField] private Timer fryTimer;
        
        public CircleTimerDisplay circleTimer;
        public bool isFrying => fryTimer.isRunning;
        
        [Header("Config")]
        [SerializeField] private float cookDuration;
        public Color timerFillColor = CircleTimerDisplay.defaultFillColor;

        private void Start()
        {
            if (fryTimer == null)
                return;
            
            fryTimer.SetDuration(cookDuration);
            fryTimer.onFinished.AddListener(FryDone);
            
            if (fryResults.Count <= 0)
                return;
            
            circleTimer = new CircleTimerDisplay(fryTimer, fryResults[0].icon);
            circleTimer.fillColor = timerFillColor;
            
            itemInteractor.onHover.AddListener(circleTimer.ShowUI);
            itemInteractor.onHovering.AddListener(circleTimer.ShowUI);
            itemInteractor.onUnHover.AddListener(circleTimer.HideUI);
        }

        public void StartFry()
        {
            if (!fryTimer.isPause)
                fryTimer.StartTimer();
            else
                fryTimer.UnPause();
        }
        
        public void StopFry()
        {
            fryTimer.Pause();
            circleTimer.HideUI();
        }

        private void FryDone()
        {
            foreach (var fryResult in fryResults)
            {
                Item result = Instantiate(fryResult.itemPrefab, transform.position, transform.rotation).GetComponent<Item>();
                
                result.transform.SetParent(Environment.Instance.root);
                result.Initialize();
            }
            circleTimer.HideUI();
            Destroy(gameObject);
        }
    }
}