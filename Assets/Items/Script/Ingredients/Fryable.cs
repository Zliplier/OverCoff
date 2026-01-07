using System.Collections.Generic;
using Items.Data;
using Players;
using Players.UI;
using UI;
using UnityEngine;
using Zlipacket.CoreZlipacket.Misc;
using Zlipacket.CoreZlipacket.Tools;

namespace Items.Script.Ingredients
{
    public class Fryable : ItemScript
    {
        public List<SO_Item> fryResults;
        [SerializeField] private Timer fryTimer;
        public bool isFrying => fryTimer.isRunning;
        
        [Header("Config")]
        [SerializeField] private float cookDuration;

        private void Start()
        {
            if (fryTimer == null)
                return;
            
            fryTimer.SetDuration(cookDuration);
            fryTimer.onFinished.AddListener(FryDone);
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
        }

        private void FryDone()
        {
            foreach (var fryResult in fryResults)
            {
                Item result = Instantiate(fryResult.itemPrefab, Environment.Instance.transform).GetComponent<Item>();
                
                result.Initialize();
            }
        }
    }
}