using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Players.UI
{
    public class UIManager : MonoBehaviour
    {
        public List<UISection> uiSections;
        
        public UISection GetUISection(string name) => uiSections.Find(s => string.Equals(s.sectionName, name, StringComparison.InvariantCultureIgnoreCase));

        public void SetActiveUISection(string name, bool active)
        {
            UISection uiSection = GetUISection(name);

            if (uiSection != null)
                uiSection.sectionRoot.SetActive(active);
            else
                Debug.LogError("UI section not found: " + name);
        }
        
        
        
        
    }

    [Serializable]
    public class UISection
    {
        public string sectionName;
        public GameObject sectionRoot;
        public List<Panel> panels;
    }
    
    [Serializable]
    public class Panel
    {
        public string panelName;
        public GameObject panelRoot;
    }
}