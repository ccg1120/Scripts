using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Xml.Linq;
using System.IO;

public class Parser : MonoBehaviour {
    public string data;
    public List<string> XnameList = new List<string>();
    private void Start()
    {
        XDocument csprojdata = XDocument.Load(Application.dataPath + "/Data/Kids.csproj");

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
                    Debug.Log(elm.Name.ToString());
                }
            }
        }
    }

}
