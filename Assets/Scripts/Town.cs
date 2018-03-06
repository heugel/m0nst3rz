using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class Town : MonoBehaviour {

    public PlayerMain player;
    public GameObject shop, fountain, book, adventure,iteminv,moninv,downtown,gameover,menubut;
    public SpriteRenderer fader,healed;
    private bool moving = false;
    private GameObject go = null;
    private GameObject go2 = null;
    private bool onehit = true;


    public static bool IsActive() { return instance != null; }
    private static Town curInstance;
    private static Town instance { get { return curInstance; } }

    void Awake()
    {
        if (curInstance == null) { curInstance = this; }
    }

    public static PlayerMain.Player GetPlayer() { return instance.player.GetPlayer(); }

    public static void ShopError(GameObject thing)
    {
        instance.StartCoroutine("NotEnough", thing);
    }

    IEnumerator NotEnough(GameObject thing)
    {
        thing.SetActive(true);
        yield return new WaitForSeconds(1f);
        thing.SetActive(false);
    }

    void Update()
    {
        /*if (Input.GetKeyDown("space"))
        {
            SaveLoad.Save(SaveCallBack);
            GlobalData.loadGame = true;

            StartCoroutine("Fader");
        }*/

        if (player.GetPlayer().moninv.IsEmpty())
        {
            gameover.SetActive(true);
            gameObject.SetActive(false);
        }


        //use bools to make this section go off only if there's a change?
        bool shopisactive = (ShopMain.isActive()||MICollider.isActive()||InvCol.isActive()||BookControl.isActive());
        shop.GetComponent<BoxCollider>().enabled = !shopisactive;
        fountain.GetComponent<BoxCollider>().enabled = !shopisactive;
        book.GetComponent<BoxCollider>().enabled = !shopisactive;
        adventure.GetComponent<BoxCollider>().enabled = !shopisactive;
        iteminv.GetComponent<BoxCollider>().enabled = !shopisactive;
        iteminv.GetComponent<SpriteRenderer>().enabled = !shopisactive;
        moninv.GetComponent<SpriteRenderer>().enabled = !shopisactive;
        moninv.GetComponent<BoxCollider>().enabled = !shopisactive;
        downtown.GetComponent<BoxCollider>().enabled = !shopisactive;
        menubut.SetActive(!shopisactive);
        //if (Input.GetKeyDown("space"))
            //print(go == go2);

        if (Input.touchCount == 1 && onehit)
        {
            if(!shopisactive)
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
                    if (go == shop && go == go2)
                        ShopMain.ShopSet(true);
                    else if (go == fountain && go == go2)
                    {
                        player.GetPlayer().moninv.HealAll();
                        if(canenter) StartCoroutine("HealSprite");
                        //play sound
                        SaveLoad.Save(SaveCallBack);
                    }
                    else if (go == book && go == go2)
                        BookControl.BookSet(true);
                    else if (go == adventure && go == go2)
                    {
                        if (player.GetPlayer().moninv.HasAnActive())
                        {

                            SaveLoad.Save(SaveCallBack);
                            GlobalData.loadGame = true;

                            //fader.color = Color.clear;
                            //SceneManager.LoadScene("Adventure");
                            StartCoroutine("Fader","Adventure");

                        }
                        else
                        {
                            //you need at least one active monster to adventure!
                        }

                    }
                    else if (go == iteminv && go == go2)
                        InvCol.SetActive(true);
                    else if (go == moninv && go == go2)
                        MICollider.SetActive(true);
                    else if(go==downtown && go == go2)
                    {
                        SaveLoad.Save(SaveCallBack);
                        GlobalData.loadGame = true;
                        StartCoroutine("Fader", "Downtown");
                    }
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
    private void SaveCallBack()
    {

    }

    IEnumerator Fader(string toscene)
    {
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

    bool canenter = true;
    IEnumerator HealSprite()
    {
        canenter = false;
        healed.gameObject.SetActive(true);
        Color temp = healed.color;
        temp.a = 1;
        healed.color = temp;
        yield return new WaitForSeconds(1f);
        while(healed.color.a > 0)
        {
            temp.a -= .05f;
            healed.color = temp;
            yield return new WaitForEndOfFrame();
        }
        canenter = true;
    }

    public void LoadMenu()
    {
        StartCoroutine("Fader", "MainMenu");
    }
    
}
