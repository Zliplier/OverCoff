using System;
using System.Collections;
using Players;
using Players.UI;
using UI.Display;
using UnityEngine;
using Zlipacket.CoreZlipacket.Scene.Transition;

namespace Interactable.Object
{
    [RequireComponent(typeof(Interactor))]
    public class TeleportInteract : MonoBehaviour
    {
        public Transform fromWaypoint;
        public Transform targetWaypoint;
        public SO_SceneTransition transition;
        private Interactor interactor;

        private Coroutine co_Transition = null;
        public bool isWarping => co_Transition != null;
        
        [Header("Configs")]
        public float delay = 0.2f;

        private void Start()
        {
            interactor = GetComponent<Interactor>();
            
            interactor.onInteract.AddListener(Interact);
        }

        private void Interact(Player player)
        {
            if (isWarping)
                return;
            
            player.playerInputMap.SetMapEnable(false);
            player.uiInputMap.SetMapEnable(false);
            co_Transition = StartCoroutine(Warping(player));
        }

        private IEnumerator Warping(Player player)
        {
            PlayerUIManager playerUIManager = player.playerUIManager;
            Transform canvas = playerUIManager.GetPanel("Scene", "Transition").panelRoot.transform;
            SceneTransition sceneTransition = transition.InitializeTransition(canvas);
            
            yield return sceneTransition.StartTransition();
            
            player.transform.position = targetWaypoint.position;
            player.transform.rotation = targetWaypoint.rotation;
            
            yield return new WaitForSeconds(delay);
            
            yield return sceneTransition.EndTransition();
            
            player.playerInputMap.SetMapEnable(true);
            player.uiInputMap.SetMapEnable(true);
            co_Transition = null;
        }
    }
}