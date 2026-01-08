using System;
using System.Collections.Generic;
using UnityEngine;

namespace Players.UI
{
    public class PlayerUIManager : MonoBehaviour
    {
        public const string CIRCLE_TIMER_UI_PATH = "UI/CircleTimerUI";
        public const string ICON_UI_PATH = "UI/IconUI";
        
        public List<UISection> uiSections;
        
        public UISection GetUISection(string sectionName) => uiSections
            .Find(s => string.Equals(s.sectionName, sectionName, StringComparison.InvariantCultureIgnoreCase));
        public Panel GetPanel(string sectionName, string panelName) => GetUISection(sectionName).GetPanel(panelName);
        
        public void SetActiveUISection(string name, bool active)
        {
            UISection uiSection = GetUISection(name);

            if (uiSection != null)
                uiSection.sectionRoot.SetActive(active);
            else
                Debug.LogError("UI section not found: " + name);
        }

        public GameObject SpawnUIElement(string sectionName, string panelName, GameObject element, Vector2 screenPosition, string layerName = "")
        {
            Panel spawnPanel = GetPanel(sectionName, panelName);

            if (spawnPanel == null)
            {
                Debug.LogError($"UI panel of {panelName} not found in section {sectionName}.");
                return null;
            }
            
            if (string.IsNullOrWhiteSpace(layerName))
                return SpawnUIElement(element, screenPosition, spawnPanel.panelRoot.transform);
            else
                return SpawnUIElement(element, screenPosition, spawnPanel.GetLayer(layerName).transform);
        }

        public GameObject SpawnUIElement(GameObject element, Vector2 screenPosition, Transform parent)
        {
            RectTransform spawnElement = Instantiate(element, parent).GetComponent<RectTransform>();
            spawnElement.localPosition = new Vector3(
                Mathf.Lerp(0, 1920, screenPosition.x), 
                Mathf.Lerp(0, 1080, screenPosition.y), 0);
            
            return spawnElement.gameObject;
        }
    }

    [Serializable]
    public class UISection
    {
        public string sectionName;
        public GameObject sectionRoot;
        public List<Panel> panels;
        
        public Panel GetPanel(string panelName) => 
            panels.Find(p => string.Equals(p.panelName, panelName, StringComparison.InvariantCultureIgnoreCase));
    }
    
    [Serializable]
    public class Panel
    {
        public string panelName;
        public GameObject panelRoot;

        public GameObject GetLayer(string layerName) => panelRoot.transform.Find(layerName).gameObject;
    }
}