using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Players.PlayerScripts
{
    public class PlayerStatusDisplay : PlayerScript
    {
        //For Group Control and Fade In/Out
        public CanvasGroup root;
        
        public TextMeshProUGUI moneyText;
        public Slider healthSlider;
        public Slider staminaSlider;

        private void Start()
        {
            player.onHealthChanged.AddListener(UpdateHealthBar);
            player.onStaminaChanged.AddListener(UpdateStaminaBar);
            player.onMoneyChanged.AddListener(UpdateMoneyText);
            
            player.onHealthFull.AddListener(arg0 => PlayBounceAnimation(healthSlider.transform));
            player.onStaminaFull.AddListener(arg0 => PlayBounceAnimation(staminaSlider.transform));
            player.onStaminaFull.AddListener(arg0 => PlayBounceAnimation(moneyText.transform));
            
            UpdateHealthBar(player.health, player.maxHealth);
            UpdateStaminaBar(player.stamina, player.maxStamina);
            UpdateMoneyText(player.money);
        }

        private void UpdateHealthBar(float health, float maxHealth) => healthSlider.value = health / maxHealth;
        private void UpdateStaminaBar(float stamina, float maxStamina) => staminaSlider.value = stamina / maxStamina;

        private void UpdateMoneyText(int value) => moneyText.SetText(value.ToString("N0"));
        
        private void PlayBounceAnimation(Transform target, float duration = 0.3f)
        {
            target.DOScale(1.1f, duration / 2).onComplete += () => 
                target.DOScale(1f, duration / 2);
        }
    }
}