using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class Money : MonoBehaviour {

    private int numcoins = 0;
    public GameObject okay;
    public Text text;
    private GameObject go = null;
    private GameObject go2 = null;
    private bool moving = false;
    private bool onehit = true;

    void OnEnable()
    {
        Events.SetActive(false);
        numcoins = Random.Range(10, 151);
        Events.GetPlayer().iteminv.AddCoins(numcoins);

        text.text = "Wow!\nYou've found " + numcoins + " coins\nunder a rock!";
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
}
