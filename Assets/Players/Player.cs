using System;
using Players.Data;
using Players.UI;
using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.Events;
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
        public UIManager uiManager;
        
        #region Health
        public UnityEvent<float, float> onHealthChanged;
        public float health 
        {
            get { return playerData.health; }
            set
            {
                playerData.health = Mathf.Clamp(value, 0f, playerData.maxHealth);
                onHealthChanged?.Invoke(playerData.health, maxHealth);
            }
        }
        public float maxHealth
        { get { return playerData.maxHealth; } set { playerData.maxHealth = value; } }
        #endregion

        #region Stamina
        public UnityEvent<float, float> onStaminaChanged;
        public float stamina
        {
            get { return playerData.stamina; }
            set
            {
                playerData.stamina = Mathf.Clamp(value, 0f, maxStamina);
                onStaminaChanged?.Invoke(playerData.stamina, maxStamina);
            }
        }
        public float maxStamina
        { get { return playerData.maxStamina; } set { playerData.maxStamina = value; } }
        #endregion

        #region Money
        public UnityEvent<float> onMoneyChanged;
        public int money
        {
            get { return playerData.money; }
            set
            {
                money = Math.Clamp(value, 0, Int32.MaxValue);
                onMoneyChanged?.Invoke(playerData.money);
            }
        }
        #endregion

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