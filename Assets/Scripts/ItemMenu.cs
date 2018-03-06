using UnityEngine;
using System.Collections;

public class ItemMenu : MonoBehaviour
{

    public Item[] items = new Item[4]; //can be private

    public GameObject[] icons = new GameObject[4];
    public TextMesh[] stacks = new TextMesh[4];
    public TextMesh[] names = new TextMesh[4];
    public TextMesh coincount;
    public GameObject close;
    public GameObject delete;
    private bool moving = false;
    private GameObject go = null;
    private GameObject go2 = null;
    private bool onehit = true;
    private GameObject recsel = null;


    //public ItemPlaceholder test, test2;

    //private bool activated = false;
    public PlayerMain playerm;
    private PlayerMain.Player player;
    private void Awake()
    {
        player = playerm.GetPlayer();
    }

    void OnEnable()
    {
        //ItemInv.AddItem(test.theitem,1);
        //ItemInv.AddItem(test2.theitem, 1);

        if (recsel != null)
            recsel.GetComponent<InvSlot>().Deselect();

        recsel = null;
        UpSlots();

    }
    // Use this for initialization
    //void Start (){}
    void UpSlots()
    {
        coincount.text = "Coins: " + player.iteminv.Coins();
        Item[] temp = new Item[4];
        int[] stacksint = player.iteminv.ReturnAllStacks();
        for (int i = 0; i < 4; i++)
        {
            
            temp[i] = new Item();
            //icons[i].GetComponent<InvSlot>().Nullify();
            //icons[i].GetComponent<InvSlot>().curItem.theitem = new Item();
            items[i] = new Item();
        }
        
        temp = player.iteminv.ReturnAllItems();
        
        for (int i = 0; i < 4; i++)
        {
            items[i] = temp[i];
        }

        for (int i = 0; i < 4; i++)
        {
            //icons[i].GetComponent<InvSlot>().SnapBack();

            if (items[i] != null)
            {
                names[i].text = items[i].itemname;
                stacks[i].text = "" + stacksint[i];
                icons[i].GetComponent<SpriteRenderer>().sprite = Resources.Load("ItemSprites/" + items[i].imagepath, typeof(Sprite)) as Sprite;
                icons[i].GetComponent<InvSlot>().curItem = items[i];
                //icons[i].GetComponent<InvSlot>().SnapBack();
            }
            else
            {
                names[i].text = "";
                stacks[i].text = "";
                icons[i].GetComponent<SpriteRenderer>().sprite = null;
                icons[i].GetComponent<InvSlot>().curItem = null;
                //icons[i].GetComponent<InvSlot>().SnapBack();
            }
        }

    }
    // Update is called once per frame
    private Vector3 fingerpos, temp1, temp2;
    private GameObject selected = null;
    //private GameObject swaptarget = null;
    void Update()
    {

        if (Input.touchCount > 0 && onehit)
        {
            // touch on screen
            if (Input.GetTouch(0).phase == TouchPhase.Began)
            {
                if (selected != null)
                {
                    selected.GetComponent<InvSlot>().SnapBack();
                    selected.GetComponent<BoxCollider>().enabled = true;
                }

                Ray ray = Camera.main.ScreenPointToRay(Input.GetTouch(0).position);
                RaycastHit hit = new RaycastHit();
                moving = Physics.Raycast(ray, out hit);
                if (moving)
                {
                    go = hit.transform.gameObject;
                    if (go.GetComponent<InvSlot>() != null)
                    {
                        if (go.GetComponent<InvSlot>().curItem.itemname!="")
                        {
                            selected = go;

                            if (recsel != null)
                                recsel.GetComponent<InvSlot>().Deselect();

                            recsel = go;
                            recsel.GetComponent<InvSlot>().Select();
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
                temp2 = Camera.main.ScreenToWorldPoint(temp1);
                selected.transform.position = Vector3.Lerp(selected.transform.position, temp2, Time.deltaTime * 20);

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



                if (go2 == delete && selected != null) //check
                {
                    selected.GetComponent<InvSlot>().SnapBack();
                    selected.GetComponent<BoxCollider>().enabled = true;

                    player.iteminv.RemoveItem(selected.GetComponent<InvSlot>().curItem);

                    selected = null;
                    recsel.GetComponent<InvSlot>().Deselect(); recsel = null;

                    //ItemInv.RemoveItem(selected.GetComponent<InvSlot>().curItem);
                    UpSlots();
                }
                else if (go2!=null & selected != null)
                {
                    selected.GetComponent<InvSlot>().SnapBack();
                    selected.GetComponent<BoxCollider>().enabled = true;
                }
                else if (go2 != delete && selected != null)
                {
                    selected.GetComponent<InvSlot>().SnapBack();
                    selected.GetComponent<BoxCollider>().enabled = true;
                }


                if (go == close && go2 == close)
                {
                    InvCol.SetActive(false);
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
            //swaptarget = null;
        }
    }
}

