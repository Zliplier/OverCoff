using System;
using Players.Data;
using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.Serialization;

namespace Players
{
    [RequireComponent(typeof(Rigidbody), typeof(Collider))]
    public class Player : MonoBehaviour
    {
        public SO_PlayerData data;
        public PlayerData playerData => data.playerData; //TODO: Change later when Save/Load.
        
        public Rigidbody rb;
        public CinemachineCamera cam;

        private void Awake()
        {
            rb = GetComponent<Rigidbody>();
        }

        private void Start()
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
    }
}