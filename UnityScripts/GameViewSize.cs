
using System.Reflection;
using UnityEngine;
using UnityEditor;
using System;


public class GameViewSize : Editor {


    static object gameViewSizesInstance;
    static MethodInfo getGroup;

    static GameViewSize()
    {
        Debug.Log("GameViewSize");
        //gameViewSizesInstance  = ScriptableSingleton<GameViewSizes>.instance;
        
        var sizesType = typeof(Editor).Assembly.GetType("UnityEditor.GameViewSizes");
        var singleType = typeof(ScriptableSingleton<>).MakeGenericType(sizesType);
        var instanceProp = singleType.GetProperty("instance");
        getGroup = sizesType.GetMethod("GetGroup");
        gameViewSizesInstance = instanceProp.GetValue(null, null);

        if(gameViewSizesInstance == null)
        {
            Debug.Log("gameViewSizesInstance is null");
        }
    }

    public enum GameViewSizeType
    {
        AspectRatio, FixedResolution
    }

    
    public static void SetGameViewSizeORAddsize512()
    {
        GameViewSizeGroupType type = GetCurrentType();
        int idx = FindSize(type, 512, 512);
        Debug.Log(type);
        if (idx != -1)
        {
            SetSize(idx);
        }
        else
        {
            AddCustomSize(GameViewSizeType.FixedResolution, type, 512, 512, "PrintView");
            idx = FindSize(type, 512, 512);
            SetSize(idx);
        }
    }


    public static void SetGameViewSizeORAddsizeFHD()
    {
        GameViewSizeGroupType type = GetCurrentType();
        int idx = FindSize(type, 1920, 1080);
        if (idx != -1)
        {
            SetSize(idx);
        }
        else
        {
            AddCustomSize(GameViewSizeType.FixedResolution, type, 1920, 1080, "Sticker");
            idx = FindSize(type, 1920, 1080);
            SetSize(idx);
        }
    }


    public static void SetSize(int index)
    {
        //Debug.Log("Set Size :" + index);
        var gvWndType = typeof(Editor).Assembly.GetType("UnityEditor.GameView");
        
        var gvWnd = EditorWindow.GetWindow(gvWndType);
        if(gvWnd != null)
        {
            var SizeSelectionCallback = gvWndType.GetMethod("SizeSelectionCallback",
                    BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
            try
            {
                SizeSelectionCallback.Invoke(gvWnd, new object[] { index, 0 });
            }
            catch(Exception e)
            {
                Debug.Log(e.ToString());
            }
        }
    }


    public static void AddCustomSize(GameViewSizeType viewSizeType, GameViewSizeGroupType sizeGroupType, int width, int height, string text)
    {
        // GameViewSizes group = gameViewSizesInstance.GetGroup(sizeGroupTyge);
        // group.AddCustomSize(new GameViewSize(viewSizeType, width, height, text);
        if(getGroup == null)
        {
            Debug.Log("getGroup is null");
        }
        Debug.Log("AddCustomSize");
        var group = GetGroup(sizeGroupType);
        if(group == null)
        {
            Debug.Log("group is null");
        }
        var addCustomSize = getGroup.ReturnType.GetMethod("AddCustomSize"); // or group.GetType().
        if (addCustomSize == null)
        {
            Debug.Log("addCustomSize is null");
        }
        
        var gvsType = typeof(Editor).Assembly.GetType("UnityEditor.GameViewSize");
        var gameviewtype = typeof(Editor).Assembly.GetType("UnityEditor.GameViewSizeType");
        if (gvsType == null)
        {
            Debug.Log("gvsType is null");
        }
      
        for (int i = 0; i < gvsType.GetMethods().Length; i++)
        {
            Debug.Log(gvsType.GetMethods(BindingFlags.Public | BindingFlags.Instance | BindingFlags.Static)[i].Name);
        }

        for (int i = 0; i < gvsType.GetFields().Length; i++)
        {
            Debug.Log(gvsType.GetFields(BindingFlags.Public | BindingFlags.Instance | BindingFlags.Static)[i].Name);
        }
        var cons = gvsType.GetConstructors();

        for (int i = 0; i < gvsType.GetConstructors().Length; i++)
        {
            Debug.Log(gvsType.GetConstructors(BindingFlags.Public | BindingFlags.Instance | BindingFlags.Static)[i].Name);
        }
        Debug.Log("-----------------------------------------------");
        Debug.Log("Length : " + cons.Length);
        for (int i = 0; i < cons.Length; i++)
        {
            var pram = cons[i].GetParameters();
            for (int j = 0; j < pram.Length; j++)
            {
                Debug.Log(pram[j].ParameterType);
            }
            Debug.Log("GetParameters Length : " + pram.Length);
        }

        var cctor = gvsType.GetConstructors()[0];

        Type type = gameviewtype.GetMembers()[1].GetType();
        //var ctor = gvsType.GetConstructor(new Type[] { typeof(int), typeof(int), typeof(int), typeof(string) });

        



        if (cctor == null)
        {
            Debug.Log("ctor is null");
        }
        //Debug.Log(ctor.Name);
        //Debug.Log(ctor.ReflectedType);

        Debug.Log("AddCustomSize End ");

        //var newsizetest = info.Invoke(gvsType, new object[] { (int)viewSizeType, width, height, text });

        //if (newsizetest == null)
        //{
        //    Debug.Log("newsizetest is null");
        //}
        //Debug.Log(newsizetest.GetType());
        //Debug.Log(newsizetest.ToString());
        //addCustomSize.Invoke(getGroup, new object[] { newsizetest });


        //var newSize = ctor.Invoke(new object[] { ((int)viewSizeType), width, height, text });
        var newSize = cctor.Invoke(new object[] { ((int)viewSizeType), width, height, text });

        Debug.Log("New size ");
        if (newSize == null)
        {
            Debug.Log("newSize is null");
        }
        addCustomSize.Invoke(group, new object[] { newSize });
        if(addCustomSize == null)
        {
            Debug.Log("null!!");
        }
        Debug.Log("Add ");
    }

    public static bool SizeExists(GameViewSizeGroupType sizeGroupType, string text)
    {
        return FindSize(sizeGroupType, text) != -1;
    }

    public static int FindSize(GameViewSizeGroupType sizeGroupType, string text)
    {
        // GameViewSizes group = gameViewSizesInstance.GetGroup(sizeGroupType);
        // string[] texts = group.GetDisplayTexts();
        // for loop...

        var group = GetGroup(sizeGroupType);
        var getDisplayTexts = group.GetType().GetMethod("GetDisplayTexts");
        var displayTexts = getDisplayTexts.Invoke(group, null) as string[];
        for (int i = 0; i < displayTexts.Length; i++)
        {
            string display = displayTexts[i];
            // the text we get is "Name (W:H)" if the size has a name, or just "W:H" e.g. 16:9
            // so if we're querying a custom size text we substring to only get the name
            // You could see the outputs by just logging
            // Debug.Log(display);
            int pren = display.IndexOf('(');
            if (pren != -1)
                display = display.Substring(0, pren - 1); // -1 to remove the space that's before the prens. This is very implementation-depdenent
            if (display == text)
                return i;
        }
        return -1;
    }

    public static bool SizeExists(GameViewSizeGroupType sizeGroupType, int width, int height)
    {
        return FindSize(sizeGroupType, width, height) != -1;
    }



    public static int FindSize(GameViewSizeGroupType sizeGroupType, int width, int height)
    {
        // goal:
        // GameViewSizes group = gameViewSizesInstance.GetGroup(sizeGroupType);
        // int sizesCount = group.GetBuiltinCount() + group.GetCustomCount();
        // iterate through the sizes via group.GetGameViewSize(int index)

        var group = GetGroup(sizeGroupType);
        var groupType = group.GetType();
        var getBuiltinCount = groupType.GetMethod("GetBuiltinCount");
        var getCustomCount = groupType.GetMethod("GetCustomCount");
        int sizesCount = (int)getBuiltinCount.Invoke(group, null) + (int)getCustomCount.Invoke(group, null);
        var getGameViewSize = groupType.GetMethod("GetGameViewSize");
        var gvsType = getGameViewSize.ReturnType;
        var widthProp = gvsType.GetProperty("width");
        var heightProp = gvsType.GetProperty("height");
        var indexValue = new object[1];
        for (int i = 0; i < sizesCount; i++)
        {
            indexValue[0] = i;
            var size = getGameViewSize.Invoke(group, indexValue);
            int sizeWidth = (int)widthProp.GetValue(size, null);
            int sizeHeight = (int)heightProp.GetValue(size, null);
            if (sizeWidth == width && sizeHeight == height)
                return i;
        }
        return -1;
    }

    static object GetGroup(GameViewSizeGroupType type)
    {
        return getGroup.Invoke(gameViewSizesInstance, new object[] { (int)type });
    }
    public static GameViewSizeGroupType GetCurrentGroupType()
    {
        var getCurrentGroupTypeProp = gameViewSizesInstance.GetType().GetProperty("currentGroupType");
        return (GameViewSizeGroupType)(int)getCurrentGroupTypeProp.GetValue(gameViewSizesInstance, null);
    }

    private static GameViewSizeGroupType GetCurrentType()
    {
        if (EditorUserBuildSettings.activeBuildTarget == BuildTarget.iOS)
        {
            return GameViewSizeGroupType.iOS;
        }
        else if (EditorUserBuildSettings.activeBuildTarget == BuildTarget.Android)
        {
            return GameViewSizeGroupType.Android;
        }
        else
        {
            return GameViewSizeGroupType.Standalone;
        }
    }
        
     
}


