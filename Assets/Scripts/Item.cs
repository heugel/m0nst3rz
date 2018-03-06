using UnityEngine;
using System.Collections;

[System.Serializable]
public class Item
{
    public string itemname;
    public ItemType IT;
    public Types.Type nettype;
    public int cost;
    public string imagepath;

    public Item()
    {
        imagepath = "";
        IT = ItemType.net;
        itemname = "";
        cost = 0;
        nettype = Types.Type.none;
    }
    public Item(Item copy)
    {
        imagepath = copy.imagepath;
        IT = copy.IT;
        itemname = copy.itemname;
        cost = copy.cost;
        nettype = copy.nettype;
    }

    public enum ItemType
    {
        net,
        typenet,
        potion
    }


    public string ItemName() { return itemname; }

}
