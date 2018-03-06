using UnityEngine;
using System.Collections;

public class PickTwo : MonoBehaviour {

    public PlayerMain player;
    public GameObject towncontrol;
    //public GameObject gameover;
    public Monster[] set1 = new Monster[3];
    public Monster[] set2 = new Monster[3];

    public GameObject[] acticons = new GameObject[3];

    public GameObject welcome, second;
    private int picker = 1;

    private bool moving = false;
    private GameObject go = null;
    private GameObject go2 = null;
    private bool onehit = true;

    
    void SaveCallBack()
    {

    }

    void Update ()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            player.GetPlayer().moninv.AddMonster(MonInit.MakeFromPrefab(acticons[2].GetComponent<InvSlot>().curMonster));
            //MonInv.AddMonster(MonInit.MakeFromPrefab(acticons[2].GetComponent<InvSlot>().curMonster));
            //MonInv.AddMonster(MonInit.MakeFromPrefab(acticons[2].GetComponent<InvSlot>().curMonster));
            picker++;
            SaveLoad.Save(SaveCallBack);
        }

        if (GlobalData.pickstart == false)
        {
            //SaveLoad.Save(SaveCallBack);
            towncontrol.SetActive(true);
            gameObject.SetActive(false);
            
        }
        else
        {
            towncontrol.SetActive(false);
        }

        if (picker == 1)
        {
            welcome.SetActive(true);
            for (int i = 0; i < 3; i++)
            {
                acticons[i].GetComponent<InvSlot>().curMonster = set1[i].themonster;
                acticons[i].GetComponent<SpriteRenderer>().sprite = Resources.Load("MonsterSprites/" + set1[i].themonster.imagepath, typeof(Sprite)) as Sprite;
            }
        }
        else if (picker == 2)
        {
            welcome.SetActive(false);
            second.SetActive(true);
            for (int i = 0; i < 3; i++)
            {
                acticons[i].GetComponent<InvSlot>().curMonster = set2[i].themonster;
                acticons[i].GetComponent<SpriteRenderer>().sprite = Resources.Load("MonsterSprites/" + set2[i].themonster.imagepath, typeof(Sprite)) as Sprite;

            }
        }
        else { towncontrol.SetActive(true); GlobalData.pickstart = false; }

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
                //moving = false;
                //Debug.Log("Touch Released from : " + go.name);
                if (go == acticons[0] && go==go2)
                {
                    player.GetPlayer().moninv.AddMonster(MonInit.MakeFromPrefab(acticons[0].GetComponent<InvSlot>().curMonster));
                    //MonInv.AddMonster(MonInit.MakeFromPrefab(acticons[0].GetComponent<InvSlot>().curMonster));
                    //MonInv.AddMonster(MonInit.MakeFromPrefab(acticons[0].GetComponent<InvSlot>().curMonster));
                    picker++;
                }
                else if (go == acticons[1] && go == go2)
                {
                    player.GetPlayer().moninv.AddMonster(MonInit.MakeFromPrefab(acticons[1].GetComponent<InvSlot>().curMonster));
                    //MonInv.AddMonster(MonInit.MakeFromPrefab(acticons[1].GetComponent<InvSlot>().curMonster));
                    //MonInv.AddMonster(MonInit.MakeFromPrefab(acticons[1].GetComponent<InvSlot>().curMonster));
                    picker++;
                }
                else if (go == acticons[2] && go == go2)
                {
                    player.GetPlayer().moninv.AddMonster(MonInit.MakeFromPrefab(acticons[2].GetComponent<InvSlot>().curMonster));
                    //MonInv.AddMonster(MonInit.MakeFromPrefab(acticons[2].GetComponent<InvSlot>().curMonster));
                    //MonInv.AddMonster(MonInit.MakeFromPrefab(acticons[2].GetComponent<InvSlot>().curMonster));
                    picker++;
                }
                }
            }
        else
        {
            go = null;
            go2 = null;
            onehit = true; //im pretty sure this is completely fucking unnecessary
        }

    }
}
