using System;
using Interactable.Object;
using Items.Script;
using UnityEngine;

namespace Items.Resources.Ingredients.Soda
{
    [RequireComponent(typeof(ImpactDetector))]
    public class Soda : ItemScript
    {
        private ImpactDetector impactDetector;
        
        
        public float impactThreshold = 0.5f;

        private void Start()
        {
            impactDetector = GetComponent<ImpactDetector>();
            
            impactDetector.onImpact.AddListener(OnImpact);
        }

        private void OnImpact(float impactMagnitude)
        {
            if (impactThreshold > impactMagnitude)
                return;
            
            
            
            
        }
        
        
        
    }
}