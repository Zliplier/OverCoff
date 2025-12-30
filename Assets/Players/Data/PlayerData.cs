using UnityEngine;

namespace Players.Data
{
    [System.Serializable]
    public class PlayerData
    {
        public string playerID;
        
        public Transform playerTransform;
        public Transform cameraTransform;
        
        public float health;
        public float maxHealth = 100f;
        
        public float stamina;
        public float maxStamina = 100f;

        public int money;

        public PlayerData(PlayerData playerData)
        {
            playerID = playerData.playerID;
            playerTransform = playerData.playerTransform;
            cameraTransform = playerData.cameraTransform;
            health = playerData.health;
            maxHealth = playerData.maxHealth;
            stamina = playerData.stamina;
            maxStamina = playerData.maxStamina;
            
            money = playerData.money;
        }
    }
}