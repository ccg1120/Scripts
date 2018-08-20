using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System;
using System.Threading.Tasks;

namespace Samsung.ARUX
{
    public class FileManager
    {
        public enum ImageType
        {
            PNG,
            JPG
        }


        private static string filePath;
        private static string savePath;
        
        private static readonly string SaveFolderName = "Capture";
        private static readonly string CaptureName = "Screen_";
        private static bool initCheck = false; 

        #region Get
        public string FilePath
        {
            get
            {
                return filePath;
            }
        }
        public string SavePath
        {
            get
            {
                return savePath;
            }
        }
        #endregion

        public static void Init()
        {
           
            
            initCheck = true;
            filePath = Application.persistentDataPath;
            savePath = Path.Combine(filePath, SaveFolderName);
            DirectoryInfo info = new DirectoryInfo(savePath);
            if(!info.Exists)
            {
                Debug.Log("Create!!");
                info.Create();
            }
            Debug.Log("filePath : "+ filePath);
            Debug.Log("savePath : " + savePath);
            
        }
        public static string CaptureSaveFullPath(out string name)
        {
            if(!initCheck)
            {
                Debug.LogError("initCheck is False");
                name = string.Empty;
                return string.Empty;
            }

            
            
            TimeSpan time = DateTime.Now.TimeOfDay;
            string filecreatetime = time.Hours + "_" + time.Minutes + "_" + time.Seconds + "_" + time.Milliseconds;

            string filename = CaptureName + DateTime.Now.ToString("yyyy_hh_mm") + "_"+ filecreatetime;
            name = filename; //out 파일이름 반환

            return Path.Combine(savePath, filename);
        }

        public static void CaptureSave(string path, Texture2D tex, ImageType type)
        {
            if(!initCheck)
            {
                Debug.LogError("initCheck is False");
                return;
            }
            string savepath = path;
            byte[] imagebytearray = null;

            switch (type)
            {
                case ImageType.PNG:
                    imagebytearray = tex.EncodeToPNG();
                    savepath = savepath + ".png";
                    break;
                case ImageType.JPG:
                    imagebytearray = tex.EncodeToJPG();
                    savepath = savepath + ".jpg";
                    break;
            }

            File.WriteAllBytes(savepath, imagebytearray);

            //Task saveTask = new Task(()=> Task_SaveImage(savepath,imagebytearray));
            //saveTask.Start();
            //saveTask.Wait();
        }

        private static void Task_SaveImage(string path, byte[] array)
        {
            Debug.Log("TASK - Start Task_SaveImage");
            File.WriteAllBytes(path,array);
            Debug.Log("TASK - Finish Task_SaveImage");
        }
    }
}
