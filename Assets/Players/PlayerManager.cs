using System;
using System.Collections.Generic;
using Items;
using Players.Data;
using UnityEngine;
using Zlipacket.CoreZlipacket.Tools;

namespace Players
{
    public class PlayerManager : Singleton<PlayerManager>
    {
        [SerializeField] private List<SO_Player> Players;
        
        public SO_Player GetPlayer(string playerID)
        {
            return Players.Find(x => string.Equals(x.playerData.playerID, playerID, StringComparison.InvariantCultureIgnoreCase));
        }
    }
}