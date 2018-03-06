using UnityEngine;
using System.Collections;

public class ItmMenuBattle : MonoBehaviour
{

    public Item[] items = new Item[4];
    public int[] stacks = new int[4];

    public GameObject[] itemslots = new GameObject[4];

    public TextMesh[] texts = new TextMesh[4];
    public TextMesh[] stacknums = new TextMesh[4];

    //private bool moving = false;
    //private GameObject go = null;
    //private GameObject go2 = null;
    //private bool onehit = true;

    //private bool activated = false;


    void OnEnable()
    {

        UpSlots();


    }
    // Use this for initialization
    //void Start (){}
    void UpSlots()
    {
        items = BattleMain3.Player1().iteminv.ReturnAllItems();
        stacks = BattleMain3.Player1().iteminv.ReturnAllStacks();
        for (int i = 0; i < 4; i++)
        {
            if (items[i] != null)
            {
                if (items[i].itemname != "")
                {
                    itemslots[i].GetComponent<SpriteRenderer>().sprite = Resources.Load("ItemSprites/" + items[i].imagepath, typeof(Sprite)) as Sprite;
                    itemslots[i].GetComponent<InvSlot>().curItem = items[i];
                    texts[i].text = items[i].ItemName();
                    stacknums[i].text = "" + stacks[i];
                    itemslots[i].GetComponent<BoxCollider>().enabled = true;
                }
                else
                {
                    texts[i].text = "";
                    stacknums[i].text = "";
                    itemslots[i].GetComponent<BoxCollider>().enabled = false;
                    itemslots[i].GetComponent<SpriteRenderer>().sprite = null;
                    //itemslots[i].GetComponent<SpriteRenderer>().color = Color.white;
                    itemslots[i].GetComponent<InvSlot>().curMonster = null;
                    itemslots[i].GetComponent<InvSlot>().curItem = null;
                    //acticons[i].GetComponent<InvSlot>().ISNULL = true;
                }
                //acticons[i].GetComponent<InvSlot>().ISNULL = false;
            }
            else
            {
                texts[i].text = "";
                stacknums[i].text = "";
                itemslots[i].GetComponent<BoxCollider>().enabled = false;
                itemslots[i].GetComponent<SpriteRenderer>().sprite = null;
                //itemslots[i].GetComponent<SpriteRenderer>().color = Color.white;
                itemslots[i].GetComponent<InvSlot>().curMonster = null;
                itemslots[i].GetComponent<InvSlot>().curItem = null;
                //acticons[i].GetComponent<InvSlot>().ISNULL = true;
            }
        }

    }
    
}
