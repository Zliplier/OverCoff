using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Items.Data;
using Items.Script.Ingredients;
using Players;
using UnityEngine;

namespace Items.Script.Furniture
{
    [RequireComponent(typeof(ItemInteractor))]
    public class Grinder : ItemScript
    {
        public SO_Item grindItem => data.containItems.FirstOrDefault();
        public bool isEmpty => grindItem == null;

        private List<SO_Item> grindResults;
        
        private Coroutine co_Grind = null;
        public bool isGrinding => co_Grind != null;
        
        [Header("Configs")]
        public List<ItemTag> inputFilterTag;
        public float grindDuration = 1f;
        
        

        private void Start()
        {
            itemInteractor.onInteract.AddListener(Grind);
            
        }

        private void OnCollisionEnter(Collision other)
        {
            if (!other.gameObject.TryGetComponent(out Grindable grindItem))
                return;
            if (!Item.CheckFilterTags(inputFilterTag, grindItem.item))
                return;
            if (!isEmpty)
                return;
            
            data.containItems.Add(grindItem.item.item /*WTF is this*/);
            grindResults = new List<SO_Item>(grindItem.grindResults);
            grindItem.DestroyItem();
        }

        private void Grind(Player player)
        {
            //if (isGrinding) ;
        }

        private IEnumerator Grinding()
        {
            yield return new WaitForSeconds(grindDuration);
            
            
        }
    }
}