using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

namespace Items.ItemData
{
    [CreateAssetMenu(fileName = "Item Data", menuName = "Items/ItemData")]
    public class SO_ItemData : ScriptableObject
    {
        public string nameID;
        [SerializeField] public ItemDisplayData displayData;
        public List<ItemTag> itemTag;
        public int cost;
        public Sprite icon;
        public GameObject prefab;
    }
}