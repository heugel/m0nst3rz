using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class MoveMenu : MonoBehaviour
{

    public Move.SubMove[] themoves = new Move.SubMove[3];
    public GameObject[] cols = new GameObject[3];
    public GameObject[] texts = new GameObject[3];

    private Monster.SubMonster you = new Monster.SubMonster();

    void OnEnable()
    {
        
        you = BattleMain3.GetYou();

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
