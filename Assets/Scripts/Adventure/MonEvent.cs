using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class MonEvent : MonoBehaviour
{

    //private double dam = 0;
    public SpriteRenderer disp;
    public GameObject yesbut,nobut,okay;
    public Text text;
    private Monster.SubMonster temp = new Monster.SubMonster();
    private GameObject go = null;
    private GameObject go2 = null;
    private bool moving = false;
    private bool onehit = true;

    void OnEnable()
    {
        Events.SetActive(false);

        temp = Lists.RandMon();
        temp = MonInit.RandomFromPrefab(temp);
        
        disp.sprite = Resources.Load("MonsterSprites/" + temp.imagepath, typeof(Sprite)) as Sprite;
        text.text = "Aw! You've found a lonely " + temp.Name + "\nwho wants to join your team\n\nDo you want to keep it?";

        //SaveLoad.Save(SaveCallBack);

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
                if (go == nobut && go == go2)
                {
                    text.text = "";

                        Events.SetActive(true);
                        gameObject.SetActive(false);
                    
                }
                else if(go==yesbut && go == go2)
                {
                    Events.GetPlayer().moninv.AddMonster(temp);
                    SaveLoad.Save(SaveCallBack);
                    okay.SetActive(true);
                    yesbut.SetActive(false);
                    nobut.SetActive(false);

                    if (Events.GetPlayer().moninv.FindReference(temp) > 2)
                    {
                        text.text = temp.Name + " has been added to your backup m0nst3rz";
                    }
                    else text.text = temp.Name + " has been added to your active m0nst3rz";
                }
                else if(go==okay && go == go2)
                {
                    okay.SetActive(false);
                    yesbut.SetActive(true);
                    nobut.SetActive(true);
                    text.text = "";
                    Events.SetActive(true);
                    gameObject.SetActive(false);
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
}
