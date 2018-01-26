using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FPS2 : MonoBehaviour {

    float deltaTime = 0.0f;
    public GUIText text;
    float msec = 0;
    float fps = 0;
    void Update()
    {
        deltaTime += (Time.unscaledDeltaTime - deltaTime) * 0.1f;
        msec = deltaTime * 1000.0f;
        fps = 1.0f / deltaTime;
        text.text = string.Format("{0:0.0} ms ({1:0.} fps)", msec, fps);
        msec = 0;
        fps = 0;
    }

    //void OnGUI()
    //{
    //    int w = Screen.width, h = Screen.height;

    //    GUIStyle style = new GUIStyle();

    //    Rect rect = new Rect(0, 0, w, h * 2 / 100);
    //    style.alignment = TextAnchor.UpperLeft;
    //    style.fontSize = h * 2 / 100;
    //    style.normal.textColor = new Color(0.0f, 0.0f, 0.5f, 1.0f);
      
    //    GUI.Label(rect, text, style);
    //}
}
