using UnityEngine;
using System.Collections;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using System;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{

    public GameObject newgame, reload, submit;

    private bool moving = false;
    private GameObject go = null;
    private GameObject go2 = null;
    private bool onehit = true;
    // Use this for initialization
    void Start()
    {
        if (!File.Exists(Application.persistentDataPath + "/savedgame.gd"))
        {
            reload.SetActive(false);
        }

        if (GameObject.Find("NetworkManager2") != null)
        {
            Destroy(GameObject.Find("NetworkManager2"));
        }
    }

    // Update is called once per frame
    void Update()
    {
       if (Input.GetKeyDown(KeyCode.Space))
        {
            //go.GetComponent<SpriteRenderer>().color = Color.red;
            GlobalData.pickstart = true;
            GlobalData.loadGame = false;
            SceneManager.LoadScene("MultiplayerTest");
        }
        if (Input.GetKeyDown(KeyCode.C))
        {
            //go.GetComponent<SpriteRenderer>().color = Color.red;
            GlobalData.pickstart = false;
            GlobalData.loadGame = true;
            SceneManager.LoadScene("TownTest");
        }
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
                    moving = false;
                }

                //Debug.Log("Touch Released from : " + go.name);
                if (go == newgame && go == go2)
                {
                    go.GetComponent<SpriteRenderer>().color = Color.red;
                    GlobalData.pickstart = true;
                    GlobalData.loadGame = false;
                    SceneManager.LoadScene("TownTest");
                }
                else if (go == reload && go == go2)
                {
                    go.GetComponent<SpriteRenderer>().color = Color.red;
                    GlobalData.loadGame = true;
                    GlobalData.pickstart = false;
                    SceneManager.LoadScene("TownTest");
                }
                else if (go == submit && go == go2)
                {
                    go.GetComponent<SpriteRenderer>().color = Color.red;
                    GlobalData.loadGame = true;
                    GlobalData.pickstart = false;
                    SceneManager.LoadScene("MultiplayerTest");
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
}