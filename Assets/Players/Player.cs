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