using UnityEngine;
using UnityEngine.UI;

namespace Players.UI
{
    public class StatusDisplay : PlayerScript
    {
        //For Group Control and Fade In/Out
        public CanvasGroup root;
        
        public Slider healthSlider;
        public Slider staminaSlider;

        private void Start()
        {
            player.onHealthChanged.AddListener(UpdateHealthBar);
            player.onStaminaChanged.AddListener(UpdateStaminaBar);
            
            UpdateHealthBar(player.health, player.maxHealth);
            UpdateStaminaBar(player.stamina, player.maxStamina);
        }

        private void UpdateHealthBar(float health, float maxHealth) => healthSlider.value = health / maxHealth;
        private void UpdateStaminaBar(float stamina, float maxStamina) => staminaSlider.value = stamina / maxStamina;
    }
}