using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class DowntownControl : MonoBehaviour
{

    public PlayerMain player;
    public GameObject quest, questmenu, slotmachine, actualslots, portal, iteminv, moninv, menubut, backbut, movewizard, actualwizard;
    public SpriteRenderer fader;
    private bool moving = false;
    private GameObject go = null;
    private GameObject go2 = null;
    private bool onehit = true;


    public static bool IsActive() { return instance != null; }
    private static DowntownControl curInstance;
    private static DowntownControl instance { get { return curInstance; } }

    void Awake()
    {
        if (curInstance == null) { curInstance = this; }
    }

    public static PlayerMain.Player GetPlayer() { return instance.player.GetPlayer(); }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Portal.SetActive(true);

        }

        //use bools to make this section go off only if there's a change?
        bool shopisactive = (MICollider.isActive() || InvCol.isActive() || Portal.isActive() || questmenu.activeSelf || actualslots.activeSelf || actualwizard.activeSelf);
        quest.GetComponent<BoxCollider>().enabled = !shopisactive;
        slotmachine.GetComponent<BoxCollider>().enabled = !shopisactive;
        portal.GetComponent<BoxCollider>().enabled = !shopisactive;
        iteminv.GetComponent<BoxCollider>().enabled = !shopisactive;
        iteminv.GetComponent<SpriteRenderer>().enabled = !shopisactive;
        moninv.GetComponent<SpriteRenderer>().enabled = !shopisactive;
        moninv.GetComponent<BoxCollider>().enabled = !shopisactive;
        movewizard.GetComponent<BoxCollider>().enabled = !shopisactive;
        menubut.SetActive(!shopisactive);
        backbut.SetActive(!shopisactive);
        //if (Input.GetKeyDown("space"))
        //print(go == go2);

        if (Input.touchCount == 1 && onehit)
        {
            if (!shopisactive)
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

                    if (go == quest && go == go2)
                    {
                        questmenu.SetActive(true);
                    }
                    else if (go == portal && go == go2)
                    {
                        Portal.SetActive(true);
                    }
                    else if (go == slotmachine && go == go2)
                    {
                        actualslots.SetActive(true);
                    }

                    else if (go == iteminv && go == go2)
                    {
                        InvCol.SetActive(true);
                    }
                    else if (go == moninv && go == go2)
                    {
                        MICollider.SetActive(true);
                    }
                    else if (go == movewizard && go == go2)
                    {
                        actualwizard.SetActive(true);
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

    public void LoadMenu()
    {
        SaveLoad.Save(SaveCallBack);
        StartCoroutine("Fader", "MainMenu");
    }
    public void LoadTown()
    {
        SaveLoad.Save(SaveCallBack);
        StartCoroutine("Fader", "TownTest");
    }

}
