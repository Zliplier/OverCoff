using System;
using System.Collections.Generic;
using UnityEngine;
using Zlipacket.CoreZlipacket.Tools;
using Random = UnityEngine.Random;

namespace Characters.Customer.Waypoint
{
    public class ChairManager : Singleton<ChairManager>
    {
        public List<Chair> chairs;
        
        public Chair RequestChair()
        {
            List<Chair> validChairs = new();

            foreach (var chair in chairs)
            {
                if (chair.occupant == null)
                {
                    validChairs.Add(chair);
                }
            }

            if (validChairs.Count > 0)
            {
                return validChairs[Random.Range(0, validChairs.Count)];
            }
            
            return null;
        }
    }
}