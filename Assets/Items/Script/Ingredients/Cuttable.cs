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
                GameObject result = Instantiate(cutResult.itemPrefab, Environment.Instance.transform);
                
                result.name = cutResult.nameID;
            }
            
            DestroyItem();
        }
    }
}