using System;
using TMPro;
using UnityEngine;

namespace Items.Data
{
    [Serializable]
    public class ItemDisplayData
    {
        public string displayName;
        public TMP_FontAsset textFont;
        public Color textColor;
    }
}