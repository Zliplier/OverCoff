using System.Collections.Generic;
using QuickOutline;
using UnityEngine;
using UnityEngine.Serialization;

namespace Items.ItemData
{
    [CreateAssetMenu(fileName = "Item", menuName = "Items/Default Item")]
    public class SO_Item : ScriptableObject
    {
        public string nameID;
        public ItemData itemData;
        public SO_OutlineConfig outlineConfig;
        
        public List<ItemTag> itemTag => itemData.itemTag;
        public int cost => itemData.cost;
        public Sprite icon => itemData.icon;
        public GameObject prefab => itemData.prefab;
        public ItemDisplayData displayData => itemData.displayData;

        public bool CheckTag(ItemTag tag)
        {
            return itemTag.Contains(tag);
        }
    }
}