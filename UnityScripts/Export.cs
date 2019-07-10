using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class Export
{
    [MenuItem("T real/Export")]
    static void Export()
    {
        string browserCore = "Assets/BrowserCore";
        string trealScriptFolder = "Assets/TrealScript";

        var assets = new string[] { browserCore, trealScriptFolder };

        var export = EditorUtility.DisplayDialog("Export", "Export?", "Export", "Cancel");
        if (export)
        {
            AssetDatabase.ExportPackage(assets, "Assets/TrealBrowserCore.unitypackage", ExportPackageOptions.Recurse);
            Debug.Log("Export Complete");
        }
    }
}