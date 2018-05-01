using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Xml;
using Assets.Scripts.CityGML2GO.GmlHandlers;
using Framework.Variables;
using UnityEngine;
using Debug = UnityEngine.Debug;
using Material = UnityEngine.Material;

namespace Assets.Scripts.CityGML2GO
{
    public partial class CityGml2GO : MonoBehaviour {
        [LabelOverride("File-/Directory Name")]
        public string Filename;
        public bool StreamingAssets;
        public Material DefaultMaterial;
        public GameObject Parent;
        [LabelOverride("Apply automatic or manual translation")]
        public bool ApplyTranslation;
        [HideInInspector]
        public Vector3 ActualTranslate;
        [LabelOverride("Manual translation (Set to 0,0,0 for automatic)")]
        public Vector3 Translate = Vector3.zero;
        public bool ShowDebug;
        public float UpdateRate;
        public bool ShowCurves;
        public bool Semantics;
        public float CurveThickness;
        public GameObject LineRendererPrefab;
        public bool GenerateColliders;
        public List<string> SemanticSurfaces = new List<string>{ "GroundSurface", "WallSurface", "RoofSurface", "ClosureSurface", "CeilingSurface", "InteriorWallSurface", "FloorSurface", "OuterCeilingSurface", "OuterFloorSurface", "Door", "Window" };
        
        public List<GameObject> Polygons = new List<GameObject>();
        public Dictionary<string, List<string>> Materials = new Dictionary<string, List<string>>();
        public List<UnityEngine.Texture> Textures = new List<UnityEngine.Texture>();
        [HideInInspector]
        public SemanticSurfaceMaterial SemanticSurfMat;

        void Start()
        {
            SemanticSurfMat = GetComponent<SemanticSurfaceMaterial>();
        }

        /// <summary>
        /// Check if K key is pressed, if so, run the import.
        /// Could be any other input etc.
        /// </summary>
        void Update()
        {
            if (Input.GetKeyDown(KeyCode.K))
            {
                var fn = "";
                if (StreamingAssets)
                {
                    fn = Path.Combine(Application.streamingAssetsPath, Filename);
                }
                else
                {
                    fn = Filename;
                }
                Polygons = new List<GameObject>();
                var attributes = File.GetAttributes(fn);
                if ((attributes & FileAttributes.Directory) == FileAttributes.Directory)
                {
                    SetTranslate(new DirectoryInfo(fn));
                    StartCoroutine("RunDirectory", fn);
                }
                else
                {
                    SetTranslate(new FileInfo(fn));
                    StartCoroutine("Run", fn);
                }
            }
        }

        /// <summary>
        /// As the values of GML are way outside of unitys range, you should apply a global translate vector to it.
        /// SetTranslate tries to calculate that vector.
        /// </summary>
        /// <param name="file"></param>
        void SetTranslate(FileInfo file)
        {
            if (!ApplyTranslation)
            {
                ActualTranslate = Vector3.zero;
                return;
            }
            
            ActualTranslate = Translate == Vector3.zero ? TranslateVector.GetTranslateVectorFromFile(file) : Translate;
        }

        /// <summary>
        ///         /// As the values of GML are way outside of unitys range, you should apply a global translate vector to it.
        /// SetTranslate tries to calculate that vector.
        /// </summary>
        /// <param name="directory"></param>
        void SetTranslate(DirectoryInfo directory)
        {
            if (!ApplyTranslation)
            {
                ActualTranslate = Vector3.zero;
                return;
            }

            if (Translate != Vector3.zero)
            {
                ActualTranslate = Translate;
                return;
            }

            Vector3 translate = Vector3.zero;
            var count = 0;
            foreach (var fileInfo in directory.GetFiles("*.gml"))
            {
                count++;
                translate += TranslateVector.GetTranslateVectorFromFile(fileInfo);
            }

            ActualTranslate = translate / count;
        }
       
        /// <summary>
        /// Proccesses all GML files ina directory
        /// </summary>
        /// <param name="directoryName"></param>
        /// <returns></returns>
        IEnumerator RunDirectory(string directoryName)
        {
            foreach (var gml in Directory.GetFiles(directoryName, "*.gml"))
            {
                Polygons = new List<GameObject>();
                Materials = new Dictionary<string, List<string>>();
                Textures = new List<UnityEngine.Texture>();
                yield return Run(Path.Combine(directoryName, gml));
            }
        }

        /// <summary>
        /// Processes a single file
        /// </summary>
        /// <param name="fileName"></param>
        /// <returns></returns>
        IEnumerator Run(string fileName)
        {
            var counter = 0;
            var sw = new Stopwatch();
            sw.Start();

            var lastFrame = sw.ElapsedMilliseconds;
            using (XmlReader reader = XmlReader.Create(fileName, new XmlReaderSettings {IgnoreWhitespace = true})) 
            {
                yield return null;

                while (!reader.EOF)
                {
                    reader.Read();
                    if (reader.LocalName == "CityModel")
                    {
                        break;
                    }
                }

                var version = 0;
                for (int i = 0; i < reader.AttributeCount; i++)
                {
                    var attr = reader.GetAttribute(i);
                    if (attr == "http://www.opengis.net/citygml/1.0")
                    {
                        version = 1;
                        break;
                    }
                    if (attr == "http://www.opengis.net/citygml/2.0")
                    {
                        version = 2;
                        break;
                    }
                }
            
                if (version == 0)
                {
                    Debug.LogWarning("Possibly invalid xml. Check for xml:ns citygml version.");
                }
            
                while (reader.Read())
                {
                    if (reader.NodeType == XmlNodeType.Element && reader.LocalName == "cityObjectMember")
                    {
                        while (reader.Read())
                        {
                            if (reader.NodeType == XmlNodeType.Element && reader.LocalName == "Building")
                            {
                                counter++;

                                if (UpdateRate > 0 && sw.ElapsedMilliseconds > lastFrame + UpdateRate) 
                                {
                                    lastFrame = sw.ElapsedMilliseconds;
                                    yield return null;
                                }
                                BuildingHandler.HandleBuilding(reader, this);
                            }

                            if (reader.NodeType == XmlNodeType.Element && reader.LocalName == "X3DMaterial")
                            {
                                //HandleMaterial(reader);
                            }

                            if (reader.NodeType == XmlNodeType.Element && reader.LocalName == "ParameterizedTexture")
                            {
                                //HandleTexture(reader);
                            }
                        }
                    }
                }
            }
            //CombineMeshes();
            //ApplyMaterials();

            yield return null;
        }
    }
}
