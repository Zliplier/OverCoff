using UnityEngine;
using Zlipacket.CoreZlipacket.Audio;
using Zlipacket.CoreZlipacket.Player.Input;

namespace Players.Data
{
    [CreateAssetMenu(menuName = "Player Data", fileName = "PlayerData")]
    public class SO_PlayerData : ScriptableObject
    {
        public SO_InputReader inputReader;
        public PlayerData playerData;
    }
}