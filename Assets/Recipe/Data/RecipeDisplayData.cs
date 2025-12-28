using System;
using TMPro;
using UnityEngine;

namespace Recipe.Data
{
    [Serializable]
    public class RecipeDisplayData
    {
        public string displayName;
        public TMP_FontAsset textFont;
        public Color textColor;
    }
}