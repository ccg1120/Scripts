using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class Export
{
    [MenuItem("xx/xxxxxxxx")]
    static void Export()
    {
        string browserCore = "xxx/xxxxxxxxx";
        string trealScriptFolder = "xxxxxxxx/xxxxxxxxxxx";

        var assets = new string[] { browserCore, trealScriptFolder };

        var export = EditorUtility.DisplayDialog("xxxxxxxx", "xxxxxxx?", "xxxxxxxx", "xxxxxxxxxxx");
        if (export)
        {
            AssetDatabase.ExportPackage(assets, "xxxxxx/xxxxxxxx.xxxxxxxx", ExportPackageOptions.Recurse);
            Debug.Log("Export Complete");
        }
    }
}