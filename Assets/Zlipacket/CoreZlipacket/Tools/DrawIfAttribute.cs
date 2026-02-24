using UnityEngine;

// Attribute to draw a field conditionally in the inspector
namespace Zlipacket.CoreZlipacket.Tools
{
    public class DrawIfAttribute : PropertyAttribute
    {
        public string comparedPropertyName { get; private set; }
        // You can add more logic here to handle different comparison types (e.g., int, enum)

        public DrawIfAttribute(string comparedPropertyName)
        {
            this.comparedPropertyName = comparedPropertyName;
        }
    }
}