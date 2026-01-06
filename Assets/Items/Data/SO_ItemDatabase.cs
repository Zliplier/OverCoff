using System.Collections.Generic;
using UnityEngine;

namespace Items.Data
{
    [CreateAssetMenu(fileName = "Item Database", menuName = "Items/Item Database")]
    public class SO_ItemDatabase : ScriptableObject
    {
        public List<SO_ItemList> database;
    }
}