using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Items.Data;
using Items.Script;
using UnityEngine;

namespace Items
{
    [RequireComponent(typeof(Collider))]
    public class Item : MonoBehaviour
    {
        public SO_Item item;
        public ItemData itemData;
        
        public Rigidbody rb;
        
        public ItemInteractor itemInteractor;

        public Tween itemAnimation = null;
        public bool isTweening => itemAnimation != null;

        private void Awake()
        {
            if (string.IsNullOrWhiteSpace(itemData.nameId))
                itemData = new ItemData(item.itemData);
            
            rb = GetComponent<Rigidbody>();
            itemInteractor = GetComponent<ItemInteractor>();
        }

        /*private void Start()
        {
            transform.localScale = new Vector3(0.5f, 0.5f, 0.5f);
            transform.DOScale(1, 0.3f);
        }*/

        private void OnDestroy()
        {
            if (isTweening)
                itemAnimation.Kill();
        }

        public void Initialize(ItemData itemData)
        {
            this.itemData = new ItemData(itemData);
            
            Initialize();
        }

        public void Initialize()
        {
            name = itemData.nameId;
            
            PlaySpawnAnimation();
        }
        
        public static bool CheckFilterTags(List<ItemTag> inputFilter, Item item, bool checkAll = false)
        {
            foreach (var filter in inputFilter)
            {
                //Might have some bugs man. Nah, it's fine. Probably. Hopefully.
                if (item.item.itemTag.Contains(filter))
                    return true;
                if (checkAll)
                    return false;
            }
            
            return false;
        }
        
        public void ResetVelocity() => StartCoroutine(ResetingVelocity());
        private IEnumerator ResetingVelocity()
        {
            rb.Sleep();
            yield return null;
            rb.WakeUp();
        }

        public Tween PlaySpawnAnimation(float time = 0.3f)
        {
            if (isTweening)
                itemAnimation.Kill();
            
            transform.localScale = new Vector3(0.5f, 0.5f, 0.5f);
            itemAnimation = transform.DOScale(1, time);
            return itemAnimation;
        }

        public Tween PlayDestroyAnimation(float time)
        {
            if (isTweening)
                itemAnimation.Kill();
            
            itemAnimation = transform.DOScale(0.7f, time);
            return itemAnimation;
        }

        public void DestroyItem(float timeToDestroy = 0.1f)
        {
            PlayDestroyAnimation(timeToDestroy).onComplete += () => Destroy(gameObject);
        }
    }
}