using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Items.Data;
using Items.Script.Ingredients;
using Players;
using UI.Display;
using UnityEngine;
using Zlipacket.CoreZlipacket.Tools;
using Environment = Zlipacket.CoreZlipacket.Misc.Environment;

namespace Items.Script.Furniture
{
    [RequireComponent(typeof(ItemInteractor))]
    public class Grinder : ItemScript
    {
        public SO_Item grindItem => data.containItems.FirstOrDefault();
        public bool isEmpty => grindItem == null;

        private List<SO_Item> grindResults;
        
        public CircleBarDisplay circleBar;
        private int grindNum = 0;
        private int maxGrindNum;
        private float elapsedTime = 0f;

        [SerializeField] private GameObject grinderArm;
        [SerializeField] private GameObject spawnPos;
        
        [Header("Configs")]
        public List<ItemTag> inputFilterTag;

        private void Start()
        {
            circleBar = new CircleBarDisplay();
            
            itemInteractor.onInteract.AddListener(Grind);
            
            itemInteractor.onHover.AddListener(ShowUI);
            itemInteractor.onHovering.AddListener(ShowUI);
            itemInteractor.onUnHover.AddListener(circleBar.HideUI);
        }

        private void ShowUI(Player player)
        {
            if (isEmpty)
                return;
            circleBar.ShowUI(player);
            circleBar.UpdateUI(Mathf.InverseLerp(0, 1, 1 - elapsedTime/maxGrindNum));
        }

        private void OnCollisionEnter(Collision other)
        {
            if (!other.gameObject.TryGetComponent(out Grindable grindItem))
                return;
            if (!Item.CheckFilterTags(inputFilterTag, grindItem.item))
                return;
            if (!isEmpty)
                return;
            
            data.containItems.Add(grindItem.item.item /*WTF is this: item.item.item.item.item.item*/);
            grindResults = new List<SO_Item>(grindItem.grindResults);
            circleBar.icon = grindResults[0].icon;
            grindNum = grindItem.grindNum;
            maxGrindNum = grindNum;
            grindItem.DestroyItem();
        }

        private void Update()
        {
            if (isEmpty)
                return;
            
            if (elapsedTime < maxGrindNum - grindNum)
            {
                elapsedTime += 3 * Time.deltaTime;
                circleBar.UpdateUI(Mathf.InverseLerp(0, 1, 1 - elapsedTime/maxGrindNum));
                grinderArm.transform.localRotation = Quaternion.Euler(0, 359 * (elapsedTime / maxGrindNum), 0);
                
                if (elapsedTime > maxGrindNum)
                {
                    GrindDone();
                }
            }
        }

        private void Grind(Player player)
        {
            if (isEmpty)
                return;
            grindNum = Math.Clamp(grindNum - 1, 0, maxGrindNum);
        }

        private void GrindDone()
        {
            foreach (var grindResult in grindResults)
            {
                Item result = Instantiate(grindResult.itemPrefab
                    , spawnPos.transform.position, transform.rotation).GetComponent<Item>();
                
                result.transform.SetParent(Environment.Instance.root);
                result.Initialize();
            }
            
            elapsedTime = 0f;
            grinderArm.transform.localRotation = Quaternion.Euler(0, 0, 0);
            data.containItems.Clear();
            grindResults.Clear();
            
            circleBar.HideUI();
        }
    }
}