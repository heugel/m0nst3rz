using System.Collections;
using UnityEngine.UI;
using UnityEngine;

public class Quest : MonoBehaviour {

    public static SubQuest thequest = new SubQuest();
    public Text talktext;
    public GameObject newquestbut, remindquestbut,leavebut;
    public GameObject turninbut;
    public GameObject intropanel, turninpanel, failpanel, successpanel;
    //private int screennum;

    public enum QuestType
    {
        type,
        move,
        ATK,
        SPD,
        HP,
        none
    }

    [System.Serializable]
    public class SubQuest
    {
        public QuestType QT;
        public Types.Type gettype;
        public Move.SubMove getmove;
        public double num;
        public string name;
        public string desc;
        public int pay;

        public SubQuest()
        {
            QT = QuestType.none;
            gettype = Types.Type.none;
            getmove = new Move.SubMove();
            num = 0;
            name = "";
            desc = "";
            pay = 0;
        }

        public void ClearQuest()
        {
            QT = QuestType.none;
            gettype = Types.Type.none;
            getmove = new Move.SubMove();
            num = 0;
            name = "";
            desc = "";
            pay = 0;
        }

        public void RandomQuest()
        {
            int typetest = (int)UnityEngine.Random.Range((int)0, (int)5);
            SubQuest temp = new SubQuest();

            switch (typetest)
            {
                case 0:
                    QT = QuestType.type;
                    gettype = Types.Int2Type((int)UnityEngine.Random.Range((int)1, (int)22));
                    desc = "Oi! I need you to get a young " + Types.TypePrinter(gettype) + " type m0nst3r for me!";
                    if(gettype!=Types.Type.spooky && gettype != Types.Type.amyschumer)
                        pay = (int)UnityEngine.Random.Range((int)100, (int)301);
                    else
                        pay = (int)UnityEngine.Random.Range((int)250, (int)501);
                    break;
                case 1:
                    QT = QuestType.move;
                    getmove = Lists.RandMove();
                    desc = "Oi oi! I need a young m0nst3r that knows the move " + getmove.Name+"!";
                    pay = (int)UnityEngine.Random.Range((int)100, (int)301);
                    break;
                case 2:
                    QT = QuestType.ATK;
                    num = (double)UnityEngine.Random.Range(50f, 95f);
                    num = System.Math.Round(num, 2);
                    desc = "Hey-o oi oi! I'm gonna need a m0nst3r with an ATK rating of over " + num + "!";
                    if (num < 90) pay = (int)(2 * ((num * num) / 50));
                    else pay = 1000;
                    break;
                case 3:
                    QT = QuestType.SPD;
                    num = (double)UnityEngine.Random.Range(50f, 95f);
                    num = System.Math.Round(num, 2);
                    desc = "Hey-o oi! I'm gonna need a m0nst3r with an SPD rating of over " + num + "!";
                    if (num < 90) pay = (int)(2 * ((num * num) / 50));
                    else pay = 1000;
                    break;
                case 4:
                    QT = QuestType.HP;
                    num = (double)UnityEngine.Random.Range(45f, 70f);
                    num = System.Math.Round(num, 2);
                    desc = "Hoi! I'm gonna need a m0nst3r with over " + num + " health!";
                    if (num < 60) pay = (int)(4 * ((num * num) / 45));
                    else pay = 2000;
                    break;
            }
        }
    }

    public bool QuestExists()
    {
        if (thequest.desc != "") return true;
        return false;
    }
    public void NewQuest()
    {
        turninbut.SetActive(true);
        thequest.RandomQuest();
        talktext.text = thequest.desc;
        talktext.text += "\nThe pay is " + thequest.pay;
    }
    public void Clear()
    {
        thequest.ClearQuest();
    }
    public void GetQuest()
    {
        talktext.text = thequest.desc;
        talktext.text += "\nThe pay is " + thequest.pay;
    }

    public void Leave()
    {
        gameObject.SetActive(false);
    }

    public void TurnInButton()
    {
        intropanel.SetActive(false);
        turninpanel.SetActive(true);
    }

    public void NVMBut()
    {
        intropanel.SetActive(true);
        turninpanel.SetActive(false);
    }

    public void TurnInBut()
    {
        //implement
    }

    public void IsComplete(Monster.SubMonster test)
    {
        turninpanel.SetActive(false);
        int thepay = thequest.pay;
        if (CheckComplete(test))
        {
            successpanel.SetActive(true);
            DowntownControl.GetPlayer().iteminv.AddCoins(thepay);
            DowntownControl.GetPlayer().moninv.RemoveMonster(test);
            thequest.ClearQuest();
        }
        else
        {
            failpanel.SetActive(true);
        }
    }

    public bool CheckComplete(Monster.SubMonster test)
    {
        switch (thequest.QT)
        {
            case QuestType.ATK:
                if (test.ATK > thequest.num)
                {
                    return true;
                }
                break;
            case QuestType.HP:
                if (test.maxhealth>thequest.num)
                {
                    return true;
                }
                break;
            case QuestType.move:
                if (test.KnowsMove(thequest.getmove))
                {
                    return true;
                }
                break;
            case QuestType.SPD:
                if (test.speed > thequest.num)
                {
                    return true;
                }
                break;
            case QuestType.type:
                if (test.Type == thequest.gettype)
                {
                    return true;
                }
                break;
            case QuestType.none:
                return false;
        }

        return false;
    }

    private void OnEnable()
    {
        intropanel.SetActive(true);

        successpanel.SetActive(false);
        failpanel.SetActive(false);
        turninpanel.SetActive(false);

        if (QuestExists()) remindquestbut.SetActive(true);
        else remindquestbut.SetActive(false);

        if (QuestExists()) turninbut.SetActive(true);
        else turninbut.SetActive(false);

        talktext.text = "Oi hey! How can I help yoi oi today?";
    }

    private bool moving = false;
    private GameObject go = null;
    private GameObject go2 = null;
    private bool onehit = true;
    private void Update()
    {
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
                        if(hit.transform.gameObject.GetComponent<InvSlot>()!=null)
                            if(hit.transform.gameObject.GetComponent<InvSlot>().curMonster!=null)
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

                        if (hit.transform.gameObject.GetComponent<InvSlot>() != null)
                        if (hit.transform.gameObject.GetComponent<InvSlot>().curMonster != null)
                            go2 = hit.transform.gameObject;
                    }
                    moving = false;

                if (go == go2)
                {
                    IsComplete(go.GetComponent<InvSlot>().curMonster);
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


    


