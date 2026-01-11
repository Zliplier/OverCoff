using System;
using System.Collections.Generic;
using DG.Tweening;
using Items.Data;
using Items.Script;
using Players;
using UnityEngine;

namespace Items
{
    [RequireComponent(typeof(Item))]
    public abstract class ItemScript : MonoBehaviour
    {
        [HideInInspector] public Item item;
        public ItemData data => item.itemData;
        
        public Rigidbody rb => item.rb;
        
        public ItemInteractor itemInteractor => item.itemInteractor;

        public Tween itemAnimation
        {
            get => item.itemAnimation;
            set => item.itemAnimation = value;
        }
        
        public bool isTweening => item.isTweening;
        
        protected virtual void Awake()
        {
            item = GetComponent<Item>();
        }
        
        public void DestroyItem(float time = 0.1f) => item.DestroyItem(time);
    }
}