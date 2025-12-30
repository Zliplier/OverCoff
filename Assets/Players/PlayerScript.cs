using Players.Data;
using Players.UI;
using Unity.Cinemachine;
using UnityEngine;
using Zlipacket.CoreZlipacket.Player.Input;
using Zlipacket.CoreZlipacket.Player.Input.InputMap;

namespace Players
{
    [RequireComponent(typeof(Player))]
    public class PlayerScript : MonoBehaviour
    { 
        [HideInInspector] public Player player;
        public PlayerData data => player.playerData;
        protected Rigidbody rb => player.rb;
        protected SO_InputReader inputReader => player.player.inputReader;
        protected PlayerInputMap playerInputMap => inputReader.playerInputMap;
        protected UIInputMap uiInputMap => inputReader.uiInputMap;
        
        protected CinemachineCamera cam => player.cam;
        protected UIManager uiManager => player.uiManager;
        
        protected virtual void Awake()
        {
            player = GetComponent<Player>();
        }
    }
}