using System;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.CityGML2GO.Scripts
{
    
    public class BuildingProperties : MonoBehaviour
    {
        [Serializable]
        public class BuildingProperty
        {
            public string Key;
            public string Value;

            public BuildingProperty(string key, string value)
            {
                Key = key;
                Value = value;
            }
        }
        
        public List<BuildingProperty> Properties = new List<BuildingProperty>();
        public String PropertyList = "";

    }
}
