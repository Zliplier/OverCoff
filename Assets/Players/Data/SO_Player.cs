using Players.UI;
using UnityEngine;
using Zlipacket.CoreZlipacket.Audio;
using Zlipacket.CoreZlipacket.Player.Input;

namespace Players.Data
{
    [CreateAssetMenu(menuName = "Player", fileName = "Player/Data")]
    public class SO_Player : ScriptableObject
    {
        public SO_InputReader inputReader;
        public PlayerData playerData;
        public GameObject playerPrefab;
    }
}