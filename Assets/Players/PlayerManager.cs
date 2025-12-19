using System;
using System.Collections.Generic;
using Items;
using Players.PlayerConfig;
using UnityEngine;
using Zlipacket.CoreZlipacket.Tools;

namespace Players
{
    public class PlayerManager : Singleton<PlayerManager>
    {
        [SerializeField] private List<Player> Players;

        public Player GetPlayer(string playerID)
        {
            return Players.Find(x => string.Equals(x.playerID, playerID, StringComparison.InvariantCultureIgnoreCase));
        }
        
        public Player GetPlayer(int playerNumber)
        {
            return Players[playerNumber];
        }
    }
}