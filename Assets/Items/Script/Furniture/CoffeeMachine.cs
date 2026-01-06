using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Items.Data;
using Players;
using Players.UI;
using UI;
using UnityEngine;
using UnityEngine.UI;
using Zlipacket.CoreZlipacket.Tools;

namespace Items.Script.Furniture
{
    [RequireComponent(typeof(ItemInteractor))]
    public class CoffeeMachine : ItemScript
    {
        [SerializeField] private ItemHolderTrigger cupHolder;
        [SerializeField] private Timer coffeeTimer;
        [SerializeField] private SO_Item coffeeResult;
        public GameObject powderDisplay;

        [Header("Configs")]
        public List<ItemTag> inputFilterTag;

        public float duration = 10f;
        
        public SO_Item coffeePowder => data.containItems.FirstOrDefault();
        public bool isPowderEmpty => coffeePowder == null;

        public Item cup => cupHolder.isEmpty ? cupHolder.containItem : null;
        public bool hasCup => cup != null;

        public bool isMakingCoffee => coffeeTimer.isRunning;
        
        private ItemInteractor itemInteractor;

        protected override void Awake()
        {
            base.Awake();
            itemInteractor = GetComponent<ItemInteractor>();
        }
        
        private void Start()
        {
            coffeeTimer.SetDuration(duration);
            coffeeTimer.onFinished.AddListener(OnCoffeeFinish);
            
            cupHolder.onContain.AddListener(StartMakingCoffee);
            cupHolder.onUnContain.AddListener(StopMakingCoffee);
            
            itemInteractor.onHover.AddListener(ShowUI);
            itemInteractor.onUnHover.AddListener(HideUI);
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
            Destroy(item.gameObject);
            powderDisplay.SetActive(true);
            
            StartMakingCoffee();
        }

        private bool TryStartCoffee() => hasCup && !isPowderEmpty && !isMakingCoffee;
        
        private void StartMakingCoffee()
        {
            
            if (!TryStartCoffee())
                return;
            Debug.Log("Coffee started");
            if (isMakingCoffee)
                StopMakingCoffee();
            
            coffeeTimer.StartTimer();
        }
        
        private void StopMakingCoffee()
        {
            coffeeTimer.StopTimer();
            Debug.Log("Coffee stopped");
        }
        
        private void OnCoffeeFinish()
        {
            Debug.Log("Coffee finished");
            Instantiate(coffeeResult.itemPrefab, cup.transform.position, cup.transform.rotation);
            
            powderDisplay.SetActive(false);
            
            coffeeTimer.StopTimer();
        }

        private FillCircleUI callbackUI = null;
        public bool isUIShown => callbackUI != null;
        
        private void ShowUI(GameObject player)
        {
            if (!isMakingCoffee)
                return;
            if (isUIShown)
                return;
            
            PlayerUIManager playerUI = player.GetComponent<Player>().playerUIManager;
            if (playerUI == null)
                return;

            callbackUI = playerUI.SpawnUIElement("Main", "First Person", 
                Resources.Load<GameObject>(PlayerUIManager.CIRCLE_TIMER_UI_PATH), Vector3.zero, 
                "TimerUI").GetComponent<FillCircleUI>();
            
            UpdateTimerUI(1 - coffeeTimer.GetPercentage());
            coffeeTimer.onTimerUpdate.AddListener(UpdateTimerUI);
        }

        private void UpdateTimerUI(float percentage)
        {
            callbackUI.fillAmount = 1 - percentage;
        }

        private void HideUI(GameObject player)
        {
            if (!isUIShown)
                return;
            
            coffeeTimer.onTimerUpdate.RemoveListener(UpdateTimerUI);
            
            Destroy(callbackUI.gameObject);
            callbackUI = null;
        }
    }
}