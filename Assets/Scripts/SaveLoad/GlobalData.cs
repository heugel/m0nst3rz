using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

[System.Serializable]
public class GlobalData : MonoBehaviour
{
    public static bool loadGame;
    public static bool pickstart;
    public static bool finishedGame = false;
    //public static bool gameover = false; //check
    public bool first = false;
    //public float saveInterval;
    //start by picking 2 starters?
    public static bool IsInstantiated() { return instance != null; }

    private float startTime;


    void Awake()
    {
        DontDestroyOnLoad(gameObject);
        startTime = Time.time;
        if (loadGame) //check
        {
            SaveLoad.Load(SaveCallBack);
        }

        if (!first)
        {
            Destroy(gameObject);
        }

    }

    private static GlobalData curInstance;
    private static GlobalData instance { get { return curInstance; } }

    void Start()
    {
        if (curInstance == null)
        {
            curInstance = this;
        }

    }

    /*void Update()
    {
        //show a "you win screen" when all monsters discovered
    }*/
    private void SaveCallBack()
    {

    }

}
