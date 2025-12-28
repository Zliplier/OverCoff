using System.Collections.Generic;
using UnityEngine;

namespace Items.Data
{
    [CreateAssetMenu(fileName = "Item Data List", menuName = "Items/Item Data List")]
    public class SO_ItemList : ScriptableObject
    {
        public string listID;
        public List<SO_Item> Items;
    }
}