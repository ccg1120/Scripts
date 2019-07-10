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


    public static List<XElement> XElementList = new List<XElement>();

    public static XElement temp;
     
    public static string Include = @"Include=""..\..\";

    static void OnPostprocessAllAssets(
        string[] importedAssets, string[] deletedAssets,
        string[] movedAssets, string[] movedFromAssetPaths)
    {

        for (int index = 0; index < importedAssets.Length; index++)
        {
            Debug.Log(importedAssets[index]);
        }
        for (int index = 0; index < deletedAssets.Length; index++)
        {
            Debug.Log(deletedAssets[index]);
        }
        for (int index = 0; index < movedAssets.Length; index++)
        {
            Debug.Log(movedAssets[index]);
        }

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
        
        foreach (XNode node in csprojdata.Nodes())
        {
            Debug.Log(node.ToString());
        }

        foreach (XElement element in csprojdata.Root.Elements())
        {
            XnameList.Add(element.Name.ToString());
            Debug.Log(element.Name.ToString());

            if (element.Name.ToString().Contains("ItemGroup"))
            {
                Debug.Log(element.Name.ToString() + " : ");

                foreach (XElement elm in element.Elements())
                {


                    if (elm.Name.ToString().Contains("Compile"))
                    {
                        temp = element;
                        break;
                    }

                    
                    ComplieList.Add(elm.Name.ToString());
                    Debug.Log(elm.Value);
                    Debug.Log(elm.Attribute("Include").Value);
                    //Debug.Log(elm.Name.ToString());
                    Debug.Log(elm.FirstNode);
                    

                }
            }
        }

        Debug.Log("______________________________________________________________________________________________");
        foreach (XElement item in temp.Elements())
        {
            Debug.Log(item.Value);

            ComplieList.Add(item.Name.ToString());
            Debug.Log(item.Value);
            Debug.Log(item.Attribute("Include").Value);
            //Debug.Log(elm.Name.ToString());
            Debug.Log(item.FirstNode);
            // 속성 추가 방법 attribute 를 하면 괄호 옆에 값을 넣을 수 있음 
            //element.Add("Compile", new XAttribute("Include", ""), new XElement("Link", ""));
        }
    }

}
