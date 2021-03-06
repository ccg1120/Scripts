﻿
using UnityEngine;

using System;
using Microsoft.Win32;

public class FrameWorkVersionCheck : MonoBehaviour {

	// Use this for initialization
	void Start () {
        GetVersionFromRegistry();


        Get45or451FromRegistry();
    }
	
	// Update is called once per frame
	void Update () {
		
	}
    private static void GetVersionFromRegistry()
    {
        Debug.Log("start");
        // Opens the registry key for the .NET Framework entry.
        using (RegistryKey ndpKey =
            RegistryKey.OpenRemoteBaseKey(RegistryHive.LocalMachine, "").
            OpenSubKey(@"SOFTWARE\Microsoft\NET Framework Setup\NDP\"))
        {
            // As an alternative, if you know the computers you will query are running .NET Framework 4.5 
            // or later, you can use:
            // using (RegistryKey ndpKey = RegistryKey.OpenBaseKey(RegistryHive.LocalMachine, 
            // RegistryView.Registry32).OpenSubKey(@"SOFTWARE\Microsoft\NET Framework Setup\NDP\"))
            foreach (string versionKeyName in ndpKey.GetSubKeyNames())
            {
                if (versionKeyName.StartsWith("v"))
                {

                    RegistryKey versionKey = ndpKey.OpenSubKey(versionKeyName);
                    string name = (string)versionKey.GetValue("Version", "");
                    string sp = versionKey.GetValue("SP", "").ToString();
                    string install = versionKey.GetValue("Install", "").ToString();
                    if (install == "") //no install info, must be later.
                        Debug.Log(versionKeyName + "  " + name);
                    else
                    {
                        if (sp != "" && install == "1")
                        {
                            Debug.Log(versionKeyName + "  " + name + "  SP" + sp);
                        }

                    }
                    if (name != "")
                    {
                        continue;
                    }
                    foreach (string subKeyName in versionKey.GetSubKeyNames())
                    {
                        RegistryKey subKey = versionKey.OpenSubKey(subKeyName);
                        name = (string)subKey.GetValue("Version", "");
                        if (name != "")
                            sp = subKey.GetValue("SP", "").ToString();
                        install = subKey.GetValue("Install", "").ToString();
                        if (install == "") //no install info, must be later.
                            Debug.Log(versionKeyName + "  " + name);
                        else
                        {
                            if (sp != "" && install == "1")
                            {
                                Debug.Log("  " + subKeyName + "  " + name + "  SP" + sp);
                            }
                            else if (install == "1")
                            {
                                Debug.Log("  " + subKeyName + "  " + name);
                            }
                        }
                    }
                }
            }
        }
    }

    private static void Get45or451FromRegistry()
    {
        
        using (RegistryKey ndpKey = RegistryKey.OpenRemoteBaseKey(RegistryHive.LocalMachine, "").OpenSubKey("SOFTWARE\\Microsoft\\NET Framework Setup\\NDP\\v4\\Full\\"))
        {
            if (ndpKey != null && ndpKey.GetValue("Release") != null)
            {
                Debug.Log("Version: " + CheckFor45DotVersion((int)ndpKey.GetValue("Release")));
            }
            else
            {
                Debug.Log("Version 4.5 or later is not detected.");
            }
        }
    }
    private static string CheckFor45DotVersion(int releaseKey)
    {
        if (releaseKey >= 393295)
        {
            return "4.6 or later";
        }
        if ((releaseKey >= 379893))
        {
            return "4.5.2 or later";
        }
        if ((releaseKey >= 378675))
        {
            return "4.5.1 or later";
        }
        if ((releaseKey >= 378389))
        {
            return "4.5 or later";
        }
        // This line should never execute. A non-null release key should mean
        // that 4.5 or later is installed.
        return "No 4.5 or later version detected";
    }
}
