using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Items.Data;
using Players;
using Players.UI;
using UI;
using UI.Display;
using UnityEngine;
using UnityEngine.UI;
using Zlipacket.CoreZlipacket.Tools;
using Environment = Zlipacket.CoreZlipacket.Misc.Environment;

namespace Items.Script.Furniture
{
    [RequireComponent(typeof(ItemInteractor))]
    public class CoffeeMachine : ItemScript
    {
        [SerializeField] private ItemHolderTrigger cupHolder;
        [SerializeField] private Timer coffeeTimer;
        [SerializeField] private SO_Item coffeeResult;
        public GameObject powderDisplay;

        public CircleTimerDisplay circleTimer;

        [Header("Configs")]
        public List<ItemTag> inputFilterTag;

        public float duration = 10f;
        
        public SO_Item coffeePowder => data.containItems.FirstOrDefault();
        public bool isPowderEmpty => coffeePowder == null;

        public Item cup => !cupHolder.isEmpty ? cupHolder.containItem : null;
        public bool hasCup => cup != null;

        public bool isMakingCoffee => coffeeTimer.isRunning;
        
        private void Start()
        {
            circleTimer = new CircleTimerDisplay(coffeeTimer, coffeeResult.icon);
            
            coffeeTimer.SetDuration(duration);
            coffeeTimer.onFinished.AddListener(OnCoffeeFinish);
            
            cupHolder.onContain.AddListener(StartMakingCoffee);
            cupHolder.onUnContain.AddListener(StopMakingCoffee);
            
            itemInteractor.onHover.AddListener(circleTimer.ShowUI);
            itemInteractor.onUnHover.AddListener(circleTimer.HideUI);
            itemInteractor.onHovering.AddListener(circleTimer.ShowUI);
        }

        private void OnCollisionEnter(Collision other)
        {
            if (!other.gameObject.TryGetComponent(out Item item))
                return;
            if (!Item.CheckFilterTags(inputFilterTag, item))
                return;
            if (!isPowderEmpty)
                return;
            
            data.containItems.Add(item.item);
            item.DestroyItem();
            powderDisplay.SetActive(true);
            
            StartMakingCoffee();
        }

        private bool TryStartCoffee() => hasCup && !isPowderEmpty && !isMakingCoffee;
        
        private void StartMakingCoffee()
        {
            if (!TryStartCoffee())
                return;
            
            if (isMakingCoffee)
                StopMakingCoffee();
            
            //Debug.Log("Coffee started");
            
            coffeeTimer.StartTimer();
        }
        
        private void StopMakingCoffee()
        {
            coffeeTimer.StopTimer();
            circleTimer.HideUI();
            
            //Debug.Log("Coffee stopped");
        }
        
        private void OnCoffeeFinish()
        {
            //Debug.Log("Coffee finished");
            
            cup.GetComponent<Cup>().AddIngredient(coffeeResult);
            
            powderDisplay.SetActive(false);
            data.containItems.RemoveAt(0);
            
            coffeeTimer.StopTimer();
            circleTimer.HideUI();
        }
    }
}