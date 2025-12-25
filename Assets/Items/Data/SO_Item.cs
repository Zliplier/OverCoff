using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

namespace Items.Data
{
    [CreateAssetMenu(fileName = "Item", menuName = "Items/Default Item")]
    public class SO_Item : ScriptableObject
    {
        public string nameID => itemData.nameId;
        public ItemData itemData;
        
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