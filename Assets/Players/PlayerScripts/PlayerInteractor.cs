using System;
using Items.ItemScript;
using UnityEngine;

namespace Players.PlayerScripts
{
    [RequireComponent(typeof(Player))]
    public class PlayerInteractor : MonoBehaviour
    {
        private RaycastHit raycastHit;
        public GameObject selectedObject => raycastHit.collider.gameObject;
        
        public float minDistance = 0.3f;
        public float maxDistance = 3.5f;
        
        private Player player;

        private void Awake()
        {
            player = GetComponent<Player>();
        }

        private void Update()
        {
            //if (Physics.Raycast(player.camera.transform.position + player.transform.forward * minDistance));
        }
        
        public void Interact()
        {
            if (selectedObject == null)
                return;
            
            if (selectedObject.TryGetComponent(out ItemInteractor itemInteractor))
            {
                itemInteractor.Interact();
            }
        }
    }
}