using Players;
using UnityEngine;
using UnityEngine.Events;
using Zlipacket.CoreZlipacket.Tools;

namespace UI.Display
{
    public class CircleBarDisplay : ObjectDisplay<FillCircleUI>
    {
        public static Color defaultFillColor = new Color(1, 0.8661064f, 0.4858491f, 1);
        public static Color defaultBgColor = new Color(1, 1, 1, 0.2705882f);

        public Sprite icon;
        public Color fillColor = defaultFillColor.Copy();
        public Color BgColor = defaultBgColor.Copy();

        public CircleBarDisplay()
        {
            
        }
        
        public CircleBarDisplay(Sprite icon)
        {
            this.icon = icon;
        }

        public void UpdateUI(float percentage)
        {
            if (!isUIShown)
                return;
            display.fillAmount = percentage;
        }

        protected override FillCircleUI SpawnDisplay(UIManager ui)
        {
            if (ui == null)
                return null;
            
            FillCircleUI fillCircleUI = ui.SpawnUIElement("Main", "First Person", 
                Resources.Load<GameObject>(UIManager.CIRCLE_TIMER_UI_PATH), Vector3.zero, 
                "TimerUI").GetComponent<FillCircleUI>();
            
            fillCircleUI.icon.sprite = icon;
            fillCircleUI.fill.color = fillColor;
            
            return fillCircleUI;
        }
    }
}