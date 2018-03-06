using UnityEngine;
using System.Collections;
using UnityEngine.Networking;
using UnityEngine.UI;

public class MAM : NetworkBehaviour
{

    public Move.SubMove[] themoves = new Move.SubMove[3];
    public GameObject[] cols = new GameObject[3];
    public GameObject[] texts = new GameObject[3];

    private Monster.SubMonster you = new Monster.SubMonster();
    private int i = 0;

    void OnEnable()
    {
        if (isLocalPlayer) i = 1;
        else i = 2;

        you = BMMulti.GetYou(i);

        UpSlots();

    }
    // Use this for initialization
    //void Start (){}
    void UpSlots()
    {

        for (int i = 0; i < 3; i++)
        {
            if (you.moves[i] != null)
            {
                themoves[i] = you.moves[i];
                texts[i].GetComponent<Text>().text = themoves[i].Name;
                cols[i].GetComponent<InvSlot>().curMove = themoves[i];
                cols[i].GetComponent<BoxCollider>().enabled = true;
                texts[i].GetComponent<Text>().color = Color.black;

                if (themoves[i].curPP == 0)
                {
                    cols[i].GetComponent<BoxCollider>().enabled = false;
                    texts[i].GetComponent<Text>().color = Color.red;
                }
            }
            else
            {
                texts[i].GetComponent<Text>().text = "";
                cols[i].GetComponent<InvSlot>().curMonster = null;
                cols[i].GetComponent<InvSlot>().curItem = null;
                cols[i].GetComponent<InvSlot>().curMove = null;
            }
        }

    }


}
