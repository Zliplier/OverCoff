using Items.Script.Ingredients;
using UnityEngine;

namespace Items.Script.Furniture
{
    public class FryingPan : MonoBehaviour
    {
        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.TryGetComponent(out Fryable fryable))
                fryable.StartFry();
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.gameObject.TryGetComponent(out Fryable fryable))
                fryable.StopFry();
        }
    }
}