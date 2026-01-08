using System.Collections.Generic;
using Items.Data;
using UnityEngine;
using UnityEngine.Events;
using Zlipacket.CoreZlipacket.Misc;

namespace Items.Script.Ingredients
{
    public class Cuttable : ItemScript
    {
        public List<SO_Item> cutResults;

        public UnityEvent onCut;
        
        public void Cut()
        {
            onCut?.Invoke();
            
            foreach (var cutResult in cutResults)
            {
                Item result = Instantiate(cutResult.itemPrefab
                    , transform.position + (Vector3.up * 0.1f), transform.rotation).GetComponent<Item>();
                
                result.transform.SetParent(Environment.Instance.root);
                result.Initialize();
            }
            
            Destroy(gameObject);
        }
    }
}