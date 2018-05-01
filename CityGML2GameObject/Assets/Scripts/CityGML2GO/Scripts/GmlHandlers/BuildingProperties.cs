using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using UnityEngine;

namespace Assets.Scripts.CityGML2GO.GmlHandlers
{
    public class BuildingPropertiesHandler 
    {
        public static void HandleBuildingProperties(XmlReader reader, Scripts.BuildingProperties buildingProperties)
        {
            var propertyNames = new[]{"class",
                "function",
                "usage",
                "yearOfConstruction",
                "yearOfDemolition",
                "roofType",
                "measuredHeight",
                "storeysAboveGround",
                "storeysBelowGround",
                "storeyHeightsAboveGround",
                "storeyHeightsBelowGround",
                "outerBuildingInstallation",
                "interiorBuildingInstallation",
                //"boundedBy", is being handled by the obj builder
                "interiorRoom",
                "consistsOfBuildingPart",
                "address"
            };
            var name = reader.LocalName;
            if (propertyNames.Any(x => x == name))
            {
                var val = reader.Value;
                if (string.IsNullOrEmpty(val))
                {
                    val = reader.ReadInnerXml();
                }
                buildingProperties.Properties.Add(new Scripts.BuildingProperties.BuildingProperty(name, val));
            }
        }
    }
}
