using System;
using System.Collections.Generic;
using DG.Tweening;
using Items.Data;
using Players.UI;
using UnityEngine;
using UnityEngine.UI;
using Zlipacket.CoreZlipacket.UI;

namespace UI
{
    public class CupIngredientUI : MonoBehaviour
    {
        public Billboard canvasUI;
        
        private Tween displayAnimation = null;
        public bool isTweening => displayAnimation != null;

        public Tween OnShow()
        {
            canvasUI.Show();
            
            if (isTweening)
                displayAnimation.Kill();
            
            transform.localScale = new Vector3(0.5f, 0.5f, 0.5f);
            displayAnimation = transform.DOScale(1f, 0.1f);
            return displayAnimation;
        }

        public Tween OnHide()
        {
            canvasUI.Hide();
            
            if (isTweening)
                displayAnimation.Kill();

            displayAnimation = transform.DOScale(0.5f, 0.1f);
            return displayAnimation;
        }

        public void AddItemUI(SO_Item ingredient)
        {
            IconUI newIcon = Instantiate(Resources.Load<GameObject>(PlayerUIManager.ICON_UI_PATH)).GetComponent<IconUI>();

            newIcon.transform.SetParent(transform);
            newIcon.transform.localPosition = Vector3.zero;
            newIcon.transform.localRotation = Quaternion.identity;
            newIcon.transform.localScale = Vector3.one;
            newIcon.name = "IconUI";
            newIcon.icon.sprite = ingredient.icon;
        }
    }
}