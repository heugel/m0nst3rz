using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class MonInvMenu : MonoBehaviour {

    public GameObject CNB;
    public InputField inputfield;
    public GameObject touchtochange;

    private Monster.SubMonster[] acts = new Monster.SubMonster[3];
    private Monster.SubMonster[] backs = new Monster.SubMonster[10];

    public GameObject[] acticons = new GameObject[3];
    public GameObject[] backicons = new GameObject[10];

    public GameObject close, setfree;
    public PostMon info;

    private bool moving = false;
    private GameObject go = null;
    private GameObject go2 = null;
    private bool onehit = true;
    private GameObject recsel = null;
    private int swapper;

    private bool STOP = false;

    public PlayerMain playerm;
    private PlayerMain.Player player;
    private void Awake()
    {
        player = playerm.GetPlayer();
    }

    public void StartChangeName()
    {
        STOP = true;
        CNB.SetActive(true);
        inputfield.gameObject.SetActive(true);
        inputfield.text = "";
    }

    public void FinalChangeName()
    {
        if (inputfield.text != "")
        {
            STOP = false;
            CNB.SetActive(false);
            recsel.GetComponent<InvSlot>().curMonster.Name = inputfield.text;
            inputfield.gameObject.SetActive(false);
            info.Set(recsel.GetComponent<InvSlot>().curMonster);
        }

    }

    void OnEnable()
    {
        STOP = false;
        touchtochange.SetActive(false);
        info.Erase();
        if (recsel != null)
            recsel.GetComponent<InvSlot>().Deselect();

        recsel = null;
        UpSlots();


    }
    // Use this for initialization
    //void Start (){}
	void UpSlots()
    {
        acts=player.moninv.ReturnActives();
        for(int i=0; i<3; i++)
        {
            //acticons[i].GetComponent<InvSlot>().SnapBack();

            if (acts[i] != null)
            {

                acticons[i].GetComponent<SpriteRenderer>().sprite = Resources.Load("MonsterSprites/"+acts[i].imagepath, typeof(Sprite)) as Sprite;
                acticons[i].GetComponent<InvSlot>().curMonster = acts[i];
                //acticons[i].GetComponent<InvSlot>().ISNULL = false;
            }
            else
            {
                acticons[i].GetComponent<SpriteRenderer>().sprite = null;
                acticons[i].GetComponent<InvSlot>().curMonster = null;
                acticons[i].GetComponent<InvSlot>().curItem = null;
                //acticons[i].GetComponent<InvSlot>().ISNULL = true;
            }
        }
        backs = player.moninv.ReturnMain();
        for (int i = 0; i < 10; i++)
        {
            //backicons[i].GetComponent<InvSlot>().SnapBack();

            if (backs[i] != null)
            {
                backicons[i].GetComponent<SpriteRenderer>().sprite = Resources.Load("MonsterSprites/" + backs[i].imagepath, typeof(Sprite)) as Sprite;
                backicons[i].GetComponent<InvSlot>().curMonster = backs[i];
                //backicons[i].GetComponent<InvSlot>().ISNULL = false;

            }
            else
            {
                backicons[i].GetComponent<SpriteRenderer>().sprite = null;
                backicons[i].GetComponent<InvSlot>().curMonster = null;
                backicons[i].GetComponent<InvSlot>().curItem = null;
                //backicons[i].GetComponent<InvSlot>().ISNULL = true;


            }
        }

    }
    // Update is called once per frame
    private Vector3 fingerpos, temp1, temp2;
    private GameObject selected=null;
    private GameObject swaptarget = null;
	void Update ()
    {
        if (recsel != null)
        {
            touchtochange.SetActive(true);
        }
        else touchtochange.SetActive(false);

        if (Input.touchCount >0 && onehit && !STOP)
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
                        if (go.GetComponent<InvSlot>()!=null)
                        {
                            if (go.GetComponent<InvSlot>().isTaken())
                            {
                                selected = go;

                                if (recsel != null)
                                    recsel.GetComponent<InvSlot>().Deselect();

                                recsel = go;
                                recsel.GetComponent<InvSlot>().Select();
                            info.Set(recsel.GetComponent<InvSlot>().curMonster);
                            }
                        }
                        //Debug.Log("Touch Detected on : " + go.name);
                    }
                    else onehit = false;
                }

                if (selected != null)
                {
                
                    selected.GetComponent<BoxCollider>().enabled = false;
                    fingerpos = Input.GetTouch(0).position;
                    temp1 = fingerpos;
                    temp1.z = 3;
                    temp2= Camera.main.ScreenToWorldPoint(temp1);
                    selected.transform.position = Vector3.Lerp(selected.transform.position, temp2, Time.deltaTime*20);
            }

                // release touch/dragging
                if ((Input.GetTouch(0).phase == TouchPhase.Ended || Input.GetTouch(0).phase == TouchPhase.Canceled || Input.touchCount != 1) && go != null && onehit)
                {
                    //moving = false;

                Ray ray = Camera.main.ScreenPointToRay(Input.GetTouch(0).position);
                RaycastHit hit = new RaycastHit();
                moving = Physics.Raycast(ray, out hit);
                if (moving)
                {
                    go2 = hit.transform.gameObject;
                    moving = false;
                }

                if (go2 == close && selected!=null)
                {
                    selected.GetComponent<InvSlot>().SnapBack();
                    selected.GetComponent<BoxCollider>().enabled = true;
                }
                else if(go2==setfree && selected != null &&!player.moninv.OnlyOne())
                {
                    selected.GetComponent<InvSlot>().SnapBack();
                    selected.GetComponent<BoxCollider>().enabled = true;

                    player.moninv.RemoveMonster(selected.GetComponent<InvSlot>().curMonster);

                    selected = null; swaptarget = null;
                    recsel.GetComponent<InvSlot>().Deselect(); recsel = null;
                    info.Erase();
                    UpSlots();
                } //else if (onlyone) show "cant throw away only monster"
                else if (go2 == setfree && selected != null && player.moninv.OnlyOne())
                {
                    selected.GetComponent<InvSlot>().SnapBack();
                    selected.GetComponent<BoxCollider>().enabled = true;
                }
                else if (go2!=null && selected !=null)
                {
                    if (go2.GetComponent<InvSlot>() != null)
                    {
                        if (go2.GetComponent<InvSlot>().isTaken())
                        {
                            swaptarget = go2;
                            selected.GetComponent<InvSlot>().SnapBack();
                            selected.GetComponent<BoxCollider>().enabled = true;
                            player.moninv.SwapPos(selected.GetComponent<InvSlot>().curMonster, swaptarget.GetComponent<InvSlot>().curMonster);
                            selected = null; swaptarget = null;
                            recsel.GetComponent<InvSlot>().Deselect();  recsel = null;
                            info.Erase();
                            UpSlots();
                        }
                        else
                        {
                            
                            swapper = go2.GetComponent<InvSlot>().slotnum;
                            selected.GetComponent<InvSlot>().SnapBack();
                            selected.GetComponent<BoxCollider>().enabled = true;
                            player.moninv.Move2Empty(selected.GetComponent<InvSlot>().curMonster, swapper);
                            selected = null; swaptarget = null;
                            recsel.GetComponent<InvSlot>().Deselect(); recsel = null;
                            info.Erase();
                            UpSlots();
                        }

                    }
                }

                else if (selected!=null)
                {
                    selected.GetComponent<InvSlot>().SnapBack();
                    selected.GetComponent<BoxCollider>().enabled = true;
                }
                if (go == close && go2 == close)
                {
                    //selected.GetComponent<InvSlot>().SnapBack();
                    //selected.GetComponent<BoxCollider>().enabled = true;
                    MICollider.SetActive(false);
                }

                moving = false;

            }
         }        
        else
        {
            go = null;
            go2 = null;
            onehit = true;
            selected = null;
            swaptarget = null;
        }
    }
}
