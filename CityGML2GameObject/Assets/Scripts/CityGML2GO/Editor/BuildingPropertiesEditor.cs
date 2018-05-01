using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Assets.Scripts.CityGML2GO;
using Assets.Scripts.CityGML2GO.Scripts;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(BuildingProperties))]
public class BuildingPropertiesEditor : Editor {
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        var sb = new StringBuilder();
        foreach (var o in this.targets)
        {
            var props = (BuildingProperties) o;
            foreach (var buildingProperty in props.Properties)
            {
                sb.AppendFormat("{0}: {1}\r\n", buildingProperty.Key, buildingProperty.Value);
            }
        }
        GUILayout.Label(sb.ToString().Take(255).ToString());
    }
}
