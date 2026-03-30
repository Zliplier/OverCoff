using System;
using System.Collections.Generic;
using System.Linq;
using Items.Script.Ingredients;
using UnityEngine;
using UnityEngine.VFX;

namespace Interactable.Shop.Furniture
{
    public class FryingPan : MonoBehaviour
    {
        private List<GameObject> fryingObjects = new List<GameObject>();
        public VisualEffect smokeVfx;

        private void Start()
        {
            if (smokeVfx != null)
                smokeVfx.Stop();
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.TryGetComponent(out Fryable fryable))
            {
                fryable.StartFry();
                if (!fryingObjects.Contains(fryable.gameObject))
                    fryingObjects.Add(fryable.gameObject);
                
                if (fryingObjects.Count > 0 && smokeVfx != null)
                    smokeVfx.Play();
            }
            
            fryingObjects = fryingObjects.Where(item => item != null).Distinct().ToList();
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.gameObject.TryGetComponent(out Fryable fryable))
            {
                fryable.StopFry();
                if (fryingObjects.Contains(fryable.gameObject))
                    fryingObjects.Remove(fryable.gameObject);
                
                if (fryingObjects.Count <= 0 && smokeVfx != null)
                    smokeVfx.Stop();
            }
            
            fryingObjects = fryingObjects.Where(item => item != null).Distinct().ToList();
        }
    }
}