using System.Collections.Generic;
using UnityEngine;
using Zlipacket.CoreZlipacket.Tools;

namespace Characters.Customer
{
    public class CustomerManager : Singleton<CustomerManager>
    {
        [SerializeField] private List<GameObject> customerSpawner;
        [SerializeField] private List<GameObject> customerPrefab;

        public void SpawnCustomer(int customerID)
        {
            Vector3 randomCirclePoint = Random.insideUnitCircle * 2f;
            Instantiate(customerPrefab[customerID], customerSpawner[Random.Range(0, customerSpawner.Count)].transform.position + randomCirclePoint, Quaternion.identity);
        }
        
        public void SpawnCustomer(GameObject customerPrefab)
        {
            Vector3 randomCirclePoint = Random.insideUnitCircle * 2f;
            if (this.customerPrefab.Contains(customerPrefab))
            {
                Instantiate(customerPrefab, customerSpawner[Random.Range(0, customerSpawner.Count)].transform.position + randomCirclePoint, Quaternion.identity);
            }
        }
    }
}