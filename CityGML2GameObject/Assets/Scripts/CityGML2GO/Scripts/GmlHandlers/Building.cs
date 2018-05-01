using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Assets.Scripts.CityGML2GO.GmlHandlers
{
    public class BuildingHandler
    {
        public static void HandleBuilding(XmlReader reader, CityGml2GO cityGml2Go)
        {
            var buildingName = "";

            while (reader.MoveToNextAttribute())
            {
                if (reader.LocalName == "id")
                {
                    buildingName = reader.Value;
                }
            }

            var buildingGo = new GameObject(string.IsNullOrEmpty(buildingName) ? "Building" : buildingName);
            var buildingProperties = buildingGo.AddComponent<Scripts.BuildingProperties>();
            var semanticType = buildingGo.AddComponent<SemanticType>();
            buildingGo.transform.SetParent(cityGml2Go.Parent.transform);

            while (reader.Read())
            {
                if (reader.NodeType == XmlNodeType.Element && reader.LocalName == "Polygon")
                {
                    var polyGo = PolygonHandler.PolyToMesh(reader, buildingName, cityGml2Go, semanticType);
                    if (polyGo != null)
                    {
                        polyGo.transform.SetParent(buildingGo.transform);
                    }
                }

                if (cityGml2Go.ShowCurves)
                {
                    if (reader.NodeType == XmlNodeType.Element && reader.LocalName == "MultiCurve")
                    {
                        MultiCurveHandler.HandleMultiCurve(reader, buildingGo, semanticType, cityGml2Go);
                    }
                }

                if (cityGml2Go.Semantics)
                {
                    if (reader.NodeType == XmlNodeType.Element && cityGml2Go.SemanticSurfaces.Any(x => x == reader.LocalName))
                    {
                        semanticType.Name = reader.LocalName;
                        reader.MoveToFirstAttribute();
                        if (reader.LocalName == "id")
                        {
                            semanticType.Id = reader.Value;
                        }
                        else
                        {
                            while (reader.MoveToNextAttribute())
                            {
                                semanticType.Id = reader.Value;
                            }
                        }
                    }
                }

                BuildingPropertiesHandler.HandleBuildingProperties(reader, buildingProperties);

                if (reader.NodeType == XmlNodeType.EndElement && reader.LocalName == "Building")
                {
                    break;
                }
            }

            Object.Destroy(semanticType);
        }
       
    }
}
