using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;


namespace Samsung.ARUX
{
    public class ScreenCapture : MonoBehaviour
    {
        private static Camera captureCam;
        private static Vector2 captureSize = new Vector2(1920,1080);
        
        
        public static void Init()
        {
            captureCam = Camera.main;

        }


        public static void ScreenShot()
        {
            string filename = string.Empty;
            string savepath = FileManager.CaptureSaveFullPath(out filename);

            Texture2D tex = new Texture2D((int)captureSize.x, (int)captureSize.y);


            RenderTexture rt = new RenderTexture((int)captureSize.x, (int)captureSize.y,16);
            rt.Create();

            RenderTexture temp = captureCam.targetTexture;

            captureCam.targetTexture = rt;
            captureCam.Render();

            RenderTexture.active = rt;
            tex.ReadPixels(new Rect(0, 0, rt.width, rt.height),0,0);
            tex.Apply();
            RenderTexture.active = temp;
            captureCam.targetTexture = null;
            
            FileManager.CaptureSave(savepath, tex, FileManager.ImageType.PNG);
            Destroy(rt);
            Destroy(tex);

        }

    }
}

