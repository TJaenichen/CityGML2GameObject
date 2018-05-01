using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using UnityEngine;

namespace Assets.Scripts.CityGML2GO.GmlHandlers
{
    public class MultiCurveHandler
    {

        public static void HandleMultiCurve(XmlReader reader, GameObject buildingGo, SemanticType semanticType, CityGml2GO cityGml2Go)
        {
            while (reader.Read())
            {
                if (reader.NodeType == XmlNodeType.Element && reader.LocalName == "curveMember")
                {
                    GameObject lrGo;
                    LineRenderer lr;

                    if (cityGml2Go.LineRendererPrefab == null)
                    {
                        lrGo = new GameObject();
                        lr = lrGo.AddComponent<LineRenderer>();
                    }
                    else
                    {
                        lrGo = GameObject.Instantiate(cityGml2Go.LineRendererPrefab);
                        lr = lrGo.GetComponent<LineRenderer>();
                        if (lr == null)
                        {
                            lr = lrGo.AddComponent<LineRenderer>();
                        }
                    }
                    var points = new List<Vector3>();
                    while (reader.Read())
                    {
                        if (reader.NodeType == XmlNodeType.Element && reader.LocalName == "pos")
                        {
                            points.Add(PositionHandler.GetPos(reader, cityGml2Go.ActualTranslate));
                        }
                        if (reader.NodeType == XmlNodeType.Element && reader.LocalName == "posList")
                        {
                            var range = PositionHandler.GetPosList(reader, cityGml2Go.ActualTranslate);
                            if (range != null)
                            {
                                points.AddRange(range);
                            }
                        }

                        if (reader.NodeType == XmlNodeType.EndElement && reader.LocalName == "curveMember")
                        {
                            break;
                        }
                    }

                    lr.positionCount = points.Count;
                    lr.SetPositions(points.ToArray());

                    lrGo.transform.SetParent(buildingGo.transform);
                    lrGo.transform.localPosition = Vector3.zero;
                    if (cityGml2Go.Semantics && semanticType != null)
                    {
                        SemanticsHandler.HandleSemantics(lrGo, semanticType, cityGml2Go);
                    }
                }

                if (reader.NodeType == XmlNodeType.EndElement && reader.LocalName == "MultiCurve")
                {
                    return;
                }
            }
        }
    }
}
