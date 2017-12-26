using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Threading;
using System.Threading.Tasks;
using System.IO;

public class CPUTest : MonoBehaviour {

    public string readtext;

    // Use this for initialization
    void Start()
    {
        //CPUTesting();
        //StartCoroutine(TestCPU());
    }
    private void Update()
    {

        if(Input.GetKeyDown(KeyCode.A))
        {
            //StartCoroutine(TestCPU());
            AsyncTest();
        }
    }
    IEnumerator TestCPU()
    {
        int total = 0;
        Debug.Log("Game obejct name : " + this.gameObject.name + " ::" + total + " START !");
        for (int index = 0; index < int.MaxValue; index++)
        {
            total = (int)(index / 2);
            //Debug.Log("Thread "+ this.gameObject.name + total);
        }
        Debug.Log("Game obejct name : " + this.gameObject.name + " ::" + total + " END !!!!");
        yield return null;
    }


    //테스크로 지정 할 함수 
    private int CPUTesting(string path)
    {
        int total = 0;
        //Debug.Log("Game obejct name : " + this.gameObject.name + " ::" + total + " START !");
        //Debug.Log(this.gameObject.transform.position);
        readtext = File.ReadAllText(path);
        Debug.Log("READ FILE !! "+ readtext);
        for (int index = 0; index < int.MaxValue; index++)
        {
            total = (int)(index / 2);
        }
        //Debug.Log("Game obejct name : " + this.gameObject.name + " ::" + total + " END !!!!");
        return total;
    }
    //비동기 함수 
    async void AsyncTest()
    {
        //경로 같은걸 가져오는데는 메인 쓰레드에서 가져와야함 
        string path = Application.dataPath + "/Temp.txt";
        Debug.Log("AsyncTest start !" + this.gameObject.name);
        //Task tt = new Task(CPUTesting);
        //함수에 테스크에 대한 등록  람다식으로 함수 이름과 매개변수를 넣어서 사용 
        //매개 변수가 없을 시 그냥 함수 이름만 사용할 수 있음 
        Task<int> tt = new Task<int>(()=> CPUTesting(path)); //테스크의 결과를 반환 받기 위한 방법 
        tt.Start(); //테스트에 대한 실행 
        
        await tt; //await 를 하지 않으면 동기화 되어 다른 스레드 들을 기다리게 된다. 
        Debug.Log("AsyncTest End !!!! ::  " + this.gameObject.name + " total :" +  tt.Result);
    }
    
}
