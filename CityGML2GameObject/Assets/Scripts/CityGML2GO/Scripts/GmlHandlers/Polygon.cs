using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using UnityEngine;

namespace Assets.Scripts.CityGML2GO.GmlHandlers
{
    public class PolygonHandler
    {
        public static GameObject PolyToMesh(XmlReader reader, string id, CityGml2GO cityGml2Go, SemanticType semanticType = null)
        {
            var extPositions = new List<Vector3>();
            var intPositions = new List<List<Vector3>>();
            var polyName = "";
            while (reader.MoveToNextAttribute())
            {
                if (reader.LocalName == "id")
                {
                    polyName = reader.Value;
                }
            }

            while (reader.Read())
            {
                if (((IXmlLineInfo)reader).LineNumber >= 14459)
                {
                    int idfg = 0;
                }
                if (string.IsNullOrEmpty(polyName))
                {
                    while (reader.MoveToNextAttribute())
                    {
                        if (reader.LocalName == "id")
                        {
                            polyName = reader.Value;
                        }
                    }
                }

                if (reader.NodeType == XmlNodeType.Element && reader.LocalName == "exterior")
                {

                    while (reader.Read())
                    {
                        if (reader.NodeType == XmlNodeType.Element && reader.LocalName == "posList")
                        {
                            var range = PositionHandler.GetPosList(reader, cityGml2Go.ActualTranslate);
                            if (range != null)
                            {
                                extPositions.AddRange(range);
                            }

                        }

                        if (reader.NodeType == XmlNodeType.Element && reader.LocalName == "pos")
                        {
                            extPositions.Add(PositionHandler.GetPos(reader, cityGml2Go.ActualTranslate));
                        }

                        if (reader.NodeType == XmlNodeType.EndElement && reader.LocalName == "exterior")
                        {
                            break;
                        }
                    }
                }

                if (reader.NodeType == XmlNodeType.Element && reader.LocalName == "interior")
                {
                    var curInt = new List<Vector3>();
                    while (reader.Read())
                    {
                        if (reader.NodeType == XmlNodeType.Element && reader.LocalName == "posList")
                        {
                            var range = PositionHandler.GetPosList(reader, cityGml2Go.ActualTranslate);
                            if (range != null)
                            {
                                curInt.AddRange(range);
                            }
                        }

                        if (reader.NodeType == XmlNodeType.Element && reader.LocalName == "pos")
                        {
                            curInt.Add(PositionHandler.GetPos(reader, cityGml2Go.ActualTranslate));
                        }
                        if (reader.NodeType == XmlNodeType.EndElement && reader.LocalName == "interior")
                        {
                            break;
                        }
                    }
                    intPositions.Add(curInt);
                }
                if (reader.NodeType == XmlNodeType.EndElement && reader.LocalName == "Polygon")
                {
                    break;
                }
            }

            //var lastPoint = extPositions.Last();
            //extPositions = extPositions.Distinct().ToList();
            //extPositions.Add(lastPoint);

//            for (var index = 0; index < intPositions.Count; index++)
//            {
//                lastPoint = intPositions[index].Last();
//                //intPositions[index] = intPositions[index].Distinct().ToList();
//                intPositions[index].Add(lastPoint);
//            }


            if (!PolygonHandler.IsPolyValid(extPositions))
            {
                IXmlLineInfo xmlInfo = (IXmlLineInfo)reader;
                int lineNumber = xmlInfo.LineNumber;
                Debug.Log(lineNumber);
                return null;
            }

            foreach (var intRing in intPositions)
            {
                if (!IsPolyValid(intRing))
                {
                    IXmlLineInfo xmlInfo = (IXmlLineInfo)reader;
                    int lineNumber = xmlInfo.LineNumber;
                    Debug.Log(lineNumber);
                    return null;
                }
            }

            var polygon = new Poly2Mesh.Polygon();

			polygon.oriExt = extPositions.ConvertAll(ext => new Vector3(ext.x,ext.y, ext.z));
            extPositions.Reverse();
            intPositions.Reverse();

            polygon.outside = extPositions;
            polygon.holes = intPositions;

            polygon.name = polyName;
            cityGml2Go.oriPoly.Add(polygon);
            GameObject go = null;

            try
            {
                go = Poly2Mesh.CreateGameObject(polygon, string.IsNullOrEmpty(polyName) ? id : polyName);
            }
            catch (Exception ex)
            {
                Debug.Log(ex);
            }

            if (cityGml2Go.GenerateColliders)
            {
                //go.AddComponent<MeshCollider>();
            }

            if (go != null)
            {
                if (cityGml2Go.Semantics && semanticType != null)
                {
                    SemanticsHandler.HandleSemantics(go, semanticType, cityGml2Go);
                }
                else if (cityGml2Go.DefaultMaterial!= null)
                {
                    var mr = go.GetComponent<MeshRenderer>();
                    if (mr != null)
                    {
                        mr.sharedMaterial = cityGml2Go.DefaultMaterial;
                    }
                }

            }

            cityGml2Go.Polygons.Add(go);

            return go;
        }
        public static bool IsPolyValid(List<Vector3> poly)
        {
            if (poly.First() != poly.Last())
            {
                Debug.LogWarning("First != Last");
                return false;
            }

            if (poly.Count < 4)
            {
                Debug.LogWarning("Count < 4");
                return false;
            }

            return true;
        }

        //void CombineMeshes()
        //{
        //    var ids = Polygons.Where(x => x != null).Select(x => x.name).Distinct();
        //    foreach (var id in ids)
        //    {
        //        var allWithId = Polygons.Where(x => x != null && x.name == id).ToList();
        //        if (allWithId.Count() < 2)
        //        {
        //            continue;
        //        }
        //        var first = allWithId.First();
        //        var combines = new List<CombineInstance>();
        //        var arr = allWithId.ToArray();
        //        for (int i = 1; i < allWithId.Count(); i++)
        //        {
        //            var combine = new CombineInstance();
        //            combine.mesh = arr[i].GetComponent<MeshFilter>().sharedMesh;
        //            combine.transform = arr[i].transform.localToWorldMatrix;
        //            arr[i].gameObject.SetActive(false);
        //            combines.Add(combine);
        //        }
        //        first.GetComponent<MeshFilter>().sharedMesh.CombineMeshes(combines.ToArray());

        //    }
        //}
    }
}
