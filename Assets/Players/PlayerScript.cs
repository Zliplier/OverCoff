using System.Collections;
using Inventory;
using Items.Data;
using Players.Data;
using UI;
using Unity.Cinemachine;
using UnityEngine;
using Zlipacket.CoreZlipacket.Player.Input;
using Zlipacket.CoreZlipacket.Player.Input.InputMap;

namespace Players
{
    [RequireComponent(typeof(Player))]
    public abstract class PlayerScript : MonoBehaviour
    { 
        [HideInInspector] public Player player;
        public PlayerData data => player.playerData;
        protected Rigidbody rb => player.rb;
        protected SO_InputReader inputReader => player.inputReader;
        protected PlayerInputMap playerInputMap => player.playerInputMap;
        protected UIInputMap uiInputMap => player.uiInputMap;
        
        protected CinemachineCamera cam => player.cam;
        protected UIManager UIManager => player.uiManager;
        protected InventoryManager playerInventory => player.playerInventory;

        protected float defaultSpawningDistance => data.defaultSpawningDistance;
        
        protected virtual void Awake()
        {
            player = GetComponent<Player>();
        }
        
        protected void SpawnItem(ItemData itemData) => player.SpawnItem(itemData);
    }
}