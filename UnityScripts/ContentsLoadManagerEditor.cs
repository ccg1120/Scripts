using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.IO;
using UnityEditorInternal;
using System.Reflection;


namespace Treal
{
    [CustomEditor(typeof(ContentsLoadManager))]
    public class ContentsLoadManagerEditor : Editor {

        public Texture Addicon;
        public Texture Deleteicon;

        private static ContentsLoadManager contentsLoadManager;
        private static List<string> itemStringList = new List<string>();

        private ReorderableList reorderItemList;
        private int choiceIndex = -1;
        private bool createMode = false;

        private GameObject previewGameobject;
        private PreviewRenderUtility previewRenderUtility;

        private int previewLayer;

        private Vector3 centerPosition;

        private ReorderableList contentsRList;
        private bool previewChange = false;

        private GUIStyle labelfontstyle;
        private GUIStyle smallfontStyle;
        private bool ChoiceCheck = false;
        private bool[] checkers;

        private static int createChoice = -1;
        private static int stateChoice = -1;

        public override void OnInspectorGUI()
        {
            GUIStyleSetting();
            //DrawDefaultInspector();
            GUILayout.BeginVertical("HelpBox");
            GUILayout.Label("Contents Menu", labelfontstyle);
            if(contentsRList != null) contentsRList.DoLayoutList();
        
            GUILayout.BeginVertical("Box");
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField("Object Name",GUILayout.Width(100));

            if(choiceIndex == -1)
            {
                EditorGUILayout.TextField("");
            }
            else
            {
                EditorGUILayout.TextField(contentsLoadManager.ItemList[choiceIndex].name);
            }
        
            EditorGUILayout.EndHorizontal();
            GUILayout.BeginHorizontal();
            GUILayout.FlexibleSpace();
            createChoice = GUILayout.Toolbar(createChoice, new Texture[] {Addicon,Deleteicon }, GUILayout.Width(60),GUILayout.Height(20));

            switch(createChoice)
            {
                case 0:
                    createMode = true;
                    contentsLoadManager.CreateGameobject(choiceIndex);
                    contentsLoadManager.CreatPreview();
                    createChoice = -1;
                    break;
                case 1:
                    createChoice = -1;
                    break;
            }
            GUILayout.FlexibleSpace();
            GUILayout.EndHorizontal();

            GUI.color = Color.gray;
            GUILayout.BeginHorizontal(EditorStyles.helpBox);
            GUI.color = Color.black;
            EditorGUILayout.LabelField("You can create a contents", smallfontStyle, GUILayout.Height(12));
            GUI.color = Color.white;
            GUILayout.EndHorizontal();
            //if (GUILayout.Button("Content Create", GUILayout.Height(30)))
            //{
            //    createMode = true;
            //    contentsLoadManager.CreateGameobject(choiceIndex);
            //    contentsLoadManager.CreatPreview();
            //}
            GUILayout.EndVertical();
            EditorGUILayout.Space();
            EditorGUILayout.BeginVertical("Box");
        
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField("File Management",GUILayout.Width(100));

            stateChoice = GUILayout.Toolbar(stateChoice , new string[] { "Refresh","Clear"}, GUILayout.Height(20),GUILayout.Width(300));

            switch(stateChoice)
            {
                case 0:
                    FileDirectory();
                    LoadItems();
                    stateChoice = -1;
                    break;
                case 1:
                    contentsLoadManager.ClearItemList();
                    itemStringList.Clear();
                    stateChoice = -1;
                    break;
            }


            //if (GUILayout.Button("Refresh", GUILayout.Height(30)))
            //{
            //    FileDirectory();
            //    LoadItems();
            //}
            //if(GUILayout.Button("Clear",
            //    GUILayout.Height(30)))
            //{
            //    contentsLoadManager.ClearItemList();
            //    itemStringList.Clear();
            //}
            EditorGUILayout.EndVertical();
            EditorGUILayout.BeginHorizontal();
        
            EditorGUILayout.EndHorizontal();
       
            GUILayout.EndVertical();
        }
      

        private void OnSceneGUI()
        {
            Event current = Event.current;
            if (createMode)
            {
                RaycastHit hit = SceneClickPoint();
                if (hit.collider != null)
                {
                    contentsLoadManager.GameobjectPosition(hit.point);
                    contentsLoadManager.PreviewPosition(hit.point);
                }
                if (current.type == EventType.MouseDown )
                {
                    Debug.Log("UpAction");
                    createMode = false;
                    contentsLoadManager.FinishPreview();
                    Selection.activeGameObject = contentsLoadManager.gameObject;
                }
            }
        }

        private RaycastHit SceneClickPoint()
        {
            float height = SceneView.lastActiveSceneView.camera.pixelHeight;
            Ray ray = SceneView.lastActiveSceneView.camera.ScreenPointToRay(new Vector3(Event.current.mousePosition.x, height - Event.current.mousePosition.y, 0));
            RaycastHit hit;
            Physics.Raycast(ray, out hit);
            return hit;
        }
        private void OnEnable()
        {
            if (contentsLoadManager == null) contentsLoadManager = target as ContentsLoadManager;
            contentsLoadManager.refresh = true; 
            EditorApplication.update += RefreshFindAsset;

      
        }
        private void OnDisable()
        {
            EditorApplication.update -= RefreshFindAsset;
        }

        private void RefreshFindAsset()
        {
            if (contentsLoadManager.refresh)
            {
                contentsLoadManager.ClearItemList();
                FileDirectory();
                LoadItems();

                checkers = new bool[contentsLoadManager.ItemList.Count];
                CheckReset();
                ReorderItemList();
            }
        }
        private void FileDirectory()
        {
                itemStringList.Clear();
                DirectoryInfo directoryInfo = new DirectoryInfo(Application.dataPath + "/Treal/Contents/");

                for (int index = 0; index < directoryInfo.GetFiles().Length; index++)
                {
                    //Debug.Log(directoryInfo.GetFiles()[index].Name);
                    if (!directoryInfo.GetFiles()[index].Name.Contains(".meta") && !directoryInfo.GetFiles()[index].Name.Contains(".asset"))
                    {
                        //Debug.Log(directoryInfo.GetFiles()[index].Name);
                        Debug.Log("string Name :: " + directoryInfo.GetFiles()[index].Name + " lenght :: " + directoryInfo.GetFiles()[index].Name.Length);
                        itemStringList.Add(directoryInfo.GetFiles()[index].Name);
                    }
                }
                contentsLoadManager.refresh = false;
                contentsLoadManager.ClearItemList();
        }

        private void LoadItems()
        {
            Debug.Log("defalt path :: "+ contentsLoadManager.Path);

            //itemsstrings = AssetDatabase.FindAssets("Test1"); //// GUID를 찾아줌 별 필요 없음

            //objs = AssetDatabase.LoadAllAssetsAtPath("Assets/Contents/Test1.prefab"); //// LoadAllAssetsAtPath 해당 에셋에 들어가는 모든 에셋에 대해서 나옴 필요 x 

            //for (int index = 0; index < objs.Length; index++)
            //{
            //    Debug.Log(objs[index].ToString());
            //}
            //object obj = AssetDatabase.LoadAssetAtPath("Assets/Contents/Test1.prefab",typeof(object));
            int count = itemStringList.Count;
            for(int index = 0; index < count; index++)
            {
            
                GameObject temp = (GameObject)AssetDatabase.LoadAssetAtPath("Assets/Treal/Contents/" + itemStringList[index], typeof(GameObject));
                Debug.Log("Load item name :: " + temp.name);
                if (!contentsLoadManager.ItemList.Contains(temp)) contentsLoadManager.ItemList.Add(temp);
            }
        }

        [MenuItem("Treal/Contents")]
        private static void CreateLoadItem()
        {
            if (GameObject.FindObjectOfType<ContentsLoadManager>() != null) return;

            GameObject ContentsManager = new GameObject("ContentsManager");
            ContentsManager.transform.position = Vector3.zero;
            contentsLoadManager = ContentsManager.AddComponent<ContentsLoadManager>();
            contentsLoadManager.refresh = true;
            Selection.activeGameObject = ContentsManager;
            contentsLoadManager.PreviewMaterial = (Material)Resources.Load("PreviewMaterial");


      

        }

    
        private void ReorderItemList()
        {
            contentsRList = new ReorderableList(serializedObject, serializedObject.FindProperty("ItemList"), false, true, false, false);
            contentsRList.drawHeaderCallback = (Rect rect) =>
            {
                EditorGUI.LabelField(rect, "Contents List");
            };
            contentsRList.drawElementCallback = (Rect rect, int index, bool isActive, bool isFocused) =>
            {
                bool check = false;
                var element = contentsRList.serializedProperty.GetArrayElementAtIndex(index);
                rect.y += 2;
                rect.x = rect.x + 20;

                check = EditorGUI.Toggle(new Rect(rect.x - 20, rect.y, 20, EditorGUIUtility.singleLineHeight), checkers[index]);
                if (check != checkers[index])
                {
                    Debug.Log("false true ");
                    if(checkers[index] == true)
                    {
                        checkers[index] = false;
                        choiceIndex = -1;
                    }
                    else
                    {
                        CheckReset();
                        checkers[index] = true;
                        choiceIndex = index;
                    }
                }
                EditorGUI.PropertyField(new Rect(rect.x, rect.y, rect.width - 20, EditorGUIUtility.singleLineHeight), element, GUIContent.none);
            };
            //contentsRList.onAddCallback = (ReorderableList l) =>
            //{
            //    var index = l.serializedProperty.arraySize;
            //    l.serializedProperty.arraySize++;
            //    l.index = index;
            //    //var element = l.serializedProperty.GetArrayElementAtIndex(index);
            //    //element.FindPropertyRelative("prefab").objectReferenceValue = null;
            //};
        }

        private void GUIStyleSetting()
        {
            labelfontstyle = new GUIStyle(EditorStyles.label);
            labelfontstyle.fontStyle = FontStyle.Bold;
            smallfontStyle = new GUIStyle(EditorStyles.label);
            smallfontStyle.fontSize = 9;
        }
        private void CheckReset()
        {
            int count = contentsLoadManager.ItemList.Count;
            for(int index = 0; index < count; index++)
            {
                checkers[index] = false;
            }
        }

    }
}

