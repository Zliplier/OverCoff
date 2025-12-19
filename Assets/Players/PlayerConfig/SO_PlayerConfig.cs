using UnityEngine;
using Zlipacket.CoreZlipacket.Audio;

namespace Players.PlayerConfig
{
    [CreateAssetMenu(fileName = "Player Config", menuName = "Players/PlayerConfig")]
    public class SO_PlayerConfig : ScriptableObject
    {
        public string playerID;
        public SO_AudioConfig audioConfig;
    }
}