using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Xml.Linq;
using System.IO;
using System.Linq;
using UnityEditor;


public class Parser : AssetPostprocessor
{
    public static List<string> XnameList = new List<string>();
    public static List<string> ComplieList = new List<string>();

    static void OnPostprocessAllAssets(
        string[] importedAssets, string[] deletedAssets,
        string[] movedAssets, string[] movedFromAssetPaths)
    {
        GetCSPROJData(Application.dataPath + "/Data/Kids.csproj");

        if(importedAssets.Length != 0)
        {
            ImportAssetChecker(importedAssets);
        }

    }

    private static void ImportAssetChecker(string[] array)
    {
        var directofry = array.Where(im => im.EndsWith(".cs"))
           .GroupBy(ia => Path.GetDirectoryName(ia))
           .Select(g => g.Key);


    }


    private static void GetCSPROJData(string path)
    {
        XDocument csprojdata = XDocument.Load(path);

        Debug.Log(csprojdata);

        foreach (XElement element in csprojdata.Root.Elements())
        {
            XnameList.Add(element.Name.ToString());
            Debug.Log(element.Name.ToString());

            if (element.Name.ToString().Contains("ItemGroup"))
            {
                Debug.Log(element.Name.ToString() + " : ");
                foreach (XElement elm in element.Elements())
                {
                    if (!elm.Name.ToString().Contains("Compile")) break;
                    ComplieList.Add(elm.Name.ToString());
                    Debug.Log(elm.Name.ToString());
                }
            }
        }
    }

}
