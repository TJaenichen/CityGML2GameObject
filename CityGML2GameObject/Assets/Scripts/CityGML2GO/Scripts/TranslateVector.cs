using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using UnityEngine;

namespace Assets.Scripts.CityGML2GO
{
    public static class TranslateVector
    {
        public static Vector3 GetTranslateVectorFromFile(FileInfo file)
        {
            Vector3 fromV3 = Vector3.zero;
            Vector3 toV3 = Vector3.zero;
            using (XmlReader reader = XmlReader.Create(file.OpenRead(), new XmlReaderSettings { IgnoreWhitespace = true }))
            {
                while (reader.Read())
                {
                    if (reader.NodeType == XmlNodeType.Element && reader.LocalName == "lowerCorner")
                    {
                        reader.Read();
                        var parts = reader.Value.Split(' ');
                        fromV3 = new Vector3((float)double.Parse(parts[0]), (float)double.Parse(parts[2]),
                            (float)double.Parse(parts[1]));
                    }

                    if (reader.NodeType == XmlNodeType.Element && reader.LocalName == "upperCorner")
                    {
                        reader.Read();
                        var parts = reader.Value.Split(' ');
                        toV3 = new Vector3((float)double.Parse(parts[0]), (float)double.Parse(parts[2]),
                            (float)double.Parse(parts[1]));
                    }

                    if (reader.NodeType == XmlNodeType.EndElement && reader.LocalName == "boundedBy")
                    {
                        break;
                    }
                }
            }

            return -((fromV3 + toV3) / 2);
        }
    }
}
