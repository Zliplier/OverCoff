using System;
using System.Collections;
using Players;
using UI;
using UI.Display;
using UnityEditor;
using UnityEngine;
using Zlipacket.CoreZlipacket.Scene.Transition;

namespace Interactable.Object
{
    public class TeleportScript : MonoBehaviour
    {
        public Transform targetWaypoint;
        public SO_SceneTransition transition;
        private Interactor interactor;

        private Coroutine co_Transition = null;
        public bool isWarping => co_Transition != null;
        
        [Header("Configs")]
        public float delay = 0.2f;

        public void Warp(Player player)
        {
            Warp(player, targetWaypoint);
        }
        
        public void Warp(Player player, Transform target)
        {
            if (isWarping)
                return;
            
            player.playerInputMap.SetMapEnable(false);
            player.uiInputMap.SetMapEnable(false);
            co_Transition = StartCoroutine(Warping(player, target));
        }

        private IEnumerator Warping(Player player, Transform targetWaypoint)
        {
            //Debug.Log("Start Warping");
            UIManager uiManager = player.uiManager;
            Transform canvas = uiManager.GetPanel("Scene", "Transition").panelRoot.transform;
            SceneTransition sceneTransition = transition.InitializeTransition(canvas);
            
            yield return sceneTransition.StartTransition();
            
            player.rb.position = targetWaypoint.position;
            player.rb.rotation = targetWaypoint.rotation;
            //Debug.Log("Warp to: " + targetWaypoint.position);
            
            yield return new WaitForSeconds(delay);
            
            yield return sceneTransition.EndTransition();
            
            player.playerInputMap.SetMapEnable(true);
            player.uiInputMap.SetMapEnable(true);
            co_Transition = null;
            //Debug.Log("End Warping");
        }
    }
}