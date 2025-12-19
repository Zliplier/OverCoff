using System;
using Players.PlayerConfig;
using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.Serialization;

namespace Players
{
    public class Player : MonoBehaviour
    {
        public SO_PlayerConfig Config;
        
        public string playerID => Config.playerID;
        
        public CinemachineCamera camera;

        private void Awake()
        {
            
        }
    }
}