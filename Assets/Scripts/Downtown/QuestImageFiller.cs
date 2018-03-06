using System.Collections;
using UnityEngine.UI;
using UnityEngine;

public class QuestImageFiller : MonoBehaviour
{

    private GameObject[] images = new GameObject[13];

    private void Awake()
    {
        for (int i = 0; i < 13; i++)
        {
            images[i] = transform.GetChild(i).gameObject;
        }
    }


    private void OnEnable()
    {

        Monster.SubMonster[] acts = DowntownControl.GetPlayer().moninv.ReturnActives();
        for (int i = 0; i < 3; i++)
        {
            //acticons[i].GetComponent<InvSlot>().SnapBack();

            if (acts[i] != null)
            {

                images[i].GetComponent<Image>().sprite = Resources.Load("MonsterSprites/" + acts[i].imagepath, typeof(Sprite)) as Sprite;
                images[i].GetComponent<InvSlot>().curMonster = acts[i];
                //images[i].GetComponent<InvSlot>().ISNULL = false;
            }
            else
            {
                images[i].GetComponent<Image>().sprite = null;
              
                images[i].GetComponent<InvSlot>().curMonster = null;
                images[i].GetComponent<InvSlot>().curItem = null;
                //acticons[i].GetComponent<InvSlot>().ISNULL = true;
            }


        }

        Monster.SubMonster[] backs = DowntownControl.GetPlayer().moninv.ReturnMain();

        for (int i = 0; i < 10; i++)
        {
            if (backs[i] != null)
            {
                images[i + 3].GetComponent<Image>().sprite = Resources.Load("MonsterSprites/" + backs[i].imagepath, typeof(Sprite)) as Sprite;
                images[i + 3].GetComponent<InvSlot>().curMonster = backs[i];
                //images[i].GetComponent<InvSlot>().ISNULL = false;

            }
            else
            {
                images[i + 3].GetComponent<Image>().sprite = null;
                images[i + 3].GetComponent<InvSlot>().curMonster = null;
                images[i + 3].GetComponent<InvSlot>().curItem = null;
                //images[i].GetComponent<InvSlot>().ISNULL = true;


            }
        }
    }
}
