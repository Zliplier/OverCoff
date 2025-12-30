using Items.Data;
using UnityEngine;

namespace Items.Script.Furniture
{
    public class IngredientBin : ItemScript
    {
        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.TryGetComponent(out Item item))
            {
                if (item.item is SO_ItemIngredients)
                    Destroy(item.gameObject);
            }
        }
    }
}