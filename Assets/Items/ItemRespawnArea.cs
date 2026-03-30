using System;
using UnityEngine;

namespace Items
{
    public class ItemRespawnArea : MonoBehaviour
    {
        private void OnTriggerEnter(Collider other)
        {
            //Debug.Log("OnTriggerEnter");
            if (!other.TryGetComponent(out Item item)) return;
            
            item.StartRespawn();
        }

        private void OnTriggerExit(Collider other)
        {
            if (!other.TryGetComponent(out Item item)) return;
            
            item.CancelRespawn();
        }
    }
}