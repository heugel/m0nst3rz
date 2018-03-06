using System.Collections;
using System.Collections.Generic;
using UnityEngine.Networking;
using UnityEngine;

public class MBattlePlayer : NetworkBehaviour
{

    public PlayerMain.Player player;
    public GameObject buttons, atk, chng, esc;

    private bool moving = false;
    private GameObject go = null;
    private GameObject go2 = null;
    private bool onehit = true;

    int playernum = 0;

    public GameObject[] movebuttons = new GameObject[3];
    public GameObject[] monbuttons = new GameObject[3];

    public MUpMenu menumother;

    private bool doneyet = false;

    /*public override void OnStartLocalPlayer()
    {
        if (isClient) playernum = 2;
        else if (isServer) playernum = 1;
        print(playernum);
        BMMulti.PlayerSet(GetComponent<PlayerMain>(), playernum);
    }*/

    void Update()
    {
        if (!doneyet)
        {
            player = GetComponent<PlayerMain>().GetPlayer();
            if (isLocalPlayer) playernum = 1;
            else playernum = 2;
            doneyet = true;
        }
        /*if (BMMulti.IsActive() && !doneyet)
        {
            
            if (isLocalPlayer) playernum = 1;
            else playernum = 2;
            //print(playernum);
            //print(isServer);
            BMMulti.PlayerSet(GetComponent<PlayerMain>(), playernum);
            doneyet = true;
        }*/
        buttons.SetActive(BMMulti.TurnGet());

        if (BMMulti.TurnGet() || menumother.upnow != MUpMenu.curMenu.none)
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
                    if (go == atk && go == go2)
                    {
                        //atk menu appears
                        menumother.SetMenu(MUpMenu.curMenu.atk);
                    }
                    else if (go == chng && go == go2)
                    {
                        //active player1.moninvmenu appears
                        menumother.SetMenu(MUpMenu.curMenu.mon);
                    }

                    else if (go == esc && go == go2)
                    {
                        //DISCONNECT

                    }

                    if (go == movebuttons[0] && go2 == go)
                    {
                        menumother.SetMenu(MUpMenu.curMenu.none);
                        BMMulti.SetMove(BMMulti.GetYou(playernum).moves[0],playernum);
                        BMMulti.InitTurn(BMMulti.TurnType.attack,playernum);
                    }
                    else if (go == movebuttons[1] && go2 == go)
                    {
                        menumother.SetMenu(MUpMenu.curMenu.none);
                        BMMulti.SetMove(BMMulti.GetYou(playernum).moves[1], playernum);
                        BMMulti.InitTurn(BMMulti.TurnType.attack,playernum);

                    }
                    else if (go == movebuttons[2] && go2 == go)
                    {
                        menumother.SetMenu(MUpMenu.curMenu.none);
                        BMMulti.SetMove(BMMulti.GetYou(playernum).moves[2], playernum);
                        BMMulti.InitTurn(BMMulti.TurnType.attack, playernum);

                    }

                    if (go == monbuttons[0] && go2 == go)
                    {
                        BMMulti.SetNewMon(monbuttons[0].GetComponent<InvSlot>().curMonster, playernum);
                        UpMenu.SetMenu(UpMenu.curMenu.none);
                        if (!TheQueue.deadselect)
                            BMMulti.InitTurn(BMMulti.TurnType.change, playernum);
                        else
                        {
                            BMMulti.SetYou(monbuttons[0].GetComponent<InvSlot>().curMonster, playernum);
                            BMMulti.SetNewMon(new Monster.SubMonster(), playernum);
                            BMMulti.UpHealth();
                            TheQueue.AddQueue("You've changed to " + BMMulti.GetYou(playernum).Name + "!");
                            TheQueue.deadselect = false;
                            TheQueue.curdead = false;
                        }
                    }
                    else if (go == monbuttons[1] && go2 == go)
                    {
                        BMMulti.SetNewMon(monbuttons[1].GetComponent<InvSlot>().curMonster, playernum);
                        UpMenu.SetMenu(UpMenu.curMenu.none);
                        if (!TheQueue.deadselect)
                            BMMulti.InitTurn(BMMulti.TurnType.change, playernum);
                        else
                        {
                            BMMulti.SetYou(monbuttons[1].GetComponent<InvSlot>().curMonster, playernum);
                            BMMulti.SetNewMon(new Monster.SubMonster(), playernum);
                            BMMulti.UpHealth();
                            TheQueue.AddQueue("You've changed to " + BMMulti.GetYou(playernum).Name + "!");
                            TheQueue.deadselect = false;
                            TheQueue.curdead = false;
                        }
                    }
                    else if (go == monbuttons[2] && go2 == go)
                    {
                        BMMulti.SetNewMon(monbuttons[2].GetComponent<InvSlot>().curMonster, playernum);
                        UpMenu.SetMenu(UpMenu.curMenu.none);
                        if (!TheQueue.deadselect)
                            BMMulti.InitTurn(BMMulti.TurnType.change, playernum);
                        else
                        {
                            BMMulti.SetYou(monbuttons[2].GetComponent<InvSlot>().curMonster, playernum);
                            BMMulti.SetNewMon(new Monster.SubMonster(), playernum);
                            BMMulti.UpHealth();
                            TheQueue.AddQueue("You've changed to " + BMMulti.GetYou(playernum).Name + "!");
                            TheQueue.deadselect = false;
                            TheQueue.curdead = false;
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
        else
        {
            go = null;
            go2 = null;
            onehit = true;
        }

    }

    public void SubmitTurn()
    {

    }


}
