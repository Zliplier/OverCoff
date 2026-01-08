using System;
using System.Collections.Generic;
using DG.Tweening;
using Items.Data;
using Players.UI;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class CupIngredientUI : MonoBehaviour
    {
        private Tween displayAnimation = null;
        public bool isTweening => displayAnimation != null;

        public Tween OnShow()
        {
            if (isTweening)
                displayAnimation.Kill();
            
            transform.localScale = new Vector3(0.5f, 0.5f, 0.5f);
            displayAnimation = transform.DOScale(1f, 0.4f);
            return displayAnimation;
        }

        public Tween OnHide()
        {
            if (isTweening)
                displayAnimation.Kill();

            displayAnimation = transform.DOScale(0.5f, 0.4f);
            return displayAnimation;
        }

        public void AddItemUI(SO_Item ingredient)
        {
            IconUI newIcon = Instantiate(Resources.Load<GameObject>(PlayerUIManager.ICON_UI_PATH)).GetComponent<IconUI>();

            newIcon.name = "IconUI";
            newIcon.transform.SetParent(transform);
            newIcon.icon.sprite = ingredient.icon;
        }
    }
}