using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using System.Collections.Generic;

public class Events : MonoBehaviour {

    public PlayerMain player;
    private bool moving = false;
    private GameObject go = null;
    private GameObject go2 = null;
    private bool onehit = true;

    public GameObject panel;

    public SpriteRenderer bg;
    public Sprite[] bgs = new Sprite[6];

    //public Queue<EventType> events = new Queue<EventType>();
    private int checker;
    public GameObject goback;
    public GameObject gamble, sell, trap, pickpocket, money, monster;

    private bool stoploading = false;

    public static bool IsActive() { return instance != null; }
    private static Events curInstance;
    private static Events instance { get { return curInstance; } }

    public static PlayerMain.Player GetPlayer() { return instance.player.GetPlayer(); }

    public float[] percents;
    //from highest to lowest
    //battle
    //money
    //pickpocket
    //trap
    //freemon
    //nothing


    IEnumerator Waiting()
    {
        while (!stoploading)
        {
            //yield return new WaitForSeconds(Random.Range(3.5f, 6f));
            yield return new WaitForSeconds(1f);
            //checker = Random.Range((int)0, (int)7);
            checker = Choose(percents);
            if (!stoploading)
            {
                switch (checker)
                {
                    case 5:
                        break;
                    case 1:
                        stoploading = true;
                        panel.SetActive(true);
                        money.gameObject.SetActive(true);
                        break;
                    case 2:
                        if (player.GetPlayer().iteminv.Coins() > 0)
                        {
                            stoploading = true;
                            panel.SetActive(true);
                            pickpocket.gameObject.SetActive(true);
                        }
                        break;
                    case 3:
                        stoploading = true;
                        panel.SetActive(true);
                        trap.SetActive(true);
                        break;
                    case 4:
                        if (!player.GetPlayer().moninv.IsFull())
                        {
                            stoploading = true;
                            panel.SetActive(true);
                            monster.gameObject.SetActive(true);
                        }
                        break;
                    case 0:
                        //scene = "Battle";
                        //do an animation that leads into it
                        GlobalData.loadGame = true;
                        stoploading = true;
                        StartCoroutine("Fader", "Battle");
                        break;
                    case 6:
                        break;
                }
            }
        }
    }

    int Choose(float[] probs)
    {

        float total = 0;

        foreach (float elem in probs)
        {
            total += elem;
        }

        float randomPoint = Random.value * total;

        for (int i = 0; i < probs.Length; i++)
        {
            if (randomPoint < probs[i])
            {
                return i;
            }
            else
            {
                randomPoint -= probs[i];
            }
        }
        return probs.Length - 1;
    }

    public static void SetActive(bool set)
    {
        instance.gameObject.SetActive(set);
    }

    void OnEnable()
    {
        panel.SetActive(false);
        Random.InitState((int)System.DateTime.Now.Ticks);
        stoploading = false;
        bg.sprite = bgs[Random.Range((int)0, (int)bgs.Length)];
        SaveLoad.Save(SaveCallBack);
        StartCoroutine("Waiting");
    }
    // Use this for initialization
    void Start () {
        Random.InitState((int)System.DateTime.Now.Ticks);
        bg.sprite = bgs[Random.Range((int)0, (int)bgs.Length)];
        if (curInstance == null) { curInstance = this; }
        StartCoroutine("Waiting");
    }


    void Update ()
    {


        if (Input.touchCount == 1 && onehit)
        {

                // touch on screen
                if (Input.GetTouch(0).phase == TouchPhase.Began)
                {
                    Ray ray = Camera.main.ScreenPointToRay(Input.GetTouch(0).position);
                    RaycastHit hit = new RaycastHit();
                    moving = Physics.Raycast(ray, out hit);
                    if (moving)
                    {
                        go = hit.transform.gameObject;
                        //Debug.Log("Touch Detected on : " + go.name);
                    }
                    else onehit = false;


                }


                // release touch/dragging
                if ((Input.GetTouch(0).phase == TouchPhase.Ended || Input.GetTouch(0).phase == TouchPhase.Canceled || Input.touchCount != 1) && go != null && onehit)
                {
                    Ray ray = Camera.main.ScreenPointToRay(Input.GetTouch(0).position);
                    RaycastHit hit = new RaycastHit();
                    moving = Physics.Raycast(ray, out hit);
                    if (moving)
                    {
                        go2 = hit.transform.gameObject;
                    }
                    moving = false;
                //Debug.Log("Touch Released from : " + go.name);
                if (go == goback && go == go2)
                {
                    goback.GetComponent<SpriteRenderer>().color = Color.red;
                    GlobalData.loadGame = true;
                    GlobalData.pickstart = false;
                    stoploading = true;
                    //scene = "TownTest";
                    //SceneManager.LoadScene("TownTest");
                    StartCoroutine("Fader","TownTest");

                }

                }
        }
        
        else
        {
            go = null;
            go2 = null;
            onehit = true;
        }
    }
    public SpriteRenderer fader;
    //private string scene = "";
    IEnumerator Fader(string toscene)
    {
        stoploading = true;
        fader.gameObject.SetActive(true);
        Color temp = fader.color;
        temp.a = 0;
        fader.color = temp;
        while (fader.color.a < 1)
        {
            temp.a += .05f;

            fader.color = temp;
            yield return new WaitForSeconds(.01f);
        }


        

        SceneManager.LoadScene(toscene);
    }
    private void SaveCallBack()
    {

    }





}
