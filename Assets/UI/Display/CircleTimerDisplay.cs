using Players;
using Players.UI;
using UnityEngine;
using UnityEngine.Events;
using Zlipacket.CoreZlipacket.Tools;

namespace UI.Display
{
    public class CircleTimerDisplay : ObjectDisplay<FillCircleUI>
    {
        public static Color defaultFillColor = new Color(1, 0.8661064f, 0.4858491f, 1);
        public static Color defaultBgColor = new Color(1, 1, 1, 0.2705882f);
        
        private Timer timer;
        public bool isRunning => timer.isRunning;
        public UnityEvent<float> onTimerUpdate => timer.onTimerUpdate;

        public Sprite icon;
        public Color fillColor = defaultFillColor.Copy();
        public Color BgColor = defaultBgColor.Copy();
        
        public CircleTimerDisplay(Timer timer, Sprite icon)
        {
            this.timer = timer;
            this.icon = icon;
        }
        
        public CircleTimerDisplay(Timer timer)
        {
            this.timer = timer;
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
            if (!isUIShown)
                return;
            display.fillAmount = 1 - percentage;
        }

        public override void HideUI(Player player)
        {
            if (!isRunning)
                return;
            
            base.HideUI(player);
            timer.onTimerUpdate.RemoveListener(UpdateTimerUI);
        }

        protected override FillCircleUI SpawnDisplay(PlayerUIManager playerUI)
        {
            if (playerUI == null)
                return null;
            
            FillCircleUI fillCircleUI = playerUI.SpawnUIElement("Main", "First Person", 
                Resources.Load<GameObject>(PlayerUIManager.CIRCLE_TIMER_UI_PATH), Vector3.zero, 
                "TimerUI").GetComponent<FillCircleUI>();
            
            fillCircleUI.icon.sprite = icon;
            fillCircleUI.fill.color = fillColor;
            
            return fillCircleUI;
        }
    }
}