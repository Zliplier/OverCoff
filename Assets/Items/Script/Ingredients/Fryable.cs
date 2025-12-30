using System;
using System.Collections;
using System.Collections.Generic;
using Items.Data;
using UnityEngine;
using Zlipacket.CoreZlipacket.Tools;

namespace Items.Script.Ingredients
{
    public class Fryable : ItemScript
    {
        [SerializeField] private List<SO_Item> fryResult;
        private Timer fryTimer;
        
        [Header("Config")]
        [SerializeField] private float cookDuration;

        private void Start()
        {
            fryTimer = new GameObject("FryTimer").AddComponent<Timer>();
            fryTimer.transform.SetParent(transform);
            fryTimer.transform.localPosition = Vector3.zero;
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