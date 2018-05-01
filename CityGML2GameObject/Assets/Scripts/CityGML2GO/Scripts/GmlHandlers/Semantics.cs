using UnityEngine;

namespace Assets.Scripts.CityGML2GO.GmlHandlers
{
    public class SemanticsHandler
    {
        public static void HandleSemantics(GameObject go, SemanticType semanticType, CityGml2GO cityGml2Go)
        {
            var stAdded = go.AddComponent<SemanticType>();
            stAdded.Id = semanticType.Id;
            stAdded.Name = semanticType.Name;

            cityGml2Go.SemanticSurfMat.PaintSurfaces(go, semanticType);
        }
    }
}
