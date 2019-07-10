using System.Collections.Generic;
using UnityEngine;
using System.Diagnostics;

public class TimeChecker : MonoBehaviour
{

    public static List<StopWatchData> StopwatchList = new List<StopWatchData>();

    public static void StartTimer(int id, string str)
    {
        for (int index = 0; index < StopwatchList.Count; index++)
        {
            if(StopwatchList[index].ID.Equals(id))
            {
                UnityEngine.Debug.Log("Timer ID Error" + id );
                return;
            }
        }

        StopWatchData stopwatch = new StopWatchData();
        stopwatch.ID = id;
        stopwatch.Str = str;

        StopwatchList.Add(stopwatch);
        UnityEngine.Debug.Log("Timer Test Debug List index : " + StopwatchList.Count);
        int targetindex = StopwatchList.Count - 1;
        StopwatchList[targetindex].SW.Reset();
        StopwatchList[targetindex].SW.Start();
        UnityEngine.Debug.Log("Timer Start ID : " + StopwatchList[targetindex].ID + " , " + StopwatchList[targetindex].Str);
    }

    public static void EndTimer(int id)
    {
        int targetindex = 0;
        for (int index = 0; index < StopwatchList.Count; index++)
        {
            if(StopwatchList[index].ID.Equals(id))
            {
                targetindex = index;
                break;
            }
        }
        StopwatchList[targetindex].SW.Stop();
        UnityEngine.Debug.Log("Timer End ID : " + StopwatchList[targetindex].ID + " , " + StopwatchList[targetindex].Str + ", Time : " + StopwatchList[targetindex].SW.Elapsed.ToString());
        StopwatchList.RemoveAt(targetindex);

        UnityEngine.Debug.Log("Timer Test Debug List index : " + StopwatchList.Count);

    }


    public class StopWatchData
    {
        public int ID;
        public Stopwatch SW = new Stopwatch();
        public string Str;
    }



}

