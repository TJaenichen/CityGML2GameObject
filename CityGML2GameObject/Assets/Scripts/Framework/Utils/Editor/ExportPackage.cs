using System.Collections.Generic;
using UnityEditor;

public class ExportPackage : Editor {
    [MenuItem("Framework/Export Package")]
    public static void Export()
    {
        var fileNames = new List<string>();
        fileNames.Add("Assets/Scripts/Framework");
        fileNames.Add("Assets/Plugins/RainbowFolders");
        
        AssetDatabase.ExportPackage(fileNames.ToArray(), "Framework.unitypackage", ExportPackageOptions.Recurse);

    }
}
