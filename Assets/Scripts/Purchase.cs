using UnityEngine;
using System.Collections;

public class Purchase : MonoBehaviour {


    public GameObject netplus, netminus, netnumber;
    public GameObject potionplus, potionminus, potionnumber;
    public GameObject tnplus, tnminus, tnnumber;

    public GameObject close;
    public GameObject buy;
    public TextMesh cost;
    public TextMesh playercoins;
    public GameObject notenough;
    public GameObject nospace;

    public ItemPlaceholder thenet, thepotion;//,thetypenet;

    private int nets = 0;
    private int potions = 0;
    //private int tnets = 0;
    //private Types.Type tntype = Types.Type.none;
    private int tndisplay = 0;
    private int costint = 0;
    private bool moving = false;
    private GameObject go = null;
    private GameObject go2 = null;
    private bool onehit = true;

    void OnEnable()
    {
        playercoins.text = "Coins: "+Town.GetPlayer().iteminv.Coins();
        nets = 0;
        costint = 0;
        potions = 0;
        tndisplay = 0;
        netnumber.GetComponent<TextMesh>().text = "" + 0;
        potionnumber.GetComponent<TextMesh>().text = "" + 0;
        //tnnumber.GetComponent<TextMesh>().text = "" + 0;
    }
	
	// Update is called once per frame
	void Update ()
    {
        costint = nets * thenet.theitem.cost + potions * thepotion.theitem.cost;
        cost.text = "Cost: " + costint;

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
                if (go == netminus && nets > 0 && go==go2)
                    nets--;
                else if (go == netplus && nets < 99 && go == go2)
                    nets++;
                else if (go == tnminus && tndisplay > 0 && go == go2)
                    tndisplay--;
                else if (go == tnplus && tndisplay < 21 && go == go2)
                    tndisplay++;
                else if (go == tnminus && tndisplay == 0 && go == go2)
                    tndisplay = 20;
                else if (go == tnplus && tndisplay ==21 && go == go2)
                    tndisplay=0;
                else if (go == potionminus && potions > 0 && go == go2)
                    potions--;
                else if (go == potionplus && potions < 99 && go == go2)
                    potions++;
                else if (go == close && go == go2)
                    ShopMain.ShopSet(false);
                else if (go == buy && go == go2)
                {
                    FinalPurchase();
                    
                }
                //Debug.Log("Touch Released from : " + go.name);
            }
        }
        else
        {
            go = null;
            onehit = true;
            go2 = null;
            netnumber.GetComponent<TextMesh>().text = "" + nets;
            potionnumber.GetComponent<TextMesh>().text = "" + potions;
            if (tndisplay != 18)
                tnnumber.GetComponent<TextMesh>().text = Types.TypePrinter(tndisplay);
            else tnnumber.GetComponent<TextMesh>().text = "Amy\nSchumer";
            
        }
    }

    void FinalPurchase()
    {
        costint = nets * thenet.theitem.cost + potions * thepotion.theitem.cost;
        Item tempitem = new Item(thenet.theitem);
        Item temppot = new Item(thepotion.theitem);
        tempitem.nettype = Types.Int2Type(tndisplay);
        if(tndisplay!=0)
            tempitem.itemname = Types.TypePrinter(tndisplay)+" "+thenet.theitem.itemname;
        if (Town.GetPlayer().iteminv.Coins() >= costint)
        {
            if (nets > 0 && potions > 0)
            {
                if (Town.GetPlayer().iteminv.HasSpaceFor(tempitem) && Town.GetPlayer().iteminv.HasSpaceFor(temppot))
                {
                    Town.GetPlayer().iteminv.SubstractCoins(costint);
                    Town.GetPlayer().iteminv.AddItem(tempitem, nets);
                    Town.GetPlayer().iteminv.AddItem(temppot, potions);
                    //play sound chaching
                }
                else Town.ShopError(nospace);
            }
            else if(nets>0 && potions == 0)
            {
                if (Town.GetPlayer().iteminv.HasSpaceFor(tempitem))
                {
                    Town.GetPlayer().iteminv.SubstractCoins(costint);
                    Town.GetPlayer().iteminv.AddItem(tempitem, nets);
                    //play sound chaching
                }
                else Town.ShopError(nospace);
            }
            else if (nets == 0 && potions > 0)
            {
                if (Town.GetPlayer().iteminv.HasSpaceFor(tempitem))
                {
                    Town.GetPlayer().iteminv.SubstractCoins(costint);
                    Town.GetPlayer().iteminv.AddItem(temppot, potions);
                    //play sound chaching
                }
                else Town.ShopError(nospace);
            }
        }
        else Town.ShopError(notenough);

        ShopMain.ShopSet(false);
    }

}
