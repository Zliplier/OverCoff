using System;
using UnityEngine;

namespace Items.Script.Tools
{
    public class Knife : MonoBehaviour
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