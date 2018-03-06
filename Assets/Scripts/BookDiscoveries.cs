using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BookDiscoveries : MonoBehaviour {

    public GameObject slotmother;

    public GameObject upbut, downbut;

    private List<Monster.SubMonster> disc = new List<Monster.SubMonster>();
    private List<GameObject> places = new List<GameObject>();

    public float maxx, minx;
    public GameObject close;
    private Vector3 fingpos, origpos, temp1, temp2, temp3;
	// Use this for initialization
	void Awake ()
    {
        //print("awake");
        //closeplace = close.transform.position;
		for(int i=0; i<slotmother.transform.childCount; i++)
        {
            places.Add(slotmother.transform.GetChild(i).gameObject);
            //print(i);
        }
	}

    //public Monster test1;
    //private Monster.SubMonster test = new Monster.SubMonster();
    private void OnEnable()
    {
        close.SetActive(true);
        upbut.SetActive(true);
        downbut.SetActive(true);
        transform.position = Vector3.zero;
        temp1 = transform.position;
        //test=MonInit.MakeFromPrefab(test1);
        //MonInv.AddMonster(test);
        UpSlots();
    }

    void UpSlots()
    {
        disc = Town.GetPlayer().moninv.discovered;
        for(int i=0; i < disc.Count; i++)
        {
            places[i].GetComponent<SpriteRenderer>().sprite = Resources.Load("MonsterSprites/" + disc[i].imagepath, typeof(Sprite)) as Sprite;
        }
    }

    private bool moving = false;
    private GameObject go = null;
    private GameObject go2 = null;
    private bool onehit = true;
    // Update is called once per frame
    void Update ()
    {
        //close.transform.position = closeplace;
        if (Input.touchCount == 1 && onehit)
        {
            // touch on screen
            if (Input.GetTouch(0).phase == TouchPhase.Began)
            {
                origpos = Input.GetTouch(0).position;
                //fingpos = origpos;
                temp3 = Camera.main.ScreenToWorldPoint(origpos);
                //temp2 = temp3;
                Ray ray = Camera.main.ScreenPointToRay(Input.GetTouch(0).position);
                RaycastHit hit = new RaycastHit();
                moving = Physics.Raycast(ray, out hit);
                if (moving)
                {
                    go = hit.transform.gameObject;
                }
                else onehit = false;
            }

            //temp1 = fingpos;
            fingpos = Input.GetTouch(0).position;
            temp2 = Camera.main.ScreenToWorldPoint(fingpos);

            //if (temp2.y != temp3.y) //add room for error
            if (go != close)
            {
                if (go==downbut)
                {
                    temp1.y = Mathf.Lerp(transform.position.y, transform.position.y + 2, Time.deltaTime * 20);
                    transform.position = temp1;
                }
                if (go==upbut)
                {
                    temp1.y = Mathf.Lerp(transform.position.y, transform.position.y - 2, Time.deltaTime * 20);
                    transform.position = temp1;
                }
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

                if (go == close && go2 == close)
                {
                    close.SetActive(false);
                    upbut.SetActive(false);
                    downbut.SetActive(false);
                    BookControl.BookSet(false);
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
