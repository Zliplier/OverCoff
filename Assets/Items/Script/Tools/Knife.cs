using System;
using Items.Script.Ingredients;
using UnityEngine;

namespace Items.Script.Tools
{
    public class Knife : ItemScript
    {
        private void OnCollisionEnter(Collision other)
        {
            if (other.gameObject.TryGetComponent(out Cuttable cutable))
            {
                cutable.Cut();
            }
        }
    }
}