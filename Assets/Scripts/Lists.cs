using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lists : MonoBehaviour {

    private Monster[] allmonsters;
    private Move[] allmoves;



    public static bool IsActive() { return instance != null; }
    private static Lists curInstance;
    private static Lists instance { get { return curInstance; } }

    public static Monster[] Monsters()
    {
        return instance.allmonsters;
    }
    public static Move[] Moves()
    {
        return instance.allmoves;
    }

    public static Monster.SubMonster RandMon()
    {
        Random.InitState((int)System.DateTime.Now.Ticks);
        int i = Random.Range(0, instance.allmonsters.Length);
        Monster.SubMonster temp = new Monster.SubMonster();//instance.allmonsters[i].themonster);
        temp = MonInit.MakeFromPrefab(instance.allmonsters[i]);
        //temp = 
        return temp;
    }

    public static Move.SubMove RandMove()
    {
        Random.InitState((int)System.DateTime.Now.Ticks);
        int i = (int)Random.Range((int)0, (int)instance.allmoves.Length);
        Move.SubMove temp = new Move.SubMove(instance.allmoves[i].themove);
        return temp;
    }


    // Use this for initialization
    void Start () {
        if (curInstance == null) { curInstance = this; }

        allmonsters = Resources.LoadAll<Monster>("Monsters");
        allmoves = Resources.LoadAll<Move>("Moves");
    }
	


	// Update is called once per frame
	/*void Update () {
		
	}*/
}
