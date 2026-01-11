using System;
using Inventory;
using Items;
using Items.Data;
using Players.Data;
using Players.PlayerScripts;
using UI;
using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;
using Zlipacket.CoreZlipacket.Player.Input;
using Zlipacket.CoreZlipacket.Player.Input.InputMap;
using Environment = Zlipacket.CoreZlipacket.Misc.Environment;

namespace Players
{
    [RequireComponent(typeof(Rigidbody), typeof(Collider))]
    public class Player : MonoBehaviour
    {
        public SO_Player player;
        public PlayerData playerData => player.playerData; //TODO: Change later when Save/Load.
        
        public Rigidbody rb;
        public CinemachineCamera cam;
        public CinemachineInputAxisController camInputAxis => cam.GetComponent<CinemachineInputAxisController>();
        
        public UIManager uiManager;
        public PlayerBook playerBook;
        public InventoryManager playerInventory;

        public SO_InputReader inputReader => player.inputReader;
        public PlayerInputMap playerInputMap => inputReader.playerInputMap;
        public UIInputMap uiInputMap => inputReader.uiInputMap;
        
        [Header("Stat Events")]
        #region Health
        public UnityEvent<float, float> onHealthChanged;
        public UnityEvent<float> onHealthFull;
        public float health 
        {
            get { return playerData.health; }
            set
            {
                playerData.health = Mathf.Clamp(value, 0f, playerData.maxHealth);
                onHealthChanged?.Invoke(health, maxHealth);
                if (health >= maxHealth)
                    onHealthFull?.Invoke(maxHealth);
            }
        }
        public float maxHealth
        { get { return playerData.maxHealth; } set { playerData.maxHealth = value; } }
        #endregion
        
        #region Stamina
        public UnityEvent<float, float> onStaminaChanged;
        public UnityEvent<float> onStaminaFull;
        public float stamina
        {
            get { return playerData.stamina; }
            set
            {
                playerData.stamina = Mathf.Clamp(value, 0f, maxStamina);
                onStaminaChanged?.Invoke(stamina, maxStamina);
                if (stamina >= maxStamina)
                    onStaminaFull?.Invoke(maxStamina);
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
            SetCursorLockState(true);
            
            playerInputMap.SetMapEnable(true);
            uiInputMap.SetMapEnable(false);
        }
        
        public void SetCursorLockState(bool enabled)
        {
            if (enabled)
            {
                Cursor.lockState = CursorLockMode.Locked;
                Cursor.visible = false;
                camInputAxis.enabled = true;
            }
            else
            {
                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;
                camInputAxis.enabled = false;
            }
        }
        
        public float defaultSpawningDistance => playerData.defaultSpawningDistance;
        
        public void SpawnItem(ItemData itemData)
        {
            Vector3 spawnPosition = cam.transform.position + (cam.transform.forward * defaultSpawningDistance);
            
            //Check for obstruction.
            if (Physics.Raycast(cam.transform.position, cam.transform.forward, 
                    out RaycastHit hit, defaultSpawningDistance))
                spawnPosition = hit.point;
            
            //Spawning Object Item.
            Item newItem = Instantiate(itemData.scriptableObject.itemPrefab, spawnPosition, Quaternion.identity).GetComponent<Item>();
            newItem.transform.SetParent(Environment.Instance.root);
            newItem.Initialize(itemData);
            newItem.itemData.stack = 1; //Spawn only 1 item of the stack.
        }
    }
}