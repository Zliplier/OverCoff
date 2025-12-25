using System;
using System.Collections.Generic;
using System.Linq;
using Items.Data;
using UnityEngine;
using Zlipacket.CoreZlipacket.Tools;

namespace Items
{
    public class ItemManager : Singleton<ItemManager>
    {
        public List<SO_ItemDataList> ItemDatabase;

        public SO_Item GetItemData(string nameID, string listID = "")
        {
            //If specify Item List.
            if (!string.IsNullOrEmpty(listID))
            {
                return GetItemDataList(listID).Items.Find(x => string.Equals(x.nameID, nameID, StringComparison.InvariantCultureIgnoreCase));
            }
            
            //Find match for Item from all list.
            foreach (var itemList in ItemDatabase)
            {
                if (itemList.Items.Any(x => string.Equals(x.nameID, nameID, StringComparison.InvariantCultureIgnoreCase)))
                {
                    return itemList.Items.Find(x =>
                        string.Equals(x.nameID, nameID, StringComparison.InvariantCultureIgnoreCase));
                }
            }
            
            Debug.LogError($"{nameID} not found.");
            return null;
        }

        public SO_ItemDataList GetItemDataList(string listID) => ItemDatabase.Find(x => string.Equals(x.listID, listID, StringComparison.InvariantCultureIgnoreCase));
    }
}