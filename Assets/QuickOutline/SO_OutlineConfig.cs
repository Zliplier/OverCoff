using UnityEngine;

namespace QuickOutline
{
    [CreateAssetMenu(fileName = "Outline Config", menuName = "Outline Config")]
    public class SO_OutlineConfig : ScriptableObject
    {
        public Outline.Mode Mode;
        public Color Color;
        [Range(0f, 10f)] public float Width;
    }
}