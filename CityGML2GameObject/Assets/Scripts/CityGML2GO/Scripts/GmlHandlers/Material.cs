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
    public class MaterialHandler
    {
		public static void HandleMaterial(XmlReader reader, CityGml2GO cityGml2Go)
        {
            var id = "";
            while (reader.MoveToNextAttribute())
            {
                if (reader.LocalName == "id")
                {
                    id = reader.Value;
                }
            }
            var targets = new List<string>();
            while (reader.Read())
            {
                if (reader.NodeType == XmlNodeType.Element && reader.LocalName == "target")
                {
                    targets.Add(reader.ReadInnerXml());
                }
                if (reader.NodeType == XmlNodeType.EndElement && reader.LocalName == "X3DMaterial")
                {
                    break;
                }
            }
			if (!cityGml2Go.Materials.ContainsKey(id))
            	cityGml2Go.Materials.Add(id, targets);
        }

		public static void ApplyMaterials(CityGml2GO cityGml2Go)
        {
            //var path = Path.GetFullPath(cityGml2Go.Filename);
			var path = Application.dataPath + "/StreamingAssets/test/";
			foreach (var texture in cityGml2Go.Textures)
            {
                var fn = Path.Combine(path, texture.Url);
                var b = File.ReadAllBytes(fn);
                var tex2D = new Texture2D(1, 1);
                tex2D.LoadImage(b);
				var mat = new Material(Shader.Find("Standard")) { mainTexture = tex2D };

                foreach (var textureTarget in texture.Targets)
                {
                    if (!cityGml2Go.Polygons.Any(x => x != null && x.name == textureTarget.Id && x.activeSelf))
                    {
                        continue;
                    }
                    GameObject go = cityGml2Go.Polygons.First(x => x != null && x.name == textureTarget.Id && x.activeSelf);
                    MeshRenderer mr = go.GetComponent<MeshRenderer>();

                    mr.material = mat;

                    MeshFilter mf = go.GetComponent<MeshFilter>();

                    Vector3[] vertices = mf.sharedMesh.vertices;
                    var uv = new Vector2[vertices.Length];
                    foreach (var x in cityGml2Go.oriPoly)
                    {
                        if (x.name == textureTarget.Id)
                        {
                            x.outsideUVs = textureTarget.Coords;
                            for (int i = 0; i < vertices.Length; i++)
                            {
                                uv[i] = x.ClosestUV(vertices[i]);
                            }
                        }
                    }
                    uv.Reverse();
                    mf.sharedMesh.uv = uv.ToArray();
                    mf.sharedMesh.RecalculateTangents();
                }
            }
        }

    }
}
