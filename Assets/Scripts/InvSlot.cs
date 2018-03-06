using UnityEngine;
using System.Collections;

public class InvSlot : MonoBehaviour
{
    //public static Monster.SubMonster defmon;
    
    public Item curItem=null;
    public Monster.SubMonster curMonster = null; //=null necessary?
    public Move.SubMove curMove = null;
    private Vector3 origpos;
    public GameObject bg;
    //public bool ISNULL = true;
    public int slotnum = 0;
    public bool isTaken() { return ((curItem != null) || curMonster != null); }
    //public bool isNull() { return ISNULL; }
    /*private void Update()
    {
       // print(isTaken());
    }*/
    void Start()
    {
        origpos = transform.position;
        //curItem = new Item();
    }
    public void SnapBack()
    {
        transform.position = origpos;
    }
    public void Select()
    {
        bg.GetComponent<SpriteRenderer>().color = Color.red;
    }
    public void Deselect()
    {
        bg.GetComponent<SpriteRenderer>().color = Color.white;
    }
    public void Nullify()
    {
        curItem = null; curMonster = null;
    }
}