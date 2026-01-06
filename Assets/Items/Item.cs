using System;
using System.Collections.Generic;
using DG.Tweening;
using Items.Data;
using UnityEngine;

namespace Items
{
    [RequireComponent(typeof(Collider))]
    public class Item : MonoBehaviour
    {
        public SO_Item item;
        public ItemData itemData;
        
        public Rigidbody rb;

        private void Awake()
        {
            if (string.IsNullOrWhiteSpace(itemData.nameId))
                itemData = new ItemData(item.itemData);
            
            rb = GetComponent<Rigidbody>();
        }

        /*private void Start()
        {
            transform.localScale = new Vector3(0.5f, 0.5f, 0.5f);
            transform.DOScale(1, 0.3f);
        }*/

        public void Initialize(ItemData itemData)
        {
            this.itemData = new ItemData(itemData);
            
            transform.localScale = new Vector3(0.5f, 0.5f, 0.5f);
            transform.DOScale(1, 0.3f);
        }
        
        public static bool CheckFilterTags(List<ItemTag> inputFilter, Item item, bool checkAll = false)
        {
            foreach (var filter in inputFilter)
            {
                //Might have some bugs man.
                if (item.item.itemTag.Contains(filter))
                    return true;
                if (checkAll)
                    return false;
            }
            
            return false;
        }

        public void PlaySpawnAnimation(float time = 0.3f)
        {
            transform.localScale = new Vector3(0.5f, 0.5f, 0.5f);
            transform.DOScale(1, time).SetEase(Ease.Linear);
        }

        public Tween PlayDestroyAnimation(float time)
        {
            return transform.DOScale(0.5f, time).SetEase(Ease.Linear);
        }

        public void DestroyItem(float timeToDestroy = 0.1f)
        {
            PlayDestroyAnimation(timeToDestroy).onComplete += () => Destroy(gameObject, timeToDestroy);
        }
    }
}