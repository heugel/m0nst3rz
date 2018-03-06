using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using System;

public class PlayerMain : MonoBehaviour {

    public Player theplayer = new Player();

    [System.Serializable]
    public class Player
    {
        public MonInv moninv;
        public ItemInv iteminv;

        public Player()
        {
            moninv = new MonInv();
            iteminv = new ItemInv();
        }
    }

    public Player GetPlayer() { return theplayer; }
    public void SetPlayer(Player newplayer) { theplayer = newplayer; }

    public static bool IsActive() { return instance != null; }
    private static PlayerMain curInstance;
    private static PlayerMain instance { get { return curInstance; } }

    // Use this for initialization
    void Start ()
    {
        if (curInstance == null) { curInstance = this; }

        for (int i = 0; i < 4; i++)
        {
            theplayer.iteminv.items[i] = new ItemInv.InvSpot();
        }
        if (!GlobalData.loadGame)
        {
            for (int i = 0; i < 13; i++)
            {
                theplayer.moninv.monsters[i] = null;
            }
        }

        
        /*if (GlobalData.loadGame)
        {
            //print("load!");
            SaveLoad.Load(SaveCallBack);
        }*/

    }

    private bool doneyet = false;
	// Update is called once per frame
	void Update ()
    {
        if (loadPending != null)
            LoadHelper();
        if (savePending != null)
            SaveHelper();

        /*if (GlobalData.IsInstantiated()&&GlobalData.loadGame && !doneyet)
        {
            SaveLoad.Load(SaveCallBack);
            doneyet = true;
        }*/

    }

    private static Action loadPending = null;
    private static Action savePending = null;
    public static void Load(Action LoadCallBack) { loadPending = LoadCallBack; }
    public static void Save(Action SaveCallBack) { savePending = SaveCallBack; }

    private void LoadHelper()
    {
        Monster.SubMonster[] savedmonsters = SaveLoad.savedgame.GetMonsters();
        List<Monster.SubMonster> saveddisc = SaveLoad.savedgame.GetDisc();
        List<string> savednames = SaveLoad.savedgame.GetNames();
        ItemInv.InvSpot[] savedItems = SaveLoad.savedgame.GetInventory();

        if (savedItems != null)
            theplayer.iteminv.items = savedItems;

        theplayer.iteminv.coins = SaveLoad.savedgame.GetCoins();

        if (savedmonsters != null)
            theplayer.moninv.monsters = savedmonsters;
        if (saveddisc != null)
            theplayer.moninv.discovered = saveddisc;
        if (savednames != null)
            theplayer.moninv.discnames = savednames;

        loadPending();
        loadPending = null;
    }

    private void SaveHelper()
    {
        SaveLoad.savedgame.SetInventory(theplayer.iteminv.items);
        SaveLoad.savedgame.SetCoins(theplayer.iteminv.coins);

        SaveLoad.savedgame.SetMonsters(theplayer.moninv.monsters);
        SaveLoad.savedgame.SetDisc(theplayer.moninv.discovered);
        SaveLoad.savedgame.SetNames(theplayer.moninv.discnames);

        savePending();
        savePending = null;
    }

    //private void OnEnable()
    //{

    //}

    void SaveCallBack()
    {

    }
}
