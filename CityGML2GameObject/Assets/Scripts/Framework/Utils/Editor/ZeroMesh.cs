using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

public class ZeroMesh : Editor {
    [MenuItem("Framework/Utils/Zero Mesh")]
    public static void ZeroSelectedMesh()
    {
        var gos = Selection.gameObjects;
        var mfs = new List<MeshFilter>();
        foreach (var gameObject in gos)
        {
          

            var mf = gameObject.GetComponent<MeshFilter>();
            if (mf != null)
            {
                mfs.Add(mf);
            }
            mfs.AddRange(gameObject.GetComponentsInChildren<MeshFilter>());
           
        }
        var zero = Vector3.zero;
        var count = 0;
        foreach (var meshFilter in mfs)
        {
            foreach (var sharedMeshVertex in meshFilter.sharedMesh.vertices)
            {
                zero += sharedMeshVertex;
            }

            count += meshFilter.sharedMesh.vertices.Length;
        }

        zero = zero / count;

        foreach (var meshFilter in mfs)
        {
            var vertices = meshFilter.sharedMesh.vertices;
            
            var newVerts = new List<Vector3>();
            foreach (var vector3 in vertices)
            {
                if (Random.Range(0, 100) == 0)
                {
                    Debug.Log(vector3);
                    Debug.Log(vector3 - zero);
                }
                newVerts.Add(vector3 - zero);

            }
            var newMesh = new Mesh();
            newMesh.SetVertices(newVerts);
            newMesh.SetTriangles(meshFilter.sharedMesh.triangles, 0);
            newMesh.SetNormals( meshFilter.sharedMesh.normals.ToList());
            newMesh.SetVertices(newVerts);
            newMesh.RecalculateNormals();
            newMesh.RecalculateBounds();
            newMesh.RecalculateTangents();

            AssetDatabase.CreateAsset(newMesh, "Assets/Models/Berlin/" + meshFilter.name);
            AssetDatabase.SaveAssets();

            var go = new GameObject(meshFilter.name);
            var newMf = go.AddComponent<MeshFilter>();
            newMf.sharedMesh = newMesh;
            go.AddComponent<MeshRenderer>();

            var prefab = EditorUtility.CreateEmptyPrefab("Assets/Prefabs/Berlin/" + meshFilter.name + ".prefab");
            UnityEditor.PrefabUtility.ReplacePrefab(go, prefab, ReplacePrefabOptions.ReplaceNameBased);
        }
    }
}
