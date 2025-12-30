using System;
using System.Collections.Generic;
using Items;
using UnityEngine;
using Zlipacket.CoreZlipacket.Tools;

namespace Players
{
    public class PlayerManager : Singleton<PlayerManager>
    {
        [SerializeField] private List<Player> Players;
        
        public Player GetPlayer(string playerID)
        {
            return Players.Find(x => string.Equals(x.player.playerData.playerID, playerID, StringComparison.InvariantCultureIgnoreCase));
        }
    }
}