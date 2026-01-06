using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class FillCircleUI : MonoBehaviour
    {
        public Image icon;
        public Image fill;
        public Image bg;

        public float fillAmount
        {
            get { return fill.fillAmount; }
            set { fill.fillAmount = Mathf.Clamp(value, 0f, 1f); }
        }

        public Color color
        {
            get { return fill.color; }
            set { fill.color = value; }
        }
    }
}