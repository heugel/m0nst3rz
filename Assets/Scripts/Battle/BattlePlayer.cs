using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattlePlayer : MonoBehaviour {

    public PlayerMain.Player player;
    public GameObject buttons, atk, itm, chng, esc;

    private bool moving = false;
    private GameObject go = null;
    private GameObject go2 = null;
    private bool onehit = true;

    public GameObject[] movebuttons = new GameObject[3];
    public GameObject[] monbuttons = new GameObject[3];
    public GameObject[] itembuttons = new GameObject[4];

    // Use this for initialization
    void Start ()
    {
        player = GetComponent<PlayerMain>().GetPlayer();
	}
	
	// Update is called once per frame
	void Update ()
    {
        buttons.SetActive(BattleMain3.TurnGet());

        if(Input.GetKeyDown(KeyCode.Space))
            UpMenu.SetMenu(UpMenu.curMenu.item);


        if (BattleMain3.TurnGet() || UpMenu.upnow != UpMenu.curMenu.none)
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
                        UpMenu.SetMenu(UpMenu.curMenu.atk);
                    }
                    else if (go == chng && go == go2)
                    {
                        //active player1.moninvmenu appears
                        UpMenu.SetMenu(UpMenu.curMenu.mon);
                    }
                    else if (go == itm && go == go2)
                    {
                        //iteminvmenu appears
                        UpMenu.SetMenu(UpMenu.curMenu.item);
                    }
                    else if (go == esc && go == go2)
                    {
                        //50/50 shot at escaping
                        UpMenu.SetMenu(UpMenu.curMenu.none);
                        //StartCoroutine("CoDoTurn", TurnType.esc);
                        BattleMain3.InitTurn(BattleMain3.TurnType.esc);

                    }

                    if (go == movebuttons[0] && go2 == go)
                    {
                        UpMenu.SetMenu(UpMenu.curMenu.none);
                        BattleMain3.SetMove(BattleMain3.GetYou().moves[0]);
                        BattleMain3.InitTurn(BattleMain3.TurnType.attack);
                    }
                    else if (go == movebuttons[1] && go2 == go)
                    {
                        UpMenu.SetMenu(UpMenu.curMenu.none);
                        BattleMain3.SetMove(BattleMain3.GetYou().moves[1]);
                        BattleMain3.InitTurn(BattleMain3.TurnType.attack);

                    }
                    else if (go == movebuttons[2] && go2 == go)
                    {
                        UpMenu.SetMenu(UpMenu.curMenu.none);
                        BattleMain3.SetMove(BattleMain3.GetYou().moves[2]);
                        BattleMain3.InitTurn(BattleMain3.TurnType.attack);

                    }

                    if (go == monbuttons[0] && go2 == go)
                    {
                        BattleMain3.SetNewMon(monbuttons[0].GetComponent<InvSlot>().curMonster);
                        UpMenu.SetMenu(UpMenu.curMenu.none);
                        if (!TheQueue.deadselect)
                            BattleMain3.InitTurn(BattleMain3.TurnType.change);
                        else
                        {
                            BattleMain3.SetYou(monbuttons[0].GetComponent<InvSlot>().curMonster);
                            BattleMain3.SetNewMon(new Monster.SubMonster());
                            BattleMain3.UpHealth();
                            TheQueue.AddQueue("You've changed to " + BattleMain3.GetYou().Name + "!");
                            TheQueue.deadselect = false;
                            TheQueue.curdead = false;
                        }
                    }
                    else if (go == monbuttons[1] && go2 == go)
                    {
                        BattleMain3.SetNewMon(monbuttons[1].GetComponent<InvSlot>().curMonster);
                        UpMenu.SetMenu(UpMenu.curMenu.none);
                        if (!TheQueue.deadselect)
                            BattleMain3.InitTurn(BattleMain3.TurnType.change);
                        else
                        {
                            BattleMain3.SetYou(monbuttons[1].GetComponent<InvSlot>().curMonster);
                            BattleMain3.SetNewMon(new Monster.SubMonster());
                            BattleMain3.UpHealth();
                            TheQueue.AddQueue("You've changed to " + BattleMain3.GetYou().Name + "!");
                            TheQueue.deadselect = false;
                            TheQueue.curdead = false;
                        }
                    }
                    else if (go == monbuttons[2] && go2 == go)
                    {
                        BattleMain3.SetNewMon(monbuttons[2].GetComponent<InvSlot>().curMonster);
                        UpMenu.SetMenu(UpMenu.curMenu.none);
                        if (!TheQueue.deadselect)
                            BattleMain3.InitTurn(BattleMain3.TurnType.change);
                        else
                        {
                            BattleMain3.SetYou(monbuttons[2].GetComponent<InvSlot>().curMonster);
                            BattleMain3.SetNewMon(new Monster.SubMonster());
                            BattleMain3.UpHealth();
                            TheQueue.AddQueue("You've changed to " + BattleMain3.GetYou().Name + "!");
                            TheQueue.deadselect = false;
                            TheQueue.curdead = false;
                        }
                    }


                    if (go == itembuttons[0] && go2 == go)
                    {
                        UpMenu.SetMenu(UpMenu.curMenu.none);
                        BattleMain3.SetItem(itembuttons[0].GetComponent<InvSlot>().curItem);
                        BattleMain3.Player1().iteminv.RemoveFromStack(itembuttons[0].GetComponent<InvSlot>().curItem);
                        BattleMain3.InitTurn(BattleMain3.TurnType.item);
                    }
                    else if (go == itembuttons[1] && go2 == go)
                    {
                        UpMenu.SetMenu(UpMenu.curMenu.none);
                        BattleMain3.SetItem(itembuttons[1].GetComponent<InvSlot>().curItem);
                        BattleMain3.Player1().iteminv.RemoveFromStack(itembuttons[1].GetComponent<InvSlot>().curItem);
                        BattleMain3.InitTurn(BattleMain3.TurnType.item);

                    }
                    else if (go == itembuttons[2] && go2 == go)
                    {
                        UpMenu.SetMenu(UpMenu.curMenu.none);
                        BattleMain3.SetItem(itembuttons[2].GetComponent<InvSlot>().curItem);
                        BattleMain3.Player1().iteminv.RemoveFromStack(itembuttons[2].GetComponent<InvSlot>().curItem);
                        BattleMain3.InitTurn(BattleMain3.TurnType.item);

                    }
                    else if (go == itembuttons[3] && go2 == go)
                    {
                        UpMenu.SetMenu(UpMenu.curMenu.none);
                        BattleMain3.SetItem(itembuttons[3].GetComponent<InvSlot>().curItem);
                        BattleMain3.Player1().iteminv.RemoveFromStack(itembuttons[3].GetComponent<InvSlot>().curItem);
                        BattleMain3.InitTurn(BattleMain3.TurnType.item);

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
