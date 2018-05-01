using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using UnityEngine;

namespace Assets.Scripts.CityGML2GO.GmlHandlers
{
    class MaterialHandler
    {
        //private void HandleMaterial(XmlReader reader, CityGml2GO cityGml2Go)
        //{
        //    var id = "";
        //    while (reader.MoveToNextAttribute())
        //    {
        //        if (reader.LocalName == "id")
        //        {
        //            id = reader.Value;
        //        }
        //    }
        //    var targets = new List<string>();
        //    while (reader.Read())
        //    {
        //        if (reader.NodeType == XmlNodeType.Element && reader.LocalName == "target")
        //        {
        //            targets.Add(reader.ReadInnerXml());
        //        }
        //        if (reader.NodeType == XmlNodeType.EndElement && reader.LocalName == "X3DMaterial")
        //        {
        //            break;
        //        }
        //    }
        //    cityGml2Go.Materials.Add(id, targets);
        //}

        //private void ApplyMaterials(CityGml2GO cityGml2Go)
        //{
        //    var path = Path.GetFullPath(cityGml2Go.Filename);

        //    foreach (var texture in Textures)
        //    {
        //        var fn = Path.Combine(path, texture.Url);
        //        var b = File.ReadAllBytes(fn);
        //        var tex2D = new Texture2D(1, 1);
        //        tex2D.LoadImage(b);
        //        var mat = new UnityEngine.Material(DefaultMaterial) { mainTexture = tex2D };

        //        foreach (var textureTarget in texture.Targets)
        //        {
        //            if (!Polygons.Any(x => x != null && x.name == textureTarget.Id && x.activeSelf))
        //            {
        //                continue;
        //            }

        //            var go = Polygons.First(x => x != null && x.name == textureTarget.Id && x.activeSelf);
        //            var mr = go.GetComponent<MeshRenderer>();

        //            mr.material = mat;

        //            var mf = go.GetComponent<MeshFilter>();
        //            while (textureTarget.Coords.Count > mf.sharedMesh.vertices.Length)
        //            {
        //                textureTarget.Coords.RemoveAt(textureTarget.Coords.Count - 1);
        //            }

        //            if (textureTarget.Coords.Count != mf.sharedMesh.vertices.Length)
        //            {
        //                continue;
        //            }

        //            var arr2 = textureTarget.Coords;
        //            arr2.Reverse();
        //            for (int i = 0; i < arr2.Count; i++)
        //            {
        //                arr2[i] = new Vector2(arr2[i].y, arr2[i].y);
        //            }
        //            mf.sharedMesh.uv = arr2.ToArray();

        //            mf.sharedMesh.RecalculateTangents();
        //        }
        //    }
        //}

    }
}
