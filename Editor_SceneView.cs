using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

/// <summary>
/// 씬뷰에 버튼을 생성하여 나타나게 하는 스크립트
/// </summary>

public class Editor_SceneView : Editor {

    [MenuItem("Window/Scene GUI/Enable")]
    public static void Enable()
    {
        SceneView.onSceneGUIDelegate += OnScene;
        Debug.Log("Scene GUI : Enabled");
    }

    [MenuItem("Window/Scene GUI/Disable")]
    public static void Disable()
    {
        SceneView.onSceneGUIDelegate -= OnScene;
        Debug.Log("Scene GUI : Disabled");
    }

    private static void OnScene(SceneView sceneview)
    {
        Handles.BeginGUI();
        if (GUILayout.Button("Press Me"))
            Debug.Log("Got it to work.");

        Handles.EndGUI();
    }
   

    //씬 카메라를 해당 오브젝트로 이동할 수 있게 하는 문장
    // SceneView.lastActiveSceneView.LookAt(destinationManager.DestinationList[selectNum].transform.position);
    private static string[] TagSetting = new string[2] { "Destination", "Event" };
    private static bool[] tagSet = new bool[2];
    private static int tagCount = 0;

    public void CreateTag()
    {
        //태그를 검사하고 생성하는 코드 
        SerializedObject tagManager = new SerializedObject(AssetDatabase.LoadAllAssetsAtPath("ProjectSettings/TagManager.asset")[0]);
        SerializedProperty tagsProp = tagManager.FindProperty("tags");


        //검출용으로 사용할 태그에 대해 검사 및 설정
        for (int index = 0; index < tagsProp.arraySize; index++)
        {
            SerializedProperty t = tagsProp.GetArrayElementAtIndex(index);
            for (int index2 = 0; index2 < TagSetting.Length; index2++)
            {
                if (t.stringValue.Equals(TagSetting[index2]))
                {
                    tagSet[index2] = true;
                    tagCount++;
                    break;
                }
            }
        }
        //태그생성
        for (int index = 0; index < TagSetting.Length; index++)
        {
            if (!tagSet[index])
            {

                tagsProp.InsertArrayElementAtIndex(tagCount);
                SerializedProperty n = tagsProp.GetArrayElementAtIndex(tagCount);
                n.stringValue = TagSetting[index];
                //업데이트를 해줘야 보임
                tagManager.ApplyModifiedProperties();
                tagManager.Update();
                tagCount++;
            }
        }
        
    }

    public override void OnInspectorGUI()
    {
        //using 을 통한 분리된 구역의 칼라 설정
        using (new BackgroundColorScope(Color.green))
        {
            // 緑色のボタン
            GUILayout.Button("Button");

            using (new BackgroundColorScope(Color.yellow))
            {
                // 黄色のボタン
                GUILayout.Button("Button");
            }
        }
    }

    // 이미지 배경 색상을 바꾸는 클래스 
    public class BackgroundColorScope : GUI.Scope
    {
        private readonly Color color;
        public BackgroundColorScope(Color color)
        {
            this.color = GUI.backgroundColor;
            GUI.backgroundColor = color;
        }


        protected override void CloseScope()
        {
            GUI.backgroundColor = color;
        }
    }
    
}
