using System;
using System.Collections.Generic;
using Interactable;
using Items;
using Items.Data;
using Items.Script;
using Players;
using UnityEngine;
using UnityEngine.UI;
using Zlipacket.CoreZlipacket.Audio;
using Zlipacket.CoreZlipacket.Tools;
using Random = UnityEngine.Random;

namespace Characters.Customer
{
    public class CustomerOrdering : MonoBehaviour
    {
        [SerializeField] private List<SO_Item> orderList;
        [HideInInspector] public SO_Item order;
        private CustomerController customerController;
        [SerializeField] private ObjectTimer timer;
        [SerializeField] private Image iconDisplay;
        [SerializeField] private float orderTime;
        [SerializeField] private AudioClip orderSuccessFX;
        [SerializeField] private AudioClip orderFailFX;
        [SerializeField] private AudioClip orderStartFX;

        private void Start()
        {
            order = orderList[Random.Range(0, orderList.Count)];
            customerController = GetComponentInParent<CustomerController>();
            iconDisplay.sprite = order.icon;
        }

        protected void OnCollisionEnter(Collision other)
        {
            if (customerController.CurrentState.StateKey != E_CustomerState.Order) return;

            if (other.gameObject.TryGetComponent(out Cup item))
            {
                InteractItem(item.item);
            }
            
        }

        protected void InteractItem(Item item)
        {
            if (item.item.nameID  == order.nameID)
            {
                OrderSuccess(item.item);
            }
            else
            {
                OrderFail();
            }
            
            /*base.InteractItem(itemObject, itemID);*/
            item.DestroyItem();
            
            timer.PauseTimer();
            customerController.OrderDelivered();
        }

        public void StartOrder()
        {
            timer.SetDuration(orderTime);
            timer.StartTimer();
            if (orderStartFX != null)
                SoundFXManager.Instance.PlaySoundFX(orderStartFX, transform, 1f);
        }

        public void OrderSuccess(SO_Item itemID)
        {
            /*GameManager.Instance.AddMoney(itemID.cost);*/

            Player.Instance.money += itemID.cost;
            if (orderSuccessFX != null)
                SoundFXManager.Instance.PlaySoundFX(orderSuccessFX, transform, 1f);
        }

        public void OrderFail()
        {
            customerController.orderDelivered = true;
            if (orderFailFX != null)
                SoundFXManager.Instance.PlaySoundFX(orderFailFX, transform, 1f);
        }
    }
}