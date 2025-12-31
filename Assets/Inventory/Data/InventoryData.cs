using System;
using System.Collections.Generic;
using Items.Data;

namespace Inventory.Data
{
    [Serializable]
    public class InventoryData
    {
        public List<ItemData> items;
    }
}