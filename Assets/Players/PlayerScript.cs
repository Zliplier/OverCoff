using Players.Data;
using Unity.Cinemachine;
using UnityEngine;
using Zlipacket.CoreZlipacket.Player.Input;

namespace Players
{
    [RequireComponent(typeof(Player))]
    public class PlayerScript : MonoBehaviour
    { 
        [HideInInspector] public Player player;
        public PlayerData data => player.data.playerData;
        protected Rigidbody rb => player.rb;
        protected SO_InputReader inputReader => player.data.inputReader;
        protected CinemachineCamera cam => player.cam;
        
        public float health 
        {
            get { return data.health; }
            set { data.health = value > maxHealth ? maxHealth : value < 0 ? 0 : value; }
        }
        public float maxHealth
        { get { return data.maxHealth; } set { data.maxHealth = value; } }

        public float stamina
        {
            get { return data.stamina; }
            set { data.stamina = value > maxStamina ? maxStamina : value < 0 ? 0 : value; }
        }
        public float maxStamina
        { get { return data.maxStamina; } set { data.maxStamina = value; } }
        
        protected virtual void Awake()
        {
            player = GetComponent<Player>();
        }
    }
}