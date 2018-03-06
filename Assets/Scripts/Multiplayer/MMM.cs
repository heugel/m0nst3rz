using UnityEngine;
using UnityEngine.Networking;
using System.Collections;

public class MMM : NetworkBehaviour
{

    public Monster.SubMonster[] acts = new Monster.SubMonster[3];

    public GameObject[] acticons = new GameObject[3];

    public TextMesh[] texts = new TextMesh[3];

    private int i=0;

    void OnEnable()
    {
        if (isLocalPlayer) i = 1;
        else i = 2;

        UpSlots();

    }
    // Use this for initialization
    //void Start (){}
    void UpSlots()
    {
        acts = BMMulti.PlayerPick(i).moninv.ReturnActives();
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
                    if (acts[i] == BMMulti.GetYou(i))
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
                texts[i].text = "";
                acticons[i].GetComponent<BoxCollider>().enabled = false;
                acticons[i].GetComponent<SpriteRenderer>().sprite = null;
                acticons[i].GetComponent<SpriteRenderer>().color = Color.white;
                acticons[i].GetComponent<InvSlot>().curMonster = null;
                acticons[i].GetComponent<InvSlot>().curItem = null;
                //acticons[i].GetComponent<InvSlot>().ISNULL = true;
            }
        }

    }

}
