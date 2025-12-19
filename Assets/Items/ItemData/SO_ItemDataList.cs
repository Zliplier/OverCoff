using System.Collections.Generic;
using UnityEngine;

namespace Items.ItemData
{
    [CreateAssetMenu(fileName = "Item Data List", menuName = "Items/ItemDataList")]
    public class SO_ItemDataList : ScriptableObject
    {
        public string listID;
        public List<SO_ItemData> Items;
    }
}