using UI;
using UnityEngine;
using UnityEngine.Events;
using Zlipacket.CoreZlipacket.Tools;

namespace Players.UI.ObjectDependent
{
    public class CircleTimerDisplay : ObjectDisplay<FillCircleUI>
    {
        private Timer timer;
        public bool isRunning => timer.isRunning;
        public UnityEvent<float> onTimerUpdate => timer.onTimerUpdate;

        public Sprite icon;
        
        public CircleTimerDisplay(Timer timer, Sprite icon)
        {
            this.timer = timer;
            this.icon = icon;
        }
        
        public override void ShowUI(Player player)
        {
            if (!isRunning)
                return;
            
            base.ShowUI(player);
            
            timer.onTimerUpdate.AddListener(UpdateTimerUI);
        }

        private void UpdateTimerUI(float percentage)
        {
            display.fillAmount = 1 - percentage;
        }

        public override void HideUI(Player player)
        {
            if (!isRunning)
                return;
            
            base.HideUI(player);
        }

        protected override FillCircleUI SpawnDisplay(PlayerUIManager playerUI)
        {
            FillCircleUI fillCircleUI = playerUI.SpawnUIElement("Main", "First Person", 
                Resources.Load<GameObject>(PlayerUIManager.CIRCLE_TIMER_UI_PATH), Vector3.zero, 
                "TimerUI").GetComponent<FillCircleUI>();
            
            fillCircleUI.icon.sprite = icon;
            
            return fillCircleUI;
        }
    }
}