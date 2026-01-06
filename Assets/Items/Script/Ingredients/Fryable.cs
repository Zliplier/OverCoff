using System.Collections.Generic;
using Items.Data;
using UnityEngine;
using Zlipacket.CoreZlipacket.Tools;

namespace Items.Script.Ingredients
{
    public class Fryable : ItemScript
    {
        public List<SO_Item> fryResult;
        [SerializeField] private Timer fryTimer;
        
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
            //TODO: Spawn fry result
        }
    }
}