using UnityEngine;
using UnityEngine.SceneManagement;
using System;

[System.Serializable]
public class ItemInv
{

    public InvSpot[] items = new InvSpot[4];
    public int coins = 0;

    public ItemInv()
    {
        coins = 0;
        for (int i = 0; i < 4; i++)   
        {
            items[i] = new InvSpot();
        }
    }


    public  void AddCoins(int addcoin)
    {
        coins += addcoin;
    }
    public void SubstractCoins(int subcoin)
    {
        coins -= subcoin;
        if (coins < 0)
            coins = 0;
    }
    public  int Coins() { return coins; }

    public  void AddItem(Item newitem, int numitems)
    {
        if (numitems > 0)
        {
            if (HasItem(newitem))
            {
                int foundref = FindReference(newitem);
                AddToStack(foundref, numitems);
            }
            else
            {
                bool hasadded = false;
                for (int i = 0; i < items.Length; i++)
                {
                    if (items[i].CurStack() == 0 && !hasadded)
                    {
                        items[i].AssignItem(newitem,numitems);
                        hasadded = true;
                    }
                }
            }
        }
    }
    public  bool HasItem(Item testitem)
    {
        for (int i = 0; i < items.Length; i++)
        {
            if (!items[i].NullItem())
            {
                if (items[i].ItemName() == testitem.ItemName())
                {
                    return true;
                }
            }
        }
        return false;
    }
    public  bool IsFull()
    {
        for (int i = 0; i < items.Length; i++)
        {
            if (items[i].NullItem())
            {
                return false;
            }
        }
        return true;
    }
    public  bool HasSpaceFor(Item testitem)
    {
        if (HasItem(testitem)) return true;
        if (!IsFull()) return true;

        return false;
    }
    public  int FindReference(Item refitem)
    {
        for (int i = 0; i < items.Length; i++)
        {
            if (!items[i].NullItem())
            {
                if (items[i].ItemName() == refitem.ItemName())
                {
                    return i;
                }
            }
        }
        return 100;
    }
    public  void AddToStack(int refspot, int numitems)
    {
        items[refspot].AddtoStack(numitems);
    }
    /*public  void UseItem(Item useitem) //might not need this
    {
        if (useitem.IT == Item.ItemType.potion)
        {
            //heal selected monster
        }
        else if (useitem.IT == Item.ItemType.net)
        {
            //basic net
        }
        else if (useitem.IT == Item.ItemType.typenet)
        {
            //type specific net
        }

        RemoveFromStack(useitem); //check this
        //InvMenu.UpdateItems();
    }*/
    public  void RemoveItem(Item rmitem)
    {
        if (HasItem(rmitem))
        {
            int i = FindReference(rmitem);
            items[i].Nullify();
        }
    }
    public  void RemoveFromStack(Item rmitem)
    {
        if (HasItem(rmitem))
        {
            int i = FindReference(rmitem);
            items[i].TakefromStack();
        }
    }
    public  Item[] ReturnAllItems()
    {
        Item[] final = new Item[4];
        for (int i = 0; i < 4; i++)
        {
            final[i] = items[i].ReturnItem();
        }
        return final;
    }
    public  int[] ReturnAllStacks()
    {
        int[] final = new int[4];
        for (int i = 0; i < 4; i++)
        {
            final[i] = items[i].CurStack();
        }
        return final;
    }
    // Use this for initialization
    /*void Start()
    {
        if (curInstance == null) { curInstance = this; }

        for (int i = 0; i < 4; i++)
        {
            items[i] = new InvSpot();
        }
    }*/


    [System.Serializable]
    public class InvSpot
    {
        private Item item;
        private int stack; //make private
        public InvSpot()
        {
            item = null;
            stack = 0;
        }
        public void AssignItem(Item newitem, int numstack)
        {
            stack = numstack;
            item = newitem;
        }
        public int CurStack() { return stack; }
        public Item ReturnItem() { return item; }
        public void AddtoStack(int howmany) { stack+=howmany; }
        public void Nullify() { item = null; stack = 0; }
        public bool NullItem() { return item == null; }
        public string ItemName() { return item.ItemName(); }
        public void TakefromStack()
        {
            if (stack > 0)
                stack--;
            if (stack == 0)
                item = null;
        }
    }
}