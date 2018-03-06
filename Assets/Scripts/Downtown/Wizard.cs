using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wizard : MonoBehaviour {


    public GameObject intro, pickmon, fail, success;


    private void OnEnable()
    {
        intro.SetActive(true);
        pickmon.SetActive(false);
        fail.SetActive(false);
        success.SetActive(false);
    }


    public void Yee()
    {
        intro.SetActive(false);
        success.SetActive(false);

        if (DowntownControl.GetPlayer().iteminv.Coins() >= 750)
        {
            pickmon.SetActive(true);
            fail.SetActive(false);
        }
        else
        {
            pickmon.SetActive(false);
            fail.SetActive(true);
        }
    }

    public void Quit()
    {
        gameObject.SetActive(false);
    }

    public void Success()
    {
        intro.SetActive(false);
        pickmon.SetActive(false);
        fail.SetActive(false);
        success.SetActive(true);
    }

    private bool moving = false;
    private GameObject go = null;
    private GameObject go2 = null;
    private bool onehit = true;
    private void Update()
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
                    if (hit.transform.gameObject.GetComponent<InvSlot>() != null)
                        if (hit.transform.gameObject.GetComponent<InvSlot>().curMonster != null)
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

                    if (hit.transform.gameObject.GetComponent<InvSlot>() != null)
                        if (hit.transform.gameObject.GetComponent<InvSlot>().curMonster != null)
                            go2 = hit.transform.gameObject;
                }
                moving = false;

                if (go == go2)
                {
                    MonInit.RandomMoves(go.GetComponent<InvSlot>().curMonster);
                    DowntownControl.GetPlayer().iteminv.SubstractCoins(750);
                    Success();
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
