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