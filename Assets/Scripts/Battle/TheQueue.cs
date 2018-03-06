using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine.Networking;
using UnityEngine;

public class TheQueue : NetworkBehaviour {

    public Text eventtext;
    public Queue<string> events = new Queue<string>();
    public GameObject textbg;
    
    public enum SM
    {
        S,
        M,
        error
    }

    public SM test = SM.S;

    public static bool IsActive() { return instance != null; }
    private static TheQueue curInstance;
    private static TheQueue instance { get { return curInstance; } }

    public static int QCount() { return instance.events.Count; }

    public static void AddQueue(string ns)
    {
        instance.textbg.SetActive(true);
        instance.events.Enqueue(ns);
        if(instance.test==SM.S)
            BattleMain3.TurnSet(false);
    }

    // Use this for initialization
    void Start ()
    {
        if (curInstance == null) { curInstance = this; }
        StartCoroutine("TextEvents");

        youturn = false;
        monturn = false;
        curdead = false;
        goadventure = false;
        gohome = false;
        cont = true;
        deadselect = false;
}

    public static bool deadselect = false;
    IEnumerator DeadYou()
    {
        if (instance.test == SM.S)
        {
            deadselect = true;
            if (BattleMain3.GetYou() != null) if (BattleMain3.GetYou().Name != "") BattleMain3.Player1().moninv.RemoveMonster(BattleMain3.GetYou());
            BattleMain3.SetYou(new Monster.SubMonster());
            SaveLoad.Save(SaveCallBack);
            while (true)
            {

                cont = false;
                UpMenu.SetMenu(UpMenu.curMenu.mon);
                if (BattleMain3.GetYou().Name != "") break;
                yield return new WaitForEndOfFrame();
            }

            UpMenu.SetMenu(UpMenu.curMenu.none);
            cont = true;
            yield return new WaitForSeconds(.01f);
        }
        else
        {
            deadselect = true;
            while (true)
            {
                cont = false;
                if (BMMulti.GetYou(1).Name == "" && BMMulti.GetYou(2).Name=="")
                {
                    BMMulti.MPlayerPick(1).transform.FindChild("MenuMother").gameObject.GetComponent<MUpMenu>().SetMenu(MUpMenu.curMenu.mon);
                    BMMulti.MPlayerPick(2).transform.FindChild("MenuMother").gameObject.GetComponent<MUpMenu>().SetMenu(MUpMenu.curMenu.mon);
                    //if (BMMulti.GetYou(1).Name != "" && BMMulti.GetYou(2).Name!="") break;
                    yield return new WaitForEndOfFrame();
                }
                else if(BMMulti.GetYou(1).Name == "")
                {
                    BMMulti.MPlayerPick(2).transform.FindChild("MenuMother").gameObject.GetComponent<MUpMenu>().SetMenu(MUpMenu.curMenu.none);
                    BMMulti.MPlayerPick(1).transform.FindChild("MenuMother").gameObject.GetComponent<MUpMenu>().SetMenu(MUpMenu.curMenu.mon);
                    if (BMMulti.GetYou(1).Name != "") break;
                    yield return new WaitForEndOfFrame();
                }
                else if (BMMulti.GetYou(2).Name == "")
                {
                    BMMulti.MPlayerPick(1).transform.FindChild("MenuMother").gameObject.GetComponent<MUpMenu>().SetMenu(MUpMenu.curMenu.none);
                    BMMulti.MPlayerPick(2).transform.FindChild("MenuMother").gameObject.GetComponent<MUpMenu>().SetMenu(MUpMenu.curMenu.mon);
                    if (BMMulti.GetYou(2).Name != "") break;
                    yield return new WaitForEndOfFrame();

                }

            }
            BMMulti.MPlayerPick(1).transform.FindChild("MenuMother").gameObject.GetComponent<MUpMenu>().SetMenu(MUpMenu.curMenu.none);
            BMMulti.MPlayerPick(2).transform.FindChild("MenuMother").gameObject.GetComponent<MUpMenu>().SetMenu(MUpMenu.curMenu.none);
            cont = true;
            yield return new WaitForSeconds(.01f);
        }

    }

    public static bool youturn = false;
    public static bool monturn = false;
    public static bool curdead = false;
    public static bool goadventure = false;
    public static bool gohome = false;
    public static bool cont = true;
    IEnumerator TextEvents()
    {
        while (true)
        {
            if (instance.test == SM.S)
            {
                if (!BattleMain3.TurnGet() && events.Count > 0 && cont)
                {
                    eventtext.text = events.Dequeue();
                    yield return new WaitForSeconds(.5f);
                    while (!(Input.touchCount == 1 && Input.GetTouch(0).phase == TouchPhase.Began) && !Input.GetKey(KeyCode.C))
                    {
                        yield return new WaitForEndOfFrame();
                    }

                    if (events.Count == 0)
                    {
                        if (curdead)
                        {
                            cont = false;
                            StartCoroutine("DeadYou");
                        }

                        else if (!gohome && !goadventure && !curdead && !monturn && !youturn)
                        {
                            BattleMain3.TurnSet(true);
                            eventtext.text = "";
                            textbg.SetActive(false);
                        }
                        else if (goadventure)
                        {
                            SaveLoad.Save(SaveCallBack);
                            StartCoroutine("Fader", "Adventure");
                        }
                        else if (gohome)
                        {
                            SaveLoad.Save(SaveCallBack);
                            StartCoroutine("Fader", "TownTest");
                        }
                    }
                }
                else yield return new WaitForSeconds(.01f);
            }
            else
            {
                if (!BMMulti.TurnGet() && events.Count > 0 && cont)
                {
                    eventtext.text = events.Dequeue();
                    yield return new WaitForSeconds(.5f);
                    while (!(Input.touchCount == 1 && Input.GetTouch(0).phase == TouchPhase.Began) && !Input.GetKey(KeyCode.C))
                    {
                        yield return new WaitForEndOfFrame();
                    }

                    if (events.Count == 0)
                    {
                        if (curdead)
                        {
                            cont = false;
                            StartCoroutine("DeadYou");
                        }

                        else if (!gohome && !goadventure && !curdead && !monturn && !youturn)
                        {
                            BMMulti.TurnSet(true);
                            eventtext.text = "";
                            textbg.SetActive(false);
                        }
                        else if (goadventure)
                        {
                            //SaveLoad.Save(SaveCallBack);
                            StartCoroutine("Fader", "Adventure");
                        }
                        else if (gohome)
                        {
                            //SaveLoad.Save(SaveCallBack);
                            StartCoroutine("Fader", "TownTest");
                        }
                    }
                }
                else yield return new WaitForSeconds(.01f);
            }
        }
    }

    public SpriteRenderer fader;
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

    private void SaveCallBack() { }
}
