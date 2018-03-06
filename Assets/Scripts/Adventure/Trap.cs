using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine;

public class Trap : MonoBehaviour
{

    private double dam = 0;
    public GameObject okay;
    public Text text;
    private string[] deads;
    private bool gohome = false;
    private GameObject go = null;
    private GameObject go2 = null;
    private bool moving = false;
    private bool onehit = true;

    void OnEnable()
    {
        Events.SetActive(false);
        gohome = false;
        dam = (double)Random.Range((int)1, (int)7);
        deads = Events.GetPlayer().moninv.Trap(dam);
        text.text = "You fell into a trap!\nYour monsters took " + dam + " damage!\n";
        switch (deads.Length)
        {
            case 0:
                text.text += "\nThankfully your monsters are safe!";
                break;
            case 1:
                text.text += "\n"+deads[0] + " died!! :(";
                break;
            case 2:
                text.text += "\n"+deads[0] + " and " + deads[1] + "\nboth died!!! X( !";
                break;
            case 3:
                text.text += "\nYour active monsters have died...";
                gohome = true;
                break;
        }

        if (!gohome && !Events.GetPlayer().moninv.HasAnActive())
        {
            text.text += "\nYour active monsters have died...";
            gohome = true;
        }
        SaveLoad.Save(SaveCallBack);
        //you fell into a trap and your monsters took dam damage!
        //if (deads.Length == 0) thankfully your monsters are safe
        //else if (deads.Length == 1) deads[0] died!
        //else if(deads.Length == 2) deads[0] and deads[1] died!
        //else if (deads.Length == 3) all your active monsters died!
        //                              return to town
        //iterate through deads to see if any died

    }

    void Update()
    {
        /*if (Input.GetKeyDown("space"))
        {
            Events.SetActive(true);
            gameObject.SetActive(false);
        }*/

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
                if (go == okay && go == go2)
                {
                    text.text = "";
                    if (gohome)
                    {
                        //SaveLoad.Save(SaveCallBack);
                        GlobalData.loadGame = true;
                        GlobalData.pickstart = false;

                        //SceneManager.LoadScene("TownTest");
                        StartCoroutine("Fader");
                    }
                    else
                    {
                        Events.SetActive(true);
                        gameObject.SetActive(false);
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

    public SpriteRenderer fader;
    IEnumerator Fader()
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

        SceneManager.LoadScene("TownTest");
    }
    private void SaveCallBack()
    {

    }
}
