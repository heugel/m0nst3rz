using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MonInit : MonoBehaviour {

    //private int i;

    public static bool IsActive() { return instance != null; }
    private static MonInit curInstance;
    private static MonInit instance { get { return curInstance; } }

    public static void GiveFingerPrint(Monster.SubMonster mon)
    {
        Random.InitState((int)System.DateTime.Now.Ticks);
        //instance.i++;
        mon.fingerprint = mon.Name+Random.value;
    }

    public static Monster.SubMonster MakeFromPrefab(Monster mon)
    {
        //print("gets here");
        Monster.SubMonster temp = new Monster.SubMonster(mon.themonster);       
        
        temp.MaxHealthInit(true);
        temp.SpeedInit();
        temp = RandomMoves(temp);
        GiveFingerPrint(temp);
        return temp;
    }
    
    public static Monster.SubMonster MakeFromPrefab(Monster.SubMonster mon)
    {
        Monster.SubMonster temp = new Monster.SubMonster(mon);
        
        temp.MaxHealthInit(true);
        temp = RandomMoves(temp);
        temp.SpeedInit();
        GiveFingerPrint(temp);
        return temp;
    }

    public static Monster.SubMonster RandomFromPrefab(Monster mon)
    {
        Monster.SubMonster temp = new Monster.SubMonster(mon.themonster);
        temp = RandomMoves(temp);
        
        temp.MaxHealthInit(false);
        GiveFingerPrint(temp);
        return temp;
    }
    public static Monster.SubMonster RandomFromPrefab(Monster.SubMonster mon)
    {
        Monster.SubMonster temp = new Monster.SubMonster(mon);
        temp = RandomMoves(temp);
        temp.SpeedInit();
        temp.MaxHealthInit(false);
        GiveFingerPrint(temp);
        return temp;
    }
    public static Monster.SubMonster RandomMoves(Monster.SubMonster mon)
    {
        Move[] temp = Lists.Moves();
        List<int> done = new List<int>();
        List<Move> potentials = new List<Move>();
        int randi;

        for(int i=0; i<temp.Length;i++)
        {
            for(int j=0; j < mon.compatmovetypes.Length; j++)
            {
                if (temp[i].themove.type == mon.compatmovetypes[j])
                {
                    potentials.Add(temp[i]);
                }
            }
        }

        randi = Random.Range((int)0, (int)potentials.Count);
        mon.moves[0] = new Move.SubMove(potentials[randi].themove);
        done.Add(randi);
        for (int k = 1; k < 3; k++)
        {
            randi = Random.Range((int)0, (int)potentials.Count);
            if (!done.Contains(randi))
            {
                done.Add(randi);
                mon.moves[k] = new Move.SubMove(potentials[randi].themove);
            }
            else k--;
        }

        return new Monster.SubMonster(mon);
    }

    void Start()
    {
        if (curInstance == null) { curInstance = this; }
        //i = 0;
    }

    }
