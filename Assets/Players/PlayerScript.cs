using Players.Data;
using Players.UI;
using Unity.Cinemachine;
using UnityEngine;
using Zlipacket.CoreZlipacket.Player.Input;

namespace Players
{
    [RequireComponent(typeof(Player))]
    public class PlayerScript : MonoBehaviour
    { 
        [HideInInspector] public Player player;
        public PlayerData data => player.playerData;
        protected Rigidbody rb => player.rb;
        protected SO_InputReader inputReader => player.data.inputReader;
        protected CinemachineCamera cam => player.cam;
        protected UIManager uiManager => player.uiManager;
        
        protected virtual void Awake()
        {
            player = GetComponent<Player>();
        }
    }
}