using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Assets.Scripts.CityGML2GO
{
    [RequireComponent(typeof(CityGml2GO))]
    public class SemanticSurfaceMaterial : MonoBehaviour
    {
        private CityGml2GO _cityGml2Go;
        public List<Material> Materials;
        public Shader Shader;
        void Start()
        {
            Init();
        }

        void Reset()
        {
           Init();
        }

        void Init()
        {
            InitCity2Go();
            InitShader();
            InitMaterials();
        }
        void InitCity2Go()
        {
            if (_cityGml2Go == null)
            {
                _cityGml2Go = GetComponent<CityGml2GO>();
            }
        }
        void InitShader()
        {
            if (Shader == null)
            {
                Shader = Shader.Find("Standard");
            }
        }
        void InitMaterials()
        {
            if (Materials != null && Materials.Count > 0)
            {
                return;
            }

            Materials = new List<Material>();
            Debug.Log(_cityGml2Go.SemanticSurfaces.Count);
            foreach (var name in _cityGml2Go.SemanticSurfaces)
            {
                var mat = new Material(Shader)
                {
                    color = Random.ColorHSV(),
                    name = name
                };
                Materials.Add(mat);
            }
        }

        public void PaintSurfaces(GameObject target, SemanticType semanticType)
        {
            var rend = target.GetComponent<MeshRenderer>();
            if (rend != null)
            {
                var material = Materials.FirstOrDefault(x => x.name == semanticType.Name);
                if (material != null)
                {
                    rend.material = material;
                }
                else
                {
                    rend.material = _cityGml2Go.DefaultMaterial;
                }
            }

            var lr = target.GetComponent<LineRenderer>();
            if (lr != null)
            {
                var material = Materials.FirstOrDefault(x => x.name == semanticType.Name);
                if (material != null)
                {
                    lr.material = material;
                }
            }
        }
    }
}
