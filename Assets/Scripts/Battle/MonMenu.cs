using UnityEngine;
using System.Collections;

public class MonMenu : MonoBehaviour
{

    public Monster.SubMonster[] acts = new Monster.SubMonster[3];

    public GameObject[] acticons = new GameObject[3];

    public TextMesh[] texts = new TextMesh[3];

    //private bool moving = false;
    //private GameObject go = null;
    //private GameObject go2 = null;
    //private bool onehit = true;

    //private Monster.SubMonster[] curactives = new Monster.SubMonster[3];
    //private bool activated = false;


    void OnEnable()
    {

        UpSlots();
        //curactives = MonInv.ReturnActives();

    }
    // Use this for initialization
    //void Start (){}
    void UpSlots()
    {
        acts = BattleMain3.Player1().moninv.ReturnActives();
        for (int i = 0; i < 3; i++)
        {
            if (acts[i] != null)
            {
                if (acts[i].Name != "")
                {
                    acticons[i].GetComponent<SpriteRenderer>().sprite = Resources.Load("MonsterSprites/" + acts[i].imagepath, typeof(Sprite)) as Sprite;
                    acticons[i].GetComponent<InvSlot>().curMonster = acts[i];
                    texts[i].text = acts[i].Name;
                    acticons[i].GetComponent<SpriteRenderer>().color = Color.white;
                    acticons[i].GetComponent<BoxCollider>().enabled = true;
                    if (acts[i] == BattleMain3.GetYou())
                    {
                        acticons[i].GetComponent<SpriteRenderer>().color = Color.gray;
                        acticons[i].GetComponent<BoxCollider>().enabled = false;
                    }
                }
                else
                {
                    texts[i].text = "";
                    acticons[i].GetComponent<BoxCollider>().enabled = false;
                    acticons[i].GetComponent<SpriteRenderer>().sprite = null;
                    acticons[i].GetComponent<SpriteRenderer>().color = Color.white;
                    acticons[i].GetComponent<InvSlot>().curMonster = null;
                    acticons[i].GetComponent<InvSlot>().curItem = null;
                }
                //acticons[i].GetComponent<InvSlot>().ISNULL = false;
            }
            else
            {
                texts[i].text = "" ;
                acticons[i].GetComponent<BoxCollider>().enabled = false;
                acticons[i].GetComponent<SpriteRenderer>().sprite = null;
                acticons[i].GetComponent<SpriteRenderer>().color = Color.white;
                acticons[i].GetComponent<InvSlot>().curMonster = null;
                acticons[i].GetComponent<InvSlot>().curItem = null;
                //acticons[i].GetComponent<InvSlot>().ISNULL = true;
            }
        }

    }
    // Update is called once per frame

    void Update()
    {



        /*if (Input.touchCount > 0 && onehit)
        {
            // touch on screen
            if (Input.GetTouch(0).phase == TouchPhase.Began)
            {
                Ray ray = Camera.main.ScreenPointToRay(Input.GetTouch(0).position);
                RaycastHit hit = new RaycastHit();
                moving = Physics.Raycast(ray, out hit);
                if (moving)
                {
                    if (go.GetComponent<InvSlot>() != null)
                    {
                        if(go.GetComponent<InvSlot>().isTaken())
                            go = hit.transform.gameObject;

                    }
                    //Debug.Log("Touch Detected on : " + go.name);
                }
                else onehit = false;
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

                if (go == acticons[0] && go2 == go)
                {
                    BattleMain3.SetYou(curactives[0]);
                    UpMenu.SetMenu(UpMenu.curMenu.none);
                    if(!BattleMain3.deadselect)
                        BattleMain3.DoTurn(BattleMain3.TurnType.change);
                }
                if (go == acticons[1] && go2 == go)
                {
                    BattleMain3.SetYou(curactives[1]);
                    UpMenu.SetMenu(UpMenu.curMenu.none);
                    if (!BattleMain3.deadselect)
                        BattleMain3.DoTurn(BattleMain3.TurnType.change);
                }
                if (go == acticons[2] && go2 == go)
                {
                    BattleMain3.SetYou(curactives[2]);
                    UpMenu.SetMenu(UpMenu.curMenu.none);
                    if (!BattleMain3.deadselect)
                        BattleMain3.DoTurn(BattleMain3.TurnType.change);
                }

                moving = false;

            }
        }
        else
        {
            go = null;
            go2 = null;
            onehit = true;
        }*/
    }
}
